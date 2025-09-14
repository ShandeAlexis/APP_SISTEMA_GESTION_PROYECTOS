import React, { useEffect, useRef } from "react";
import Chart from "chart.js/auto";
import "./CurvaChart.css";

const CurvaChart = ({ curvas }) => {
  const chartRef = useRef(null);
  const chartInstance = useRef(null);

  useEffect(() => {
    if (curvas && chartRef.current) {
      const ctx = chartRef.current.getContext("2d");

      if (chartInstance.current) chartInstance.current.destroy();

      const plan = curvas.find((c) => c.tipoCurvaCodigo === "CRV_PLAN_VIG");
      const real = curvas.find((c) => c.tipoCurvaCodigo === "CRV_REAL_VIG");

      const labels = plan?.detalles.map((d) =>
        new Date(d.fecha).toLocaleDateString("es-ES", {
          month: "short",
          year: "numeric",
        })
      );

      chartInstance.current = new Chart(ctx, {
        type: "line",
        data: {
          labels,
          datasets: [
            {
              label: "Plan",
              data: plan?.detalles.map((d) => d.valorAcumulado),
              borderColor: "#4e73df",
              fill: false,
              tension: 0.3,
            },
            {
              label: "Real",
              data: real?.detalles.map((d) => d.valorAcumulado),
              borderColor: "#e74a3b",
              fill: false,
              tension: 0.3,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: { legend: { position: "top" } },
          scales: {
            y: { beginAtZero: true, title: { display: true, text: "%" } },
            x: { title: { display: true, text: "Mes/Año" } },
          },
        },
      });
    }
  }, [curvas]);

  return (
    <div className="curva-chart-wrapper">
      <canvas ref={chartRef}></canvas>
    </div>
  );
};

export default CurvaChart;
