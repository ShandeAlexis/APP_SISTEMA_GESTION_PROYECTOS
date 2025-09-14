import React, { useEffect, useState } from "react";
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
import CurvaChart from "../../components/CurvaChart/CurvaChart";
import CurvaForm from "../../components/CurvaForm/CurvaForm";
import "./Entregable.css";

const EntregablesList = () => {
  const { contratoId, id } = useParams();
  const navigate = useNavigate();
  const [entregables, setEntregables] = useState([]);
  const [curva, setCurva] = useState(null);
  const [editingData, setEditingData] = useState({});

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

  return (
    <div className="entregables-container">
      <div className="header">
        <h2>
          📦 Entregables del Contrato{" "}
          {entregables.length > 0 ? entregables[0].contratoCodigo : contratoId}
        </h2>
        <button
          className="nuevo-btn"
          onClick={() =>
            navigate(
              `/proyectos/${id}/contratos/${contratoId}/entregables/nuevo`
            )
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
          <CurvaChart curvas={curva} />
          <div className="curvas-wrapper">
            {curva.map((c) => (
              <CurvaForm
                key={c.id}
                curva={c}
                editingData={editingData}
                setEditingData={setEditingData}
                onSave={handleSaveCurva}
              />
            ))}
          </div>
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
