import React, { useEffect, useState } from "react";
import { getProyectos, getReporte } from "../../services/proyectosService";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  Legend,
  CartesianGrid,
  RadialBarChart,
  RadialBar,
  ResponsiveContainer,
} from "recharts";
import jsPDF from "jspdf";
import html2canvas from "html2canvas";
import "./Reporte.css";

const ReporteMain = () => {
  const [proyectos, setProyectos] = useState([]);
  const [selectedProyecto, setSelectedProyecto] = useState("");
  const [fecha, setFecha] = useState("");
  const [reporte, setReporte] = useState(null);

  useEffect(() => {
    const fetchProyectos = async () => {
      try {
        const res = await getProyectos();
        setProyectos(res.data);
      } catch (err) {
        console.error("❌ Error al cargar proyectos", err);
      }
    };
    fetchProyectos();
  }, []);

  const handleGenerar = async () => {
    if (!selectedProyecto || !fecha) {
      alert("⚠️ Debes seleccionar un proyecto y una fecha");
      return;
    }

    try {
      const res = await getReporte(selectedProyecto, fecha);
      setReporte(res.data);
    } catch (err) {
      console.error("❌ Error al generar reporte", err);
    }
  };

  const handleExportPDF = () => {
    const input = document.getElementById("reporteResult");
    if (!input) return;

    html2canvas(input, { scale: 2 }).then((canvas) => {
      const imgData = canvas.toDataURL("image/png");
      const pdf = new jsPDF("p", "mm", "a4"); // 👈 portrait mejor para varias páginas

      const pageWidth = pdf.internal.pageSize.getWidth();
      const pageHeight = pdf.internal.pageSize.getHeight();

      const imgWidth = pageWidth - 20; // margenes
      const imgHeight = (canvas.height * imgWidth) / canvas.width;

      let heightLeft = imgHeight;
      let position = 10;

      // primera página
      pdf.addImage(imgData, "PNG", 10, position, imgWidth, imgHeight);
      heightLeft -= pageHeight;

      // páginas adicionales si sobra contenido
      while (heightLeft > 0) {
        position = heightLeft - imgHeight + 10;
        pdf.addPage();
        pdf.addImage(imgData, "PNG", 10, position, imgWidth, imgHeight);
        heightLeft -= pageHeight;
      }

      pdf.save(`Reporte_${reporte.proyectoCodigo}_${fecha}.pdf`);
    });
  };

  return (
    <div className="reporte-container">
      <h2>📊 Reportes</h2>

      {/* Formulario */}
      <div className="form-reporte">
        <div className="field">
          <label>Proyecto</label>
          <select
            value={selectedProyecto}
            onChange={(e) => setSelectedProyecto(e.target.value)}
          >
            <option value="">-- Selecciona un proyecto --</option>
            {proyectos.map((p) => (
              <option key={p.id} value={p.id}>
                {p.nombre || p.codigo}
              </option>
            ))}
          </select>
        </div>

        <div className="field">
          <label>Fecha del reporte</label>
          <input
            type="date"
            value={fecha}
            onChange={(e) => setFecha(e.target.value)}
          />
        </div>

        <div className="btn-actions">
          <button
            className="generar-btn"
            onClick={handleGenerar}
            disabled={!selectedProyecto || !fecha}
          >
            🚀 Generar
          </button>

          {reporte && (
            <button className="export-btn" onClick={handleExportPDF}>
              📄 Exportar PDF
            </button>
          )}
        </div>
      </div>

      {/* Resultados */}
      <div className="reporte-result" id="reporteResult">
        {!reporte ? (
          <small>Resultados del reporte aparecerán aquí...</small>
        ) : (
          <div>
            <h3>📌 Proyecto: {reporte.proyectoCodigo}</h3>
            <p>
              📅 Fecha corte:{" "}
              {new Date(reporte.fechaCorte).toLocaleDateString()}
            </p>

            {/* Tabla */}
            <table className="reporte-table">
              <thead>
                <tr>
                  <th>EDT</th>
                  <th>Capex</th>
                  <th>Estimado</th>
                  <th>Plan (%)</th>
                  <th>Real (%)</th>
                  <th>Índice Cumpl. Físico</th>
                  <th>Plan Económico</th>
                  <th>Real Económico</th>
                  <th>Índice Cumpl. Económico</th>
                </tr>
              </thead>
              <tbody>
                {reporte.detalles.map((d, i) => (
                  <tr
                    key={i}
                    className={d.codigoEDT === "TOTAL" ? "total-row" : ""}
                  >
                    <td>{d.codigoEDT}</td>
                    <td>{d.capex.toLocaleString()}</td>
                    <td>{d.estimado.toLocaleString()}</td>
                    <td>{d.fisPlan.toFixed(2)}%</td>
                    <td>{d.fisReal.toFixed(2)}%</td>
                    <td>{(d.fisIndCum * 100).toFixed(2)}%</td>
                    <td>{d.ecoPlan.toLocaleString()}</td>
                    <td>{d.ecoReal.toLocaleString()}</td>
                    <td>{(d.ecoIndCum * 100).toFixed(2)}%</td>
                  </tr>
                ))}
              </tbody>
            </table>

            {/* 🔹 Gráficos */}
            <div className="charts-container">
              <h4>📈 Avance Físico (Plan vs Real)</h4>
              <ResponsiveContainer width="100%" height={300}>
                <BarChart data={reporte.detalles}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="codigoEDT" />
                  <YAxis />
                  <Tooltip />
                  <Legend />
                  <Bar dataKey="fisPlan" fill="#8884d8" name="Plan (%)" />
                  <Bar dataKey="fisReal" fill="#82ca9d" name="Real (%)" />
                </BarChart>
              </ResponsiveContainer>

              <h4>💰 Avance Económico (Plan vs Real)</h4>
              <ResponsiveContainer width="100%" height={300}>
                <BarChart data={reporte.detalles}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="codigoEDT" />
                  <YAxis />
                  <Tooltip />
                  <Legend />
                  <Bar dataKey="ecoPlan" fill="#ffc658" name="Plan ($)" />
                  <Bar dataKey="ecoReal" fill="#ff7300" name="Real ($)" />
                </BarChart>
              </ResponsiveContainer>

              {/* Gauges */}
              <h4>🎯 Índices de Cumplimiento Totales</h4>
              <div className="gauges">
                {/* Gauge Físico */}
                <ResponsiveContainer width={250} height={250}>
                  <RadialBarChart
                    cx="50%"
                    cy="50%"
                    innerRadius="70%"
                    outerRadius="100%"
                    barSize={18}
                    data={[
                      {
                        name: "Cumplimiento Físico",
                        value:
                          (reporte.detalles.find((d) => d.codigoEDT === "TOTAL")
                            ?.fisIndCum || 0) * 100,
                        fill: "#4caf50",
                      },
                    ]}
                  >
                    <RadialBar
                      minAngle={15}
                      background
                      clockWise
                      dataKey="value"
                      cornerRadius={10}
                    />
                    <Tooltip />
                    <text
                      x="50%"
                      y="50%"
                      textAnchor="middle"
                      dominantBaseline="middle"
                      style={{
                        fontSize: "22px",
                        fontWeight: "bold",
                        fill: "#4caf50",
                      }}
                    >
                      {(
                        (reporte.detalles.find((d) => d.codigoEDT === "TOTAL")
                          ?.fisIndCum || 0) * 100
                      ).toFixed(1)}
                      %
                    </text>
                    <text
                      x="50%"
                      y="65%"
                      textAnchor="middle"
                      dominantBaseline="middle"
                      style={{ fontSize: "13px", fill: "#ccc" }}
                    >
                      Físico
                    </text>
                  </RadialBarChart>
                </ResponsiveContainer>

                {/* Gauge Económico */}
                <ResponsiveContainer width={250} height={250}>
                  <RadialBarChart
                    cx="50%"
                    cy="50%"
                    innerRadius="70%"
                    outerRadius="100%"
                    barSize={18}
                    data={[
                      {
                        name: "Cumplimiento Económico",
                        value:
                          (reporte.detalles.find((d) => d.codigoEDT === "TOTAL")
                            ?.ecoIndCum || 0) * 100,
                        fill: "#ff9800",
                      },
                    ]}
                  >
                    <RadialBar
                      minAngle={15}
                      background
                      clockWise
                      dataKey="value"
                      cornerRadius={10}
                    />
                    <Tooltip />
                    <text
                      x="50%"
                      y="50%"
                      textAnchor="middle"
                      dominantBaseline="middle"
                      style={{
                        fontSize: "22px",
                        fontWeight: "bold",
                        fill: "#ff9800",
                      }}
                    >
                      {(
                        (reporte.detalles.find((d) => d.codigoEDT === "TOTAL")
                          ?.ecoIndCum || 0) * 100
                      ).toFixed(1)}
                      %
                    </text>
                    <text
                      x="50%"
                      y="65%"
                      textAnchor="middle"
                      dominantBaseline="middle"
                      style={{ fontSize: "13px", fill: "#ccc" }}
                    >
                      Económico
                    </text>
                  </RadialBarChart>
                </ResponsiveContainer>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default ReporteMain;
