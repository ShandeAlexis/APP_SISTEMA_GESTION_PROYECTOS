import React, { useEffect, useRef } from "react";
import Chart from "chart.js/auto";
import "./CurvaChart.css";

const CurvaChart2 = ({ curvas }) => {
  const chartRef = useRef(null);
  const chartInstance = useRef(null);

  useEffect(() => {
    if (!curvas || curvas.length === 0) return;

    const ctx = chartRef.current.getContext("2d");
    if (chartInstance.current) chartInstance.current.destroy();

    // ✅ Unificar TODAS las fechas de todas las curvas (por mes/año)
    const fechasMap = new Map();
    curvas.forEach((curva) => {
      curva.detalles.forEach((d) => {
        const fechaObj = new Date(d.fecha);
        const clave = `${fechaObj.getFullYear()}-${fechaObj.getMonth() + 1}`;
        if (!fechasMap.has(clave)) fechasMap.set(clave, fechaObj);
      });
    });

    const fechas = Array.from(fechasMap.values()).sort((a, b) => a - b);
    const labels = fechas.map((f) =>
      f.toLocaleDateString("es-ES", { month: "short", year: "2-digit" })
    );

    // ✅ Crear datasets alineados a las fechas globales
    const datasets = curvas.map((curva) => {
      const color =
        {
          CRV_PLAN_FIS: "#4e73df",
          CRV_REAL_FIS: "#e74a3b",
          CRV_PLAN_ECO: "#1cc88a",
          CRV_REAL_ECO: "#f6c23e",
        }[curva.tipoCurvaCodigo] || "#999";

      const data = fechas.map((f) => {
        const punto = curva.detalles.find((d) => {
          const df = new Date(d.fecha);
          return (
            df.getMonth() === f.getMonth() && df.getFullYear() === f.getFullYear()
          );
        });
        return punto ? punto.valorAcumulado : null;
      });

      return {
        label:
          {
            CRV_PLAN_FIS: "Plan Físico",
            CRV_REAL_FIS: "Real Físico",
            CRV_PLAN_ECO: "Plan Económico",
            CRV_REAL_ECO: "Real Económico",
          }[curva.tipoCurvaCodigo] || curva.tipoCurvaCodigo,
        data,
        borderColor: color,
        fill: false,
        tension: 0.3,
        spanGaps: true,
      };
    });

    chartInstance.current = new Chart(ctx, {
      type: "line",
      data: {
        labels,
        datasets,
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { position: "top", labels: { color: "#fff" } },
          tooltip: {
            callbacks: {
              label: (context) =>
                `${context.dataset.label}: ${context.parsed.y?.toFixed(2)}%`,
            },
          },
        },
        scales: {
          x: {
            ticks: { color: "#ccc" },
            title: { display: true, text: "Mes/Año", color: "#fff" },
          },
          y: {
            beginAtZero: true,
            ticks: { color: "#ccc" },
            title: { display: true, text: "% Acumulado", color: "#fff" },
          },
        },
      },
    });
  }, [curvas]);

  return (
    <div className="curva-chart-wrapper" style={{ height: "400px" }}>
      <canvas ref={chartRef}></canvas>
    </div>
  );
};

export default CurvaChart2;
