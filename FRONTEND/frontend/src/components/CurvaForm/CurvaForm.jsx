import React from "react";
import "./CurvaForm.css";

const CurvaForm = ({ curva, editingData, setEditingData, onSave }) => {
  const handleInputChange = (index, field, value) => {
    setEditingData((prev) => {
      const updated = { ...prev };
      updated[curva.id][index][field] =
        field === "fecha" ? value : parseFloat(value);
      return updated;
    });
  };

  return (
    <div className="curva-form">
      <h4>{curva.tipoCurvaCodigo === "CRV_PLAN_VIG" ? "📊 Plan" : "📊 Real"}</h4>
      <table>
        <thead>
          <tr>
            <th></th>
            {editingData[curva.id]?.map((d, i) => (
              <th key={i}>
                {new Date(d.fecha).toLocaleDateString("es-ES", {
                  month: "short",
                  year: "numeric",
                })}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>Valor Mensual</td>
            {editingData[curva.id]?.map((d, i) => (
              <td key={i}>
                <input
                  type="number"
                  value={d.valor}
                  onChange={(e) =>
                    handleInputChange(i, "valor", e.target.value)
                  }
                />
              </td>
            ))}
          </tr>
          <tr>
            <td>Valor Acumulado</td>
            {editingData[curva.id]?.map((d, i) => (
              <td key={i}>
                <input
                  type="number"
                  value={d.valorAcumulado}
                  onChange={(e) =>
                    handleInputChange(i, "valorAcumulado", e.target.value)
                  }
                />
              </td>
            ))}
          </tr>
        </tbody>
      </table>
      <button className="save-btn" onClick={() => onSave(curva.id)}>
        💾 Guardar 
      </button>
    </div>
  );
};

export default CurvaForm;
