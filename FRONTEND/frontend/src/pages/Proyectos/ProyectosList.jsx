import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  getProyectos,
  createProyecto,
  updateProyecto,
  deleteProyecto,
} from "../../services/proyectosService";
import "./Proyectos.css";

const ProyectosList = () => {
  const [proyectos, setProyectos] = useState([]);
  const [filtro, setFiltro] = useState("todos");
  const [showModal, setShowModal] = useState(false);
  const [editando, setEditando] = useState(null);

  const [formProyecto, setFormProyecto] = useState({
    codigo: "",
    descripcion: "",
    fechaInicio: "",
    fechaFin: "",
    capex: 0,
    estimado: 0,
    codigoEstado: "ACT",
    codigoEmpresa: "EMP002", // 👈 obligatorio
  });

  const navigate = useNavigate();

  useEffect(() => {
    cargarProyectos();
  }, []);

  const cargarProyectos = async () => {
    try {
      const res = await getProyectos();
      setProyectos(res.data);
    } catch (err) {
      console.error("Error cargando proyectos", err);
    }
  };

  const handleInputChange = (e) => {
    setFormProyecto({ ...formProyecto, [e.target.name]: e.target.value });
  };

  const handleGuardar = async (e) => {
    e.preventDefault();
    try {
      const payload = {
        ...formProyecto,
        fechaInicio: formProyecto.fechaInicio
          ? `${formProyecto.fechaInicio}T00:00:00`
          : null,
        fechaFin: formProyecto.fechaFin
          ? `${formProyecto.fechaFin}T00:00:00`
          : null,
        capex: parseFloat(formProyecto.capex || 0),
        estimado: parseFloat(formProyecto.estimado || 0),
      };

      if (editando) {
        await updateProyecto(editando.id, payload);
      } else {
        await createProyecto(payload);
      }

      setShowModal(false);
      setEditando(null);
      resetForm();
      cargarProyectos();
    } catch (err) {
      console.error("Error guardando proyecto", err);
    }
  };

  const handleEliminar = async (id) => {
    if (!window.confirm("¿Seguro que deseas eliminar este proyecto?")) return;
    try {
      await deleteProyecto(id);
      cargarProyectos();
    } catch (err) {
      console.error("Error eliminando proyecto", err);
    }
  };

  const abrirModalNuevo = () => {
    setEditando(null);
    resetForm();
    setShowModal(true);
  };

  const abrirModalEditar = (proyecto) => {
    setEditando(proyecto);
    setFormProyecto({
      codigo: proyecto.codigo,
      descripcion: proyecto.descripcion,
      fechaInicio: proyecto.fechaInicio
        ? proyecto.fechaInicio.substring(0, 10)
        : "",
      fechaFin: proyecto.fechaFin ? proyecto.fechaFin.substring(0, 10) : "",
      capex: proyecto.capex,
      estimado: proyecto.estimado,
      codigoEstado: proyecto.codigoEstado,
      codigoEmpresa: proyecto.codigoEmpresa || "EMP002",
    });
    setShowModal(true);
  };

  const resetForm = () => {
    setFormProyecto({
      codigo: "",
      descripcion: "",
      fechaInicio: "",
      fechaFin: "",
      capex: 0,
      estimado: 0,
      codigoEstado: "ACT",
      codigoEmpresa: "EMP002",
    });
  };

  const filteredProyectos =
    filtro === "todos"
      ? proyectos
      : proyectos.filter((p) => p.codigoEstado === filtro);

  return (
    <div className="proyectos-page">
      <div className="header">
        <h1>📋 Listado de Proyectos</h1>
        <div className="actions">
          <select value={filtro} onChange={(e) => setFiltro(e.target.value)}>
            <option value="todos">Todos</option>
            <option value="ACT">Activos</option>
            <option value="FIN">Finalizados</option>
          </select>
          <button className="btn-agregar" onClick={abrirModalNuevo}>
            ➕ Nuevo Proyecto
          </button>
        </div>
      </div>

      <table className="proyectos-table">
        <thead>
          <tr>
            <th>Código</th>
            <th>Descripción</th>
            <th>Fecha Inicio</th>
            <th>Fecha Fin</th>
            <th>Capex</th>
            <th>Estimado</th>
            <th>Estado</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          {filteredProyectos.map((p) => (
            <tr key={p.id}>
              <td>{p.codigo}</td>
              <td>{p.descripcion}</td>
              <td>{p.fechaInicio ? p.fechaInicio.substring(0, 10) : "-"}</td>
              <td>{p.fechaFin ? p.fechaFin.substring(0, 10) : "-"}</td>
              <td>{p.capex}</td>
              <td>{p.estimado}</td>
              <td>{p.codigoEstado}</td>
              <td>
                <button className="btn-editar" onClick={() => abrirModalEditar(p)}>
                  ✏️ Editar
                </button>
                <button
                  className="btn-eliminar"
                  onClick={() => handleEliminar(p.id)}
                >
                  🗑️ Eliminar
                </button>
                <button
                  className="btn-contratos"
                  onClick={() => navigate(`/proyectos/${p.id}/contratos`)}
                >
                  📑 Contratos
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
            <h2>{editando ? "Editar Proyecto" : "Nuevo Proyecto"}</h2>
            <form onSubmit={handleGuardar}>
              <label>
                Código:
                <input
                  type="text"
                  name="codigo"
                  value={formProyecto.codigo}
                  onChange={handleInputChange}
                  required
                />
              </label>
              <label>
                Descripción:
                <input
                  type="text"
                  name="descripcion"
                  value={formProyecto.descripcion}
                  onChange={handleInputChange}
                  required
                />
              </label>
              <label>
                Fecha Inicio:
                <input
                  type="date"
                  name="fechaInicio"
                  value={formProyecto.fechaInicio}
                  onChange={handleInputChange}
                  required
                />
              </label>
              <label>
                Fecha Fin:
                <input
                  type="date"
                  name="fechaFin"
                  value={formProyecto.fechaFin}
                  onChange={handleInputChange}
                />
              </label>
              <label>
                Capex:
                <input
                  type="number"
                  name="capex"
                  value={formProyecto.capex}
                  onChange={handleInputChange}
                />
              </label>
              <label>
                Estimado:
                <input
                  type="number"
                  name="estimado"
                  value={formProyecto.estimado}
                  onChange={handleInputChange}
                />
              </label>
              <label>
                Estado:
                <select
                  name="codigoEstado"
                  value={formProyecto.codigoEstado}
                  onChange={handleInputChange}
                >
                  <option value="ACT">Activo</option>
                  <option value="FIN">Finalizado</option>
                </select>
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

export default ProyectosList;
