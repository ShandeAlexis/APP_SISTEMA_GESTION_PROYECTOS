import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getProyectoById, updateProyecto } from "../../services/proyectosService";
import "./ProyectoEdit.css";

const ProyectoEdit = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [proyecto, setProyecto] = useState({
    codigo: "",
    descripcion: "",
    fechaInicio: "",
    fechaFin: "",
    capex: "",
    estimado: "",
    codigoEstado: "ACT",
  });

  useEffect(() => {
    const loadProyecto = async () => {
      try {
        const res = await getProyectoById(id);

        const data = {
          ...res.data,
          fechaInicio: res.data.fechaInicio
            ? res.data.fechaInicio.substring(0, 10)
            : "",
          fechaFin: res.data.fechaFin
            ? res.data.fechaFin.substring(0, 10)
            : "",
        };

        setProyecto(data);
      } catch (err) {
        console.error("Error al cargar proyecto", err);
      }
    };
    loadProyecto();
  }, [id]);

  const handleChange = (e) => {
    setProyecto({ ...proyecto, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await updateProyecto(id, proyecto);
      navigate("/proyectos"); // volver al listado
    } catch (err) {
      console.error("Error al actualizar proyecto", err);
    }
  };

  return (
    <div className="edit-container">
      <h2>Editar Proyecto</h2>
      <form onSubmit={handleSubmit} className="edit-form">
        <label>
          Código:
          <input
            type="text"
            name="codigo"
            value={proyecto.codigo}
            onChange={handleChange}
          />
        </label>
        <label>
          Descripción:
          <input
            type="text"
            name="descripcion"
            value={proyecto.descripcion}
            onChange={handleChange}
          />
        </label>
        <label>
          Fecha Inicio:
          <input
            type="date"
            name="fechaInicio"
            value={proyecto.fechaInicio}
            onChange={handleChange}
          />
        </label>
        <label>
          Fecha Fin:
          <input
            type="date"
            name="fechaFin"
            value={proyecto.fechaFin}
            onChange={handleChange}
          />
        </label>
        <label>
          Capex:
          <input
            type="number"
            name="capex"
            value={proyecto.capex}
            onChange={handleChange}
          />
        </label>
        <label>
          Estimado:
          <input
            type="number"
            name="estimado"
            value={proyecto.estimado}
            onChange={handleChange}
          />
        </label>
        <label>
          Estado:
          <select
            name="codigoEstado"
            value={proyecto.codigoEstado}
            onChange={handleChange}
          >
            <option value="ACT">Activo</option>
            <option value="FIN">Finalizado</option>
          </select>
        </label>

        <div className="form-buttons">
          <button type="submit">Guardar</button>
          <button type="button" onClick={() => navigate("/proyectos")}>
            Cancelar
          </button>
        </div>
      </form>
    </div>
  );
};

export default ProyectoEdit;
