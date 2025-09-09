import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Table from "../../components/Table";
import { getProyectos, deleteProyecto } from "../../services/proyectosService";

const ProyectosList = () => {
  const [proyectos, setProyectos] = useState([]);
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

  return (
    <div>
      <h2>Proyectos</h2>
      <button onClick={() => navigate("/proyectos/nuevo")}>+ Nuevo Proyecto</button>

      <Table
        headers={[ "codigo", "descripcion", "fechaInicio", "capex" , "estimado"]}
        data={proyectos}
        onEdit={(id) => navigate(`/proyectos/editar/${id}`)}
        onDelete={handleDelete}
      />
    </div>
  );
};

export default ProyectosList;
