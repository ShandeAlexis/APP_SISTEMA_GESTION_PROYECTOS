import React, { useEffect, useState } from "react";
import { getProyectos } from "../../services/proyectosService";
import "./Reporte.css";

const ReporteMain = () => {
  const [proyectos, setProyectos] = useState([]);
  const [selectedProyecto, setSelectedProyecto] = useState("");
  const [fecha, setFecha] = useState("");

  useEffect(() => {
    // cargar proyectos al montar el componente
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

  const handleGenerar = () => {
    if (!selectedProyecto || !fecha) {
      alert("⚠️ Debes seleccionar un proyecto y una fecha");
      return;
    }

    // aquí haces la llamada al backend
    console.log("📤 Generando reporte para:", {
      proyecto: selectedProyecto,
      fecha,
    });

  };

  return (
    <div className="reporte-container">
      <h2>📊 Reportes</h2>

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

        <div style={{ display: "flex", alignItems: "center" }}>
          <button
            className="generar-btn"
            onClick={handleGenerar}
            disabled={!selectedProyecto || !fecha}
          >
            🚀 Generar
          </button>
        </div>
      </div>

      <div className="reporte-result" id="reporteResult">
        {/* Aquí mostrarás la tabla / gráfico que devuelva el backend */}
        <small>Resultados del reporte aparecerán aquí...</small>
      </div>
    </div>
  );
};
export default ReporteMain;
