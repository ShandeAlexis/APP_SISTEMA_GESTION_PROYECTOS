import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Table from "../../components/Table";
import { getProyectos, deleteProyecto } from "../../services/proyectosService";
import "./Proyectos.css";

const ProyectosList = () => {
  const [proyectos, setProyectos] = useState([]);
  const [filtro, setFiltro] = useState("todos");
  const navigate = useNavigate();

  const loadData = async () => {
    const res = await getProyectos();
    setProyectos(res.data);
  };

  useEffect(() => {
    loadData();
  }, []);

  const handleDelete = async (id) => {
    if (window.confirm("¿Eliminar este proyecto?")) {
      await deleteProyecto(id);
      loadData();
    }
  };

  const filteredProyectos = proyectos.filter((p) =>
    filtro === "todos" ? true : p.codigoEstado === filtro
  );

  return (
    <div className="proyectos-container">
      <div className="header">
        <h2>📋 Listado de Proyectos</h2>
        <div className="filter">
          <select value={filtro} onChange={(e) => setFiltro(e.target.value)}>
            <option value="todos">Todos</option>
            <option value="ACT">Activos</option>
            <option value="FIN">Finalizados</option>
          </select>
          <button
            className="nuevo-btn"
            onClick={() => navigate("/proyectos/nuevo")}
          >
            + Nuevo Proyecto
          </button>
        </div>
      </div>

      <Table
        headers={[
          "codigo",
          "descripcion",
          "fechaInicio",
          "fechaFin",
          "capex",
          "estimado",
          "codigoEstado",
          "contratos",
          "acciones", 
        ]}
        data={filteredProyectos.map((p) => ({
          ...p,
          contratos: (
            <button
              className="contratos-btn"
              onClick={() => navigate(`/proyectos/${p.id}/contratos`)}
            >
              👁
            </button>
          ),
          acciones: (
            <div className="acciones-btns">
              <button
                className="edit-btn"
                onClick={() => navigate(`/proyectos/editar/${p.id}`)}
              >
                ✏️
              </button>
              <button
                className="delete-btn"
                onClick={() => handleDelete(p.id)}
              >
                🗑️
              </button>
            </div>
          ),
        }))}
      />
    </div>
  );
};

export default ProyectosList;
