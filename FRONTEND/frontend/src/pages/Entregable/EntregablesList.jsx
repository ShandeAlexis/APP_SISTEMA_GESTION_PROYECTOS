import React, { useEffect, useState, useCallback } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import {
  getEntregablesByContrato,
  createEntregable,
  updateEntregable,
  deleteEntregable,
  getEntregableById,
} from "../../services/entregablesService";
import {
  getCurvasByEntregable,
  updateCurva,
} from "../../services/curvasService";
import CurvaChart from "../../components/CurvaChart/CurvaChart";
import "./Entregable.css";

const EntregablesList = () => {
  const { id, contratoId } = useParams();
  const navigate = useNavigate();

  const [entregables, setEntregables] = useState([]);
  const [curva, setCurva] = useState(null);
  const [editingData, setEditingData] = useState({});
  const [showModal, setShowModal] = useState(false);
  const [editingId, setEditingId] = useState(null);
  const [showCurvaModal, setShowCurvaModal] = useState(false);

  const [formEntregable, setFormEntregable] = useState({
    codigo: "",
    pctContrato: "",
    fechaInicialPlan: "",
    duracionPlanDias: "",
    fechaInicialReal: "",
    duracionRealDias: "",
    tipoEntregableCodigo: "FIS",
    tipoProrrateoCodigo: "LIN",
    edtchCodigo: "ING",
  });

  // --------- Helpers ----------
  const displayPct = (value) => {
    if (value === null || value === undefined || value === "") return "";
    const n = Number(value);
    if (Number.isNaN(n)) return "";
    return n > 1 ? n : n * 100;
  };

  const formatPctForTable = (value) => {
    if (value === null || value === undefined || value === "") return "";
    const n = Number(value);
    if (Number.isNaN(n)) return "";
    return n > 1 ? `${n.toFixed(2)}%` : `${(n * 100).toFixed(2)}%`;
  };

  const normalizePctBeforeSave = (inputValue) => {
    if (inputValue === null || inputValue === undefined || inputValue === "")
      return null;
    const n = Number(String(inputValue).replace(",", "."));
    if (Number.isNaN(n)) return null;
    if (n > 1) return n / 100;
    return n;
  };

  const parseIntOrNull = (v) => {
    if (v === null || v === undefined || v === "") return null;
    const n = parseInt(v, 10);
    return Number.isNaN(n) ? null : n;
  };

  const calcularAcumulado = (detalles) => {
    let acumulado = 0;
    return detalles.map((d) => {
      acumulado += Number(d.valor) || 0;
      return { ...d, valorAcumulado: acumulado };
    });
  };

  // ---------------------------------------------------------------

  const cargarEntregables = useCallback(async () => {
    try {
      const res = await getEntregablesByContrato(contratoId);
      setEntregables(res.data);
    } catch (err) {
      console.error("❌ Error al cargar entregables", err);
    }
  }, [contratoId]);

  useEffect(() => {
    cargarEntregables();
  }, [cargarEntregables]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormEntregable((prev) => ({ ...prev, [name]: value }));
  };

  const handleOpenModal = async (entregable = null) => {
    if (entregable) {
      try {
        const res = await getEntregableById(entregable.id);
        const data = res.data;
        setEditingId(entregable.id);
        setFormEntregable({
          codigo: data.codigo ?? "",
          pctContrato: displayPct(data.pctContrato),
          fechaInicialPlan: data.fechaInicialPlan
            ? data.fechaInicialPlan.substring(0, 10)
            : "",
          duracionPlanDias: data.duracionPlanDias ?? "",
          fechaInicialReal: data.fechaInicialReal
            ? data.fechaInicialReal.substring(0, 10)
            : "",
          duracionRealDias: data.duracionRealDias ?? "",
          tipoEntregableCodigo: data.tipoEntregableCodigo ?? "FIS",
          tipoProrrateoCodigo: data.tipoProrrateoCodigo ?? "LIN",
          edtchCodigo: data.edtchCodigo ?? "ING",
        });
        setShowModal(true);
      } catch (err) {
        console.error("❌ Error al obtener entregable", err);
      }
    } else {
      setEditingId(null);
      setFormEntregable({
        codigo: "",
        pctContrato: "",
        fechaInicialPlan: "",
        duracionPlanDias: "",
        fechaInicialReal: "",
        duracionRealDias: "",
        tipoEntregableCodigo: "FIS",
        tipoProrrateoCodigo: "LIN",
        edtchCodigo: "ING",
      });
      setShowModal(true);
    }
  };

  const handleSave = async (e) => {
    e.preventDefault();

    try {
      const pctNorm = normalizePctBeforeSave(formEntregable.pctContrato);

      const payload = {
        ...formEntregable,
        pctContrato: pctNorm,
        duracionPlanDias: parseIntOrNull(formEntregable.duracionPlanDias),
        duracionRealDias: parseIntOrNull(formEntregable.duracionRealDias),
        contratoId: parseInt(contratoId, 10),
      };

      if (editingId) {
        await updateEntregable(editingId, payload);
      } else {
        await createEntregable(payload);
      }

      setShowModal(false);
      cargarEntregables();
    } catch (err) {
      console.error("❌ Error al guardar entregable", err);
      Swal.fire("Error", "❌ Error al guardar entregable", "error");
    }
  };

  const handleDelete = async (entregableId) => {
    const result = await Swal.fire({
      title: "¿Eliminar este entregable?",
      text: "Esta acción no se puede deshacer",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Sí, eliminar",
      cancelButtonText: "Cancelar",
      confirmButtonColor: "#d33",
      cancelButtonColor: "#3085d6",
    });

    if (result.isConfirmed) {
      try {
        await deleteEntregable(entregableId);
        cargarEntregables();
        Swal.fire("Eliminado", "✅ El entregable fue eliminado", "success");
      } catch (err) {
        console.error("❌ Error al eliminar entregable", err);
        Swal.fire("Error", "❌ No se pudo eliminar el entregable", "error");
      }
    }
  };

  const handleVerCurva = async (entregableId) => {
    try {
      const res = await getCurvasByEntregable(entregableId);
      const processed = res.data.map((c) => ({
        ...c,
        detalles: calcularAcumulado(c.detalles),
      }));
      setCurva(processed);

      const editState = {};
      processed.forEach((c) => {
        editState[c.id] = c.detalles;
      });
      setEditingData(editState);
      setShowCurvaModal(true);
    } catch (err) {
      console.error("❌ Error al cargar curva", err);
    }
  };

const handleCurvaChange = (curvaId, index, field, value) => {
  setEditingData((prev) => {
    const updated = [...prev[curvaId]];
    const newValue = Number(value);

    // Validamos que sea número válido
    if (isNaN(newValue) || newValue < 0 || newValue > 100) {
      Swal.fire({
        icon: "error",
        title: "Validación",
        text: "❌ El valor debe estar entre 0 y 100.",
        didOpen: (popup) => {
          popup.style.zIndex = 20000;
        },
      });
      return prev; // no aplicamos el cambio
    }

    updated[index] = { ...updated[index], [field]: newValue };
    const recalculada = calcularAcumulado(updated);

    // Validar acumulado final
    if (recalculada[recalculada.length - 1].valorAcumulado > 100) {
      Swal.fire({
        icon: "error",
        title: "Validación",
        text: "❌ El acumulado no puede superar el 100% en el último mes.",
        didOpen: (popup) => {
          popup.style.zIndex = 20000;
        },
      });
      return prev; // no aplicamos el cambio
    }

    return { ...prev, [curvaId]: recalculada };
  });
};


  const handleSaveCurva = async (curvaId) => {
    try {
      await updateCurva(curvaId, editingData[curvaId]);
      Swal.fire("Éxito", "✅ Curva actualizada correctamente", "success");
      const res = await getCurvasByEntregable(curva[0].origenId);
      setCurva(
        res.data.map((c) => ({ ...c, detalles: calcularAcumulado(c.detalles) }))
      );
      setShowCurvaModal(false);
    } catch (err) {
      console.error("❌ Error al actualizar curva", err);
      Swal.fire("Error", "❌ Error al actualizar curva", "error");
    }
  };

  const getCurvaLabel = (codigo) => {
    if (codigo === "CRV_PLAN_VIG") return "Plan";
    if (codigo === "CRV_REAL_VIG") return "Real";
    return "Curva";
  };

  return (
    <div className="entregables-container">
      <div className="entregables-header">
        <h2>
          📦 Entregables del Contrato{" "}
          {entregables.length > 0 ? entregables[0].contratoCodigo : contratoId}
        </h2>
        <button
          className="entregables-nuevo-btn"
          onClick={() => handleOpenModal()}
        >
          + Nuevo Entregable
        </button>
      </div>

      <div className="entregables-table-wrapper">
        <table className="entregables-table">
          <thead>
            <tr>
              <th>Código</th>
              <th>% Contrato</th>
              <th>Fecha Plan</th>
              <th>Duración Plan (días)</th>
              <th>Tipo</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {entregables.map((e) => (
              <tr key={e.id}>
                <td>{e.codigo}</td>
                <td>{formatPctForTable(e.pctContrato)}</td>
                <td>{e.fechaInicialPlan?.substring(0, 10)}</td>
                <td>{e.duracionPlanDias}</td>
                <td>{e.tipoEntregableCodigo}</td>
                <td>
                  <div className="entregables-acciones-btns">
                    <button
                      className="entregables-edit-btn"
                      onClick={() => handleOpenModal(e)}
                    >
                      ✏️
                    </button>
                    <button
                      className="entregables-delete-btn"
                      onClick={() => handleDelete(e.id)}
                    >
                      🗑️
                    </button>
                    <button
                      className="entregables-view-btn"
                      onClick={() => handleVerCurva(e.id)}
                    >
                      📈
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Modal Entregable */}
      {showModal && (
        <div className="entregables-modal-overlay">
          <div className="entregables-modal">
            <h3 className="entregables-modal-title">
              {editingId ? "✏️ Editar Entregable" : "➕ Nuevo Entregable"}
            </h3>
            <form onSubmit={handleSave} className="entregables-form-grid">
              <label>
                Código:
                <input
                  type="text"
                  name="codigo"
                  value={formEntregable.codigo}
                  onChange={handleInputChange}
                  required
                />
              </label>
              <label>
                % Contrato:
                <input
                  type="number"
                  step="0.01"
                  min="0"
                  max="100"
                  name="pctContrato"
                  value={formEntregable.pctContrato}
                  onChange={handleInputChange}
                  required
                />
              </label>
              <label>
                Fecha Inicial Plan:
                <input
                  type="date"
                  name="fechaInicialPlan"
                  value={formEntregable.fechaInicialPlan}
                  onChange={handleInputChange}
                  required
                />
              </label>
              <label>
                Duración Plan (días):
                <input
                  type="number"
                  name="duracionPlanDias"
                  value={formEntregable.duracionPlanDias}
                  onChange={handleInputChange}
                  required
                />
              </label>
              <label>
                Fecha Inicial Real:
                <input
                  type="date"
                  name="fechaInicialReal"
                  value={formEntregable.fechaInicialReal}
                  onChange={handleInputChange}
                />
              </label>
              <label>
                Duración Real (días):
                <input
                  type="number"
                  name="duracionRealDias"
                  value={formEntregable.duracionRealDias}
                  onChange={handleInputChange}
                />
              </label>
              <label>
                Tipo Entregable:
                <select
                  name="tipoEntregableCodigo"
                  value={formEntregable.tipoEntregableCodigo}
                  onChange={handleInputChange}
                >
                  <option value="FIS">Físico</option>
                  <option value="ECO">Económico</option>
                </select>
              </label>
              <label>
                Tipo Prorrateo:
                <select
                  name="tipoProrrateoCodigo"
                  value={formEntregable.tipoProrrateoCodigo}
                  onChange={handleInputChange}
                >
                  <option value="LIN">Lineal</option>
                  <option value="PUL">Pulso</option>
                </select>
              </label>
              <label>
                EDT:
                <select
                  name="edtchCodigo"
                  value={formEntregable.edtchCodigo}
                  onChange={handleInputChange}
                >
                  <option value="GES">GES</option>
                  <option value="ING">ING</option>
                  <option value="LTT">LTT</option>
                  <option value="PERM">PERM</option>
                  <option value="PES">PES</option>
                  <option value="SET">SET</option>
                </select>
              </label>
              <div className="entregables-form-buttons">
                <button type="submit" className="entregables-save-btn">
                  💾 Guardar
                </button>
                <button
                  type="button"
                  className="entregables-cancel-btn"
                  onClick={() => setShowModal(false)}
                >
                  ❌ Cancelar
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Modal Curvas */}
      {showCurvaModal && curva && (
        <div className="entregables-modal-overlay">
          <div className="entregables-modal entregables-curva-modal">
            <h3 className="entregables-modal-title">
              📈 Curvas del Entregable
            </h3>
            <CurvaChart curvas={curva} />
            <div className="entregables-curva-table">
              {curva.map((c) => (
                <div key={c.id} className="curva-section">
                  <h4>{getCurvaLabel(c.tipoCurvaCodigo)}</h4>
                  <div className="entregables-curva-table-horizontal">
                    <table>
                      <thead>
                        <tr>
                          <th></th>
                          {editingData[c.id].map((d, i) => (
                            <th key={i}>{d.fecha.substring(0, 10)}</th>
                          ))}
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td>Mensual (%)</td>
                          {editingData[c.id].map((d, i) => (
                            <td key={i}>
                              <input
                                type="number"
                                step="0.01"
                                min="0"
                                max="100"
                                value={d.valor}
                                onChange={(e) =>
                                  handleCurvaChange(
                                    c.id,
                                    i,
                                    "valor",
                                    e.target.value
                                  )
                                }
                              />
                            </td>
                          ))}
                        </tr>
                        <tr>
                          <td>Acumulado (%)</td>
                          {editingData[c.id].map((d, i) => (
                            <td key={i}>{d.valorAcumulado.toFixed(2)}%</td>
                          ))}
                        </tr>
                      </tbody>
                    </table>
                  </div>

                  <div className="entregables-form-buttons">
                    <button
                      className="entregables-save-btn"
                      onClick={() => handleSaveCurva(c.id)}
                    >
                      💾 Guardar Curva
                    </button>
                    <button
                      className="entregables-cancel-btn"
                      onClick={() => setShowCurvaModal(false)}
                    >
                      ❌ Cerrar
                    </button>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      )}

      <div className="entregables-footer-btns">
        <button
          className="entregables-back-btn"
          onClick={() => navigate(`/proyectos/${id}/contratos`)}
        >
          ⬅️ Volver a Contratos
        </button>
      </div>
    </div>
  );
};

export default EntregablesList;
