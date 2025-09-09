const formatValue = (key, value) => {
  if (value == null) return "";
  if (key.toLowerCase().includes("fecha")) {
    const d = new Date(value);
    return isNaN(d) ? value : d.toLocaleDateString("es-PE", { year: "numeric", month: "2-digit", day: "2-digit" });
  }
  if (typeof value === "number") {
    return new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(value);
  }
  return value;
};

const Table = ({ headers, data, onEdit, onDelete }) => (
  <table border="1" cellPadding="5">
    <thead>
      <tr>
        {headers.map((h, idx) => <th key={idx}>{h}</th>)}
        {(onEdit || onDelete) && <th>Acciones</th>}
      </tr>
    </thead>
    <tbody>
      {data.length === 0 ? (
        <tr><td colSpan={headers.length + 1}>No hay datos</td></tr>
      ) : (
        data.map(row => (
          <tr key={row.id}>
            {headers.map((h, idx) => <td key={idx}>{formatValue(h, row[h])}</td>)}
            {(onEdit || onDelete) && (
              <td>
                {onEdit && <button onClick={() => onEdit(row.id)}>Editar</button>}
                {onDelete && <button onClick={() => onDelete(row.id)}>Eliminar</button>}
              </td>
            )}
          </tr>
        ))
      )}
    </tbody>
  </table>
);

export default Table;
