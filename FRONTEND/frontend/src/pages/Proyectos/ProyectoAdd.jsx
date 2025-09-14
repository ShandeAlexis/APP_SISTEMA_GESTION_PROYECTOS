import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createProyecto } from "../../services/proyectosService";
import "./ProyectoEdit.css";

const ProyectoAdd = () => {
  const navigate = useNavigate();
  const [proyecto, setProyecto] = useState({
    codigo: "",
    descripcion: "",
    fechaInicio: "",
    fechaFin: "",
    capex: "",
    estimado: "",
    codigoEstado: "ACT",
    codigoEmpresa: "EMP002", // 👈 obligatorio para backend
  });

  const handleChange = (e) => {
    setProyecto({ ...proyecto, [e.target.name]: e.target.value });
  };

  const formatDate = (date) => {
    return date ? `${date}T00:00:00` : null;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const payload = {
        ...proyecto,
        fechaInicio: formatDate(proyecto.fechaInicio),
        fechaFin: formatDate(proyecto.fechaFin),
        capex: parseFloat(proyecto.capex || 0),
        estimado: parseFloat(proyecto.estimado || 0),
      };

      console.log("📤 Enviando payload:", payload);

      await createProyecto(payload);
      navigate("/proyectos");
    } catch (err) {
      console.error("Error al crear proyecto", err);
    }
  };

  return (
    <div className="edit-container">
      <h2>➕ Nuevo Proyecto</h2>
      <form onSubmit={handleSubmit} className="edit-form">
        <label>
          Código:
          <input
            type="text"
            name="codigo"
            value={proyecto.codigo}
            onChange={handleChange}
            required
          />
        </label>
        <label>
          Descripción:
          <input
            type="text"
            name="descripcion"
            value={proyecto.descripcion}
            onChange={handleChange}
            required
          />
        </label>
        <label>
          Fecha Inicio:
          <input
            type="date"
            name="fechaInicio"
            value={proyecto.fechaInicio}
            onChange={handleChange}
            required
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

export default ProyectoAdd;
