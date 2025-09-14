import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {
  getContratosByProyecto,
  deleteContrato,
} from "../../services/contratosService";
import Table from "../../components/Table";
import "./Contratos.css";

const ContratosList = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [contratos, setContratos] = useState([]);

  useEffect(() => {
    const loadData = async () => {
      try {
        const res = await getContratosByProyecto(id);
        console.log("📥 Contratos recibidos:", res.data);
        setContratos(res.data);
      } catch (err) {
        console.error("Error al cargar contratos", err);
      }
    };

    loadData();
  }, [id]);

  const handleDelete = async (contratoId) => {
    if (window.confirm("¿Eliminar este contrato?")) {
      try {
        await deleteContrato(contratoId);
        const res = await getContratosByProyecto(id);
        setContratos(res.data);
      } catch (err) {
        console.error("Error al eliminar contrato", err);
      }
    }
  };

  return (
    <div className="contratos-container">
      <div className="header">
        <h2>
          📑 Contratos del Proyecto{" "}
          {contratos.length > 0 ? contratos[0].proyectoCodigo : id}
        </h2>

        <button
          className="nuevo-btn"
          onClick={() => navigate(`/proyectos/${id}/contratos/nuevo`)}
        >
          + Nuevo Contrato
        </button>
      </div>

      <div className="table-wrapper">
        <Table
          headers={[
            "codigo",
            "contratista",
            "alcance",
            "fechaInicial",
            "fechaFinal",
            "capex",
            "estimado",
            "entregables",
            "acciones",
          ]}
          data={contratos.map((c) => ({
            ...c,
            entregables: (
              <button
                className="view-btn"
                onClick={() =>
                  navigate(`/proyectos/${id}/contratos/${c.id}/entregables`)
                }
              >
                👁️ Ver
              </button>
            ),
            acciones: (
              <div className="acciones-btns">
                <button
                  className="edit-btn"
                  onClick={() =>
                    navigate(`/proyectos/${id}/contratos/editar/${c.id}`)
                  }
                >
                  ✏️
                </button>
                <button
                  className="delete-btn"
                  onClick={() => handleDelete(c.id)}
                >
                  🗑️
                </button>
              </div>
            ),
          }))}
        />
      </div>

      <div className="footer-btns">
        <button className="back-btn" onClick={() => navigate("/proyectos")}>
          ⬅️ Volver a Proyectos
        </button>
      </div>
    </div>
  );
};

export default ContratosList;
