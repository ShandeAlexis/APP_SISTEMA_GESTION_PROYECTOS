using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SISTEMA.API.SISTEMAS_api.Core.Interfaces;
using SISTEMA.API.SISTEMAS_API.BD.Entities;

namespace SISTEMA.API.SISTEMAS_api.Core.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IProyectoService proyectoService;
        private readonly IContratoService contratoService;
        private readonly IEntregableService entregableService;
        private readonly ICurvaService curvaService;

        public ReporteService(
            IProyectoService proyectoSrv,
            IContratoService contratoSrv,
            IEntregableService entregableSrv,
            ICurvaService curvaSrv)
        {
            proyectoService = proyectoSrv;
            contratoService = contratoSrv;
            entregableService = entregableSrv;
            curvaService = curvaSrv;
        }

        public async Task<ReporteDTO> GenerarReporteAsync(int proyectoId, DateTime fechaCorte)
        {
            var proyecto = await proyectoService.GetProyecto(proyectoId);
            if (proyecto == null)
                throw new Exception("Proyecto no encontrado.");

            var contratos = await contratoService.GetContratosByProyectoId(proyectoId);

            var reporte = new ReporteDTO
            {
                ProyectoCodigo = proyecto.Codigo,
                FechaCorte = fechaCorte
            };

            // 🔹 acumulamos por EDT (no por contrato)
            var acumulados = new Dictionary<string, DetalleReporteDTO>(StringComparer.OrdinalIgnoreCase);

            foreach (var contrato in contratos)
            {
                var entregables = await entregableService.GetEntregablesByContratoId(contrato.Id);

                foreach (var e in entregables)
                {
                    var key = e.EDTchCodigo ?? "-";

                    if (!acumulados.TryGetValue(key, out var detalle))
                    {
                        detalle = new DetalleReporteDTO
                        {
                            CodigoEDT = key,
                            ContratoCodigo = "-", // ya no por contrato
                            Capex = 0m,
                            Estimado = 0m
                        };
                        acumulados[key] = detalle;
                    }

                    // 🔹 Calcular monto del entregable según % contrato
                    var pct = (decimal?)(e.PctContrato) ?? 0m;
                    decimal monto = 0m;

                    // Traer curvas
                    var curvas = await curvaService.GetCurvasByEntregableId(e.Id);
                    var curvaPlan = curvas.FirstOrDefault(c => c.TipoCurvaCodigo == "CRV_PLAN_VIG");
                    var curvaReal = curvas.FirstOrDefault(c => c.TipoCurvaCodigo == "CRV_REAL_VIG");

                    if (e.TipoEntregableCodigo == "FIS")
                    {
                        monto = (contrato.Capex ?? 0m) * pct;
                        detalle.Capex += monto;

                        decimal fisPlanPct = 0m, fisRealPct = 0m;

                        // 🔹 Guardar fechas SOLO para entregables FIS
                        if (curvaPlan != null && curvaPlan.Detalles.Any())
                        {
                            var primeroPlan = curvaPlan.Detalles.OrderBy(d => d.Fecha).FirstOrDefault();
                            var ultimoPlan = curvaPlan.Detalles
                                .Where(d => d.ValorAcumulado > 0)
                                .OrderByDescending(d => d.Fecha)
                                .FirstOrDefault();

                            detalle.FechaIniPlan = primeroPlan?.Fecha;
                            detalle.FechaFinPlan = ultimoPlan?.Fecha;

                            var ultimoPlanCorte = curvaPlan.Detalles
                                .Where(d => d.Fecha <= fechaCorte)
                                .OrderByDescending(d => d.Fecha)
                                .FirstOrDefault();

                            fisPlanPct = ultimoPlanCorte?.ValorAcumulado ?? 0m;
                        }

                        if (curvaReal != null && curvaReal.Detalles.Any())
                        {
                            var primeroReal = curvaReal.Detalles.OrderBy(d => d.Fecha).FirstOrDefault();
                            var ultimoReal = curvaReal.Detalles
                                .Where(d => d.ValorAcumulado > 0)
                                .OrderByDescending(d => d.Fecha)
                                .FirstOrDefault();

                            detalle.FechaIniReal = primeroReal?.Fecha;
                            detalle.FechaFinReal = ultimoReal?.Fecha;

                            var ultimoRealCorte = curvaReal.Detalles
                                .Where(d => d.Fecha <= fechaCorte)
                                .OrderByDescending(d => d.Fecha)
                                .FirstOrDefault();

                            fisRealPct = ultimoRealCorte?.ValorAcumulado ?? 0m;
                        }

                        detalle.FisPlan += fisPlanPct * monto;
                        detalle.FisReal += fisRealPct * monto;
                    }
                    else if (e.TipoEntregableCodigo == "ECO")
                    {
                        monto = (contrato.Estimado ?? 0m) * pct;
                        detalle.Estimado += monto;

                        decimal ecoPlanPct = 0m, ecoRealPct = 0m;

                        if (curvaPlan != null && curvaPlan.Detalles.Any())
                        {
                            var ultimoPlan = curvaPlan.Detalles
                                .Where(d => d.Fecha <= fechaCorte)
                                .OrderByDescending(d => d.Fecha)
                                .FirstOrDefault();

                                    ecoPlanPct = (ultimoPlan?.ValorAcumulado ?? 0m) / 100m; // 👈 dividir entre 100

                        }

                        if (curvaReal != null && curvaReal.Detalles.Any())
                        {
                            var ultimoReal = curvaReal.Detalles
                                .Where(d => d.Fecha <= fechaCorte)
                                .OrderByDescending(d => d.Fecha)
                                .FirstOrDefault();

                            ecoRealPct = (ultimoReal?.ValorAcumulado ?? 0m) / 100m; // 👈 dividir entre 100
                        }

                        detalle.EcoPlan += monto * ecoPlanPct;
                        detalle.EcoReal += monto * ecoRealPct;
                    }
                }
            }

            // 🔹 Normalizar (sacar porcentajes ponderados)
            foreach (var d in acumulados.Values)
            {
                if (d.Capex > 0)
                {
                    d.FisPlan = d.FisPlan / d.Capex;
                    d.FisReal = d.FisReal / d.Capex;
                    d.FisIndCum = d.FisPlan > 0 ? d.FisReal / d.FisPlan : 0;
                }

                if (d.Estimado > 0)
                {
                    d.EcoIndCum = d.EcoPlan > 0 ? d.EcoReal / d.EcoPlan : 0;
                }
            }

            reporte.Detalles = acumulados.Values.OrderBy(x => x.CodigoEDT).ToList();

            // 🔹 Agregar TOTAL
            if (reporte.Detalles.Any())
            {
                var total = new DetalleReporteDTO
                {
                    CodigoEDT = "TOTAL",
                    ContratoCodigo = "-",
                    Capex = reporte.Detalles.Sum(d => d.Capex),
                    Estimado = reporte.Detalles.Sum(d => d.Estimado),

                    // 🔹 Fechas totales SOLO de FIS
                    FechaIniPlan = reporte.Detalles.Where(d => d.FechaIniPlan.HasValue).Min(d => d.FechaIniPlan),
                    FechaFinPlan = reporte.Detalles.Where(d => d.FechaFinPlan.HasValue).Max(d => d.FechaFinPlan),
                    FechaIniReal = reporte.Detalles.Where(d => d.FechaIniReal.HasValue).Min(d => d.FechaIniReal),
                    FechaFinReal = reporte.Detalles.Where(d => d.FechaFinReal.HasValue).Max(d => d.FechaFinReal)
                };

                if (total.Capex > 0)
                {
                    total.FisPlan = reporte.Detalles.Sum(d => d.FisPlan * d.Capex) / total.Capex;
                    total.FisReal = reporte.Detalles.Sum(d => d.FisReal * d.Capex) / total.Capex;
                    total.FisIndCum = total.FisPlan > 0 ? total.FisReal / total.FisPlan : 0;
                }

                if (total.Estimado > 0)
                {
                    total.EcoPlan = reporte.Detalles.Sum(d => d.EcoPlan);
                    total.EcoReal = reporte.Detalles.Sum(d => d.EcoReal);
                    total.EcoIndCum = total.EcoPlan > 0 ? total.EcoReal / total.EcoPlan : 0;
                }

                reporte.Detalles.Add(total);
            }

            return reporte;
        }
    }
}
