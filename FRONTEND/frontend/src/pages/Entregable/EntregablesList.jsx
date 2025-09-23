import React, { useEffect, useState, useCallback } from "react";
import { useParams, useNavigate } from "react-router-dom";
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
import CurvaForm from "../../components/CurvaForm/CurvaForm";
import "./Entregable.css";

const EntregablesList = () => {
  const { id, contratoId } = useParams();
  const navigate = useNavigate();

  const [entregables, setEntregables] = useState([]);
  const [curva, setCurva] = useState(null);
  const [editingData, setEditingData] = useState({});
  const [showModal, setShowModal] = useState(false);
  const [editingId, setEditingId] = useState(null);

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

  // --------- Helpers para porcentajes y parsing seguros ----------
  const displayPct = (value) => {
    // Recibe valor que puede ser decimal (0-1) o porcentaje (0-100)
    if (value === null || value === undefined || value === "") return "";
    const n = Number(value);
    if (Number.isNaN(n)) return "";
    // Si viene > 1 asumimos ya está en % (ej 50 ó 100), si viene <=1 asumimos decimal y multiplicamos
    return n > 1 ? n : n * 100;
  };

  const formatPctForTable = (value) => {
    if (value === null || value === undefined || value === "") return "";
    const n = Number(value);
    if (Number.isNaN(n)) return "";
    return n > 1 ? `${n.toFixed(2)}%` : `${(n * 100).toFixed(2)}%`;
  };

  const normalizePctBeforeSave = (inputValue) => {
    // inputValue proviene del form: esperamos que usuario use 0-100
    if (inputValue === null || inputValue === undefined || inputValue === "") return null;
    const n = Number(String(inputValue).replace(",", "."));
    if (Number.isNaN(n)) return null;
    // Si usuario ingresó >1 lo interpretamos como porcentaje y lo convertimos a decimal
    if (n > 1) return n / 100;
    // Si ingresó 0<=n<=1 lo tomamos como decimal
    return n;
  };

  const parseIntOrNull = (v) => {
    if (v === null || v === undefined || v === "") return null;
    const n = parseInt(v, 10);
    return Number.isNaN(n) ? null : n;
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
      // Cargo detalle desde API
      try {
        const res = await getEntregableById(entregable.id);
        const data = res.data;
        setEditingId(entregable.id);
        setFormEntregable({
          codigo: data.codigo ?? "",
          // mostramos en % (0-100). displayPct maneja si backend usa 0-1 o 0-100
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
        // reemplazamos los campos con versiones parseadas/normalizadas
        pctContrato: pctNorm,
        duracionPlanDias: parseIntOrNull(formEntregable.duracionPlanDias),
        duracionRealDias: parseIntOrNull(formEntregable.duracionRealDias),
        contratoId: parseInt(contratoId, 10),
      };

      // limpia keys vacías o nulas si quieres (opcional)
      // Object.keys(payload).forEach(k => payload[k] === null && delete payload[k]);

      if (editingId) {
        await updateEntregable(editingId, payload);
      } else {
        await createEntregable(payload);
      }

      setShowModal(false);
      cargarEntregables();
    } catch (err) {
      console.error("❌ Error al guardar entregable", err);
      alert("Error al guardar entregable. Revisa la consola.");
    }
  };

  const handleDelete = async (entregableId) => {
    if (window.confirm("¿Eliminar este entregable?")) {
      try {
        await deleteEntregable(entregableId);
        cargarEntregables();
      } catch (err) {
        console.error("❌ Error al eliminar entregable", err);
      }
    }
  };

  const handleVerCurva = async (entregableId) => {
    try {
      const res = await getCurvasByEntregable(entregableId);
      setCurva(res.data);

      const editState = {};
      res.data.forEach((c) => {
        editState[c.id] = c.detalles.map((d) => ({
          fecha: d.fecha,
          valor: d.valor,
          valorAcumulado: d.valorAcumulado,
        }));
      });
      setEditingData(editState);
    } catch (err) {
      console.error("❌ Error al cargar curva", err);
    }
  };

  const handleSaveCurva = async (curvaId) => {
    try {
      await updateCurva(curvaId, editingData[curvaId]);
      alert("✅ Curva actualizada correctamente");
      const res = await getCurvasByEntregable(curva[0].origenId);
      setCurva(res.data);
    } catch (err) {
      console.error("❌ Error al actualizar curva", err);
      alert("❌ Error al actualizar curva");
    }
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

      {curva && (
        <div className="entregables-curva-card">
          <h3>📈 Curva del Entregable</h3>
          <CurvaChart curvas={curva} />
          <div className="entregables-curvas-wrapper">
            {curva.map((c) => (
              <CurvaForm
                key={c.id}
                curva={c}
                editingData={editingData}
                setEditingData={setEditingData}
                onSave={handleSaveCurva}
              />
            ))}
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

      {/* 🔹 Modal */}
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
    </div>
  );
};

export default EntregablesList;
