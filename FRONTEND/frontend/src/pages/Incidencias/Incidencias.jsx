import { useEffect, useState } from "react";
import {
  getIncidencias,
  createIncidencia,
  updateIncidencia,
  deleteIncidencia,
} from "../../services/incidenciaService";
import { getProyectos } from "../../services/proyectosService";
import "./Incidencias.css";

const Incidencias = () => {
  const [incidencias, setIncidencias] = useState([]);
  const [proyectos, setProyectos] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [editando, setEditando] = useState(null);

  const [formIncidencia, setFormIncidencia] = useState({
    proyectoId: "",
    nota: "",
    nivel: "BAJO",
    estado: "ABIERTA",
    responsable: "",
    categoria: "",
    fechaResolucion: "",
  });

  useEffect(() => {
    cargarIncidencias();
    cargarProyectos();
  }, []);

  const cargarIncidencias = async () => {
    try {
      const res = await getIncidencias();
      setIncidencias(res.data);
    } catch (err) {
      console.error("Error cargando incidencias", err);
    }
  };

  const cargarProyectos = async () => {
    try {
      const res = await getProyectos();
      setProyectos(res.data);
    } catch (err) {
      console.error("Error cargando proyectos", err);
    }
  };

  const handleInputChange = (e) => {
    setFormIncidencia({ ...formIncidencia, [e.target.name]: e.target.value });
  };

  const handleGuardar = async (e) => {
    e.preventDefault();
    try {
      // 👇 corregido: si no hay fecha, mandamos null
      const payload = {
        ...formIncidencia,
        fechaResolucion: formIncidencia.fechaResolucion || null,
      };

      if (editando) {
        await updateIncidencia(editando.id, payload);
      } else {
        await createIncidencia(payload);
      }
      setShowModal(false);
      setEditando(null);
      resetForm();
      cargarIncidencias();
    } catch (err) {
      console.error("Error guardando incidencia", err);
    }
  };

  const handleEliminar = async (id) => {
    if (!window.confirm("¿Seguro que deseas eliminar esta incidencia?")) return;
    try {
      await deleteIncidencia(id);
      cargarIncidencias();
    } catch (err) {
      console.error("Error eliminando incidencia", err);
    }
  };

  const abrirModalEditar = (incidencia) => {
    const proyecto = proyectos.find((p) => p.codigo === incidencia.codigoProyecto);

    setEditando(incidencia);
    setFormIncidencia({
      proyectoId: proyecto ? proyecto.id : "",
      nota: incidencia.nota,
      nivel: incidencia.nivel,
      estado: incidencia.estado,
      responsable: incidencia.responsable,
      categoria: incidencia.categoria,
      fechaResolucion: incidencia.fechaResolucion
        ? incidencia.fechaResolucion.slice(0, 16)
        : "", // 👈 vacío si es null
    });
    setShowModal(true);
  };

  const abrirModalNuevo = () => {
    setEditando(null);
    resetForm();
    setShowModal(true);
  };

  const resetForm = () => {
    setFormIncidencia({
      proyectoId: "",
      nota: "",
      nivel: "BAJO",
      estado: "ABIERTA",
      responsable: "",
      categoria: "",
      fechaResolucion: "",
    });
  };

  return (
    <div className="incidencias-page">
      <h1>Gestión de Incidencias</h1>

      <button className="btn-agregar" onClick={abrirModalNuevo}>
        ➕ Nueva Incidencia
      </button>

      <table className="incidencias-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Proyecto</th>
            <th>Nota</th>
            <th>Nivel</th>
            <th>Estado</th>
            <th>Responsable</th>
            <th>Categoría</th>
            <th>Fecha Registro</th>
            <th>Fecha Resolución</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          {incidencias.map((i) => (
            <tr key={i.id}>
              <td>{i.id}</td>
              <td>{i.codigoProyecto}</td>
              <td>{i.nota}</td>
              <td>{i.nivel}</td>
              <td>{i.estado}</td>
              <td>{i.responsable}</td>
              <td>{i.categoria}</td>
              <td>{new Date(i.fechaRegistro).toLocaleString()}</td>
              <td>
                {i.fechaResolucion
                  ? new Date(i.fechaResolucion).toLocaleString()
                  : ""} {/* 👈 ahora vacío si es null */}
              </td>
              <td>
                <button
                  className="btn-editar"
                  onClick={() => abrirModalEditar(i)}
                >
                  ✏️ Editar
                </button>
                <button
                  className="btn-eliminar"
                  onClick={() => handleEliminar(i.id)}
                >
                  🗑️ Eliminar
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {/* Modal */}
      {showModal && (
        <div className="modal">
          <div className="modal-content">
            <h2>{editando ? "Editar Incidencia" : "Nueva Incidencia"}</h2>
            <form onSubmit={handleGuardar}>
              <label>
                Proyecto:
                <select
                  name="proyectoId"
                  value={formIncidencia.proyectoId}
                  onChange={handleInputChange}
                  required
                >
                  <option value="">-- Seleccione un proyecto --</option>
                  {proyectos.map((p) => (
                    <option key={p.id} value={p.id}>
                      {p.codigo} - {p.nombre}
                    </option>
                  ))}
                </select>
              </label>

              <label>
                Nota:
                <textarea
                  name="nota"
                  value={formIncidencia.nota}
                  onChange={handleInputChange}
                  required
                />
              </label>

              <label>
                Nivel:
                <select
                  name="nivel"
                  value={formIncidencia.nivel}
                  onChange={handleInputChange}
                >
                  <option value="BAJO">BAJO</option>
                  <option value="MEDIO">MEDIO</option>
                  <option value="ALTO">ALTO</option>
                </select>
              </label>

              <label>
                Estado:
                <select
                  name="estado"
                  value={formIncidencia.estado}
                  onChange={handleInputChange}
                >
                  <option value="ABIERTA">ABIERTA</option>
                  <option value="EN_PROCESO">EN PROCESO</option>
                  <option value="CERRADA">CERRADA</option>
                </select>
              </label>

              <label>
                Responsable:
                <input
                  type="text"
                  name="responsable"
                  value={formIncidencia.responsable}
                  onChange={handleInputChange}
                  required
                />
              </label>

              <label>
                Categoría:
                <input
                  type="text"
                  name="categoria"
                  value={formIncidencia.categoria}
                  onChange={handleInputChange}
                  required
                />
              </label>

              <label>
                Fecha Resolución:
                <input
                  type="datetime-local"
                  name="fechaResolucion"
                  value={formIncidencia.fechaResolucion}
                  onChange={handleInputChange}
                />
              </label>

              <div className="modal-actions">
                <button type="submit" className="btn-guardar">
                  Guardar
                </button>
                <button
                  type="button"
                  className="btn-cancelar"
                  onClick={() => setShowModal(false)}
                >
                  Cancelar
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Incidencias;
