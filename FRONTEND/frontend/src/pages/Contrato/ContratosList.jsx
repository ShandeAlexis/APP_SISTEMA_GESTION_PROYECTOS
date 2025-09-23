import { useEffect, useState, useCallback } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {
  getContratosByProyecto,
  createContrato,
  updateContrato,
  deleteContrato,
} from "../../services/contratosService";
import "./Contratos.css";

const Contratos = () => {
  const { id: proyectoId } = useParams();
  const navigate = useNavigate();

  const [contratos, setContratos] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [editando, setEditando] = useState(null);

  const [formContrato, setFormContrato] = useState({
    codigo: "",
    codigoContrato: "",
    capex: "",
    costo: "",
    estimado: "",
    contratista: "",
    alcance: "",
    estadoCodigo: "VIG",
    objetivo: "",
    fechaInicial: "",
    fechaFinal: "",
  });

  const cargarContratos = useCallback(async () => {
    try {
      const res = await getContratosByProyecto(proyectoId);
      setContratos(res.data);
    } catch (err) {
      console.error("Error cargando contratos", err);
    }
  }, [proyectoId]);

  useEffect(() => {
    cargarContratos();
  }, [cargarContratos]);

  const handleInputChange = (e) => {
    setFormContrato({ ...formContrato, [e.target.name]: e.target.value });
  };

  const handleGuardar = async (e) => {
    e.preventDefault();
    try {
      if (editando) {
        await updateContrato(editando.id, formContrato);
      } else {
        await createContrato({
          ...formContrato,
          proyectoId: parseInt(proyectoId),
        });
      }
      setShowModal(false);
      setEditando(null);
      resetForm();
      cargarContratos();
    } catch (err) {
      console.error("Error guardando contrato", err);
    }
  };

  const handleEliminar = async (id) => {
    if (!window.confirm("¿Seguro que deseas eliminar este contrato?")) return;
    try {
      await deleteContrato(id);
      cargarContratos();
    } catch (err) {
      console.error("Error eliminando contrato", err);
    }
  };

  const abrirModalEditar = (contrato) => {
    setEditando(contrato);
    setFormContrato({
      codigo: contrato.codigo,
      codigoContrato: contrato.codigoContrato,
      capex: contrato.capex,
      costo: contrato.costo,
      estimado: contrato.estimado,
      contratista: contrato.contratista,
      alcance: contrato.alcance,
      estadoCodigo: contrato.estadoCodigo,
      objetivo: contrato.objetivo,
      fechaInicial: contrato.fechaInicial?.slice(0, 10) || "",
      fechaFinal: contrato.fechaFinal?.slice(0, 10) || "",
    });
    setShowModal(true);
  };

  const abrirModalNuevo = () => {
    setEditando(null);
    resetForm();
    setShowModal(true);
  };

  const resetForm = () => {
    setFormContrato({
      codigo: "",
      codigoContrato: "",
      capex: "",
      costo: "",
      estimado: "",
      contratista: "",
      alcance: "",
      estadoCodigo: "VIG",
      objetivo: "",
      fechaInicial: "",
      fechaFinal: "",
    });
  };

  return (
    <div className="contratos-page">
      <div className="header">
        <h1>
          📑 Contratos del Proyecto{" "}
          {contratos.length > 0 ? contratos[0].proyectoCodigo : "x"}
        </h1>
        <div className="acciones-header">
          <button className="btn-primary" onClick={abrirModalNuevo}>
            ➕ Nuevo Contrato
          </button>
          <button
            className="btn-secondary"
            onClick={() => navigate("/proyectos")}
          >
            ⬅️ Volver
          </button>
        </div>
      </div>

      <div className="table-wrapper">
        <table>
          <thead>
            <tr>
              <th>Código</th>
              <th>Código Contrato</th>
              <th>Contratista</th>
              <th>Capex</th>
              <th>Costo</th>
              <th>Estimado</th>
              <th>Alcance</th>
              <th>Estado</th>
              <th>Fecha Inicial</th>
              <th>Fecha Final</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {contratos.map((c) => (
              <tr key={c.id}>
                <td>{c.codigo}</td>
                <td>{c.codigoContrato}</td>
                <td>{c.contratista}</td>
                <td>{c.capex}</td>
                <td>{c.costo}</td>
                <td>{c.estimado}</td>
                <td>{c.alcance}</td>
                <td>{c.estadoCodigo}</td>
                <td>{new Date(c.fechaInicial).toLocaleDateString()}</td>
                <td>{new Date(c.fechaFinal).toLocaleDateString()}</td>
                <td>
                  <div className="acciones">
                    <button
                      className="btn-info"
                      onClick={() =>
                        navigate(
                          `/proyectos/${proyectoId}/contratos/${c.id}/entregables`
                        )
                      }
                    >
                      👁️ Ver
                    </button>
                    <button
                      className="btn-warning"
                      onClick={() => abrirModalEditar(c)}
                    >
                      ✏️ Editar
                    </button>
                    <button
                      className="btn-danger"
                      onClick={() => handleEliminar(c.id)}
                    >
                      🗑️ Eliminar
                    </button>
                  </div>
                </td>
              </tr>
            ))}
            {contratos.length === 0 && (
              <tr>
                <td colSpan="11">No hay contratos registrados</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      {showModal && (
        <div className="modal">
          <div className="modal-content">
            <h2>{editando ? "Editar Contrato" : "Nuevo Contrato"}</h2>
            <form onSubmit={handleGuardar}>
              <div className="form-grid">
                <label>
                  Código:
                  <input
                    type="text"
                    name="codigo"
                    value={formContrato.codigo}
                    onChange={handleInputChange}
                    required
                  />
                </label>
                <label>
                  Código Contrato:
                  <input
                    type="text"
                    name="codigoContrato"
                    value={formContrato.codigoContrato}
                    onChange={handleInputChange}
                  />
                </label>
                <label>
                  Contratista:
                  <input
                    type="text"
                    name="contratista"
                    value={formContrato.contratista}
                    onChange={handleInputChange}
                    required
                  />
                </label>
                <label className="full-width">
                  Alcance:
                  <textarea
                    name="alcance"
                    value={formContrato.alcance}
                    onChange={handleInputChange}
                    required
                  />
                </label>
                <label>
                  Objetivo:
                  <input
                    type="text"
                    name="objetivo"
                    value={formContrato.objetivo}
                    onChange={handleInputChange}
                  />
                </label>
                <label>
                  Capex:
                  <input
                    type="number"
                    name="capex"
                    value={formContrato.capex}
                    onChange={handleInputChange}
                  />
                </label>
                <label>
                  Costo:
                  <input
                    type="number"
                    name="costo"
                    value={formContrato.costo}
                    onChange={handleInputChange}
                  />
                </label>
                <label>
                  Estimado:
                  <input
                    type="number"
                    name="estimado"
                    value={formContrato.estimado}
                    onChange={handleInputChange}
                  />
                </label>
                <label>
                  Estado:
                  <select
                    name="estadoCodigo"
                    value={formContrato.estadoCodigo}
                    onChange={handleInputChange}
                    required
                  >
                    <option value="VIG">Vigente</option>
                    <option value="CER">Cerrado</option>
                  </select>
                </label>

                <label>
                  Fecha Inicial:
                  <input
                    type="date"
                    name="fechaInicial"
                    value={formContrato.fechaInicial}
                    onChange={handleInputChange}
                    required
                  />
                </label>
                <label>
                  Fecha Final:
                  <input
                    type="date"
                    name="fechaFinal"
                    value={formContrato.fechaFinal}
                    onChange={handleInputChange}
                    required
                  />
                </label>
              </div>
              <div className="modal-actions">
                <button type="submit" className="btn-success">
                  Guardar
                </button>
                <button
                  type="button"
                  className="btn-secondary"
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

export default Contratos;
