import React, { useEffect, useState, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  getEntregablesByContrato,
  deleteEntregable,
} from "../../services/entregablesService";
import {
  getCurvasByEntregable,
  updateCurva,
} from "../../services/curvasService";
import Table from "../../components/Table";
import Chart from "chart.js/auto";
import "./Entregable.css";

const EntregablesList = () => {
  const { contratoId, id } = useParams();
  const navigate = useNavigate();
  const [entregables, setEntregables] = useState([]);
  const [curva, setCurva] = useState(null); // array de curvas (plan + real)
  const [editingData, setEditingData] = useState({}); // datos editables
  const chartRef = useRef(null);
  const chartInstance = useRef(null);

  useEffect(() => {
    const loadData = async () => {
      try {
        const res = await getEntregablesByContrato(contratoId);
        setEntregables(res.data);
      } catch (err) {
        console.error("❌ Error al cargar entregables", err);
      }
    };
    loadData();
  }, [contratoId]);

  const handleDelete = async (entregableId) => {
    if (window.confirm("¿Eliminar este entregable?")) {
      try {
        await deleteEntregable(entregableId);
        const res = await getEntregablesByContrato(contratoId);
        setEntregables(res.data);
      } catch (err) {
        console.error("❌ Error al eliminar entregable", err);
      }
    }
  };

  const handleVerCurva = async (entregableId) => {
    try {
      const res = await getCurvasByEntregable(entregableId);
      setCurva(res.data);

      // inicializamos los datos editables
      const editState = {};
      res.data.forEach((c) => {
        editState[c.id] = c.detalles.map((d) => ({
          fecha: d.fecha,
          valor: d.valor,
          valorAcumulado: d.valorAcumulado,
        }));
      });
      setEditingData(editState);
    } catch (err) {
      console.error("❌ Error al cargar curva", err);
    }
  };

  const handleInputChange = (curvaId, index, field, value) => {
    setEditingData((prev) => {
      const updated = { ...prev };
      updated[curvaId][index][field] =
        field === "fecha" ? value : parseFloat(value);
      return updated;
    });
  };

  const handleSaveCurva = async (curvaId) => {
    try {
      await updateCurva(curvaId, editingData[curvaId]);
      alert("✅ Curva actualizada correctamente");

      // refrescamos curva en gráfico
      const res = await getCurvasByEntregable(curva[0].origenId);
      setCurva(res.data);
    } catch (err) {
      console.error("❌ Error al actualizar curva", err);
      alert("❌ Error al actualizar curva");
    }
  };

  // Render Chart cuando cambia curva
  useEffect(() => {
    if (curva && chartRef.current) {
      const ctx = chartRef.current.getContext("2d");

      if (chartInstance.current) {
        chartInstance.current.destroy();
      }

      const plan = curva.find((c) => c.tipoCurvaCodigo === "CRV_PLAN_VIG");
      const real = curva.find((c) => c.tipoCurvaCodigo === "CRV_REAL_VIG");

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
          plugins: {
            legend: { position: "top" },
          },
          scales: {
            y: { beginAtZero: true, title: { display: true, text: "%" } },
            x: { title: { display: true, text: "Mes/Año" } },
          },
        },
      });
    }
  }, [curva]);

  return (
    <div className="entregables-container">
      <div className="header">
        <h2>📦 Entregables del Contrato {contratoId}</h2>
        <button
          className="nuevo-btn"
          onClick={() =>
            navigate(`/proyectos/${id}/contratos/${contratoId}/entregables/nuevo`)
          }
        >
          + Nuevo Entregable
        </button>
      </div>

      <div className="table-wrapper">
        <Table
          headers={[
            "codigo",
            "pctContrato",
            "fechaInicialPlan",
            "duracionPlanDias",
            "tipoEntregableCodigo",
            "acciones",
          ]}
          data={entregables.map((e) => ({
            ...e,
            acciones: (
              <div className="acciones-btns">
                <button
                  className="edit-btn"
                  onClick={() =>
                    navigate(
                      `/proyectos/${id}/contratos/${contratoId}/entregables/editar/${e.id}`
                    )
                  }
                >
                  ✏️
                </button>
                <button
                  className="delete-btn"
                  onClick={() => handleDelete(e.id)}
                >
                  🗑️
                </button>
                <button
                  className="view-btn"
                  onClick={() => handleVerCurva(e.id)}
                >
                  📈
                </button>
              </div>
            ),
          }))}
        />
      </div>

      {curva && (
        <div className="curva-card">
          <h3>📈 Curva del Entregable</h3>
          <div className="chart-wrapper">
            <canvas ref={chartRef}></canvas>
          </div>

          {/* 🔹 Formulario de edición en formato horizontal */}
          {curva.map((c) => (
            <div key={c.id} className="curva-form">
              <h4>
                {c.tipoCurvaCodigo === "CRV_PLAN_VIG" ? "📊 Plan" : "📊 Real"}
              </h4>
              <table>
                <thead>
                  <tr>
                    <th></th>
                    {editingData[c.id]?.map((d, i) => (
                      <th key={i}>
                        {new Date(d.fecha).toLocaleDateString("es-ES", {
                          month: "short",
                          year: "numeric",
                        })}
                      </th>
                    ))}
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td>Valor</td>
                    {editingData[c.id]?.map((d, i) => (
                      <td key={i}>
                        <input
                          type="number"
                          value={d.valor}
                          onChange={(e) =>
                            handleInputChange(c.id, i, "valor", e.target.value)
                          }
                        />
                      </td>
                    ))}
                  </tr>
                  <tr>
                    <td>Valor Acumulado</td>
                    {editingData[c.id]?.map((d, i) => (
                      <td key={i}>
                        <input
                          type="number"
                          value={d.valorAcumulado}
                          onChange={(e) =>
                            handleInputChange(
                              c.id,
                              i,
                              "valorAcumulado",
                              e.target.value
                            )
                          }
                        />
                      </td>
                    ))}
                  </tr>
                </tbody>
              </table>
              <button
                className="save-btn"
                onClick={() => handleSaveCurva(c.id)}
              >
                💾 Guardar {c.tipoCurvaCodigo}
              </button>
            </div>
          ))}
        </div>
      )}

      <div className="footer-btns">
        <button
          className="back-btn"
          onClick={() => navigate(`/proyectos/${id}/contratos`)}
        >
          ⬅️ Volver a Contratos
        </button>
      </div>
    </div>
  );
};

export default EntregablesList;
