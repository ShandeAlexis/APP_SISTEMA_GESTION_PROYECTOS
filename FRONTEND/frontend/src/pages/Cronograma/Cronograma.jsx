import React, { useEffect, useState } from "react";
import { getProyectos } from "../../services/proyectosService";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Cell,
  ResponsiveContainer,
} from "recharts";
import "./Cronograma.css";

const Cronograma = () => {
  const [proyectos, setProyectos] = useState([]);
  const [minDate, setMinDate] = useState(null);
  const [maxDate, setMaxDate] = useState(null);

  // filtros de usuario
  const [filtroInicio, setFiltroInicio] = useState("");
  const [filtroFin, setFiltroFin] = useState("");

  useEffect(() => {
    const loadProyectos = async () => {
      try {
        const res = await getProyectos();
        const data = res.data.map((p) => {
          const inicio = new Date(p.fechaInicio);
          const fin = new Date(p.fechaFin);

          return {
            id: p.id,
            nombre: p.nombre || p.codigo,
            estado: p.codigoEstado || "PEN",
            inicio,
            fin,
            start: inicio.getTime(),
            end: fin.getTime(),
            duracion: (fin.getTime() - inicio.getTime()) / (1000 * 60 * 60 * 24),
          };
        });

        const min = new Date(Math.min(...data.map((d) => d.start)));
        const max = new Date(Math.max(...data.map((d) => d.end)));

        setMinDate(min);
        setMaxDate(max);
        setProyectos(data);

        // valores iniciales para filtros
        setFiltroInicio(min.toISOString().split("T")[0]);
        setFiltroFin(max.toISOString().split("T")[0]);
      } catch (err) {
        console.error("❌ Error al cargar proyectos", err);
      }
    };
    loadProyectos();
  }, []);

  const formatMonth = (timestamp) => {
    const date = new Date(timestamp);
    return date.toLocaleDateString("es-ES", {
      month: "short",
      year: "2-digit",
    });
  };

  // rango en ms según filtro
  const inicioFiltroMs = filtroInicio ? new Date(filtroInicio).getTime() : minDate?.getTime();
  const finFiltroMs = filtroFin ? new Date(filtroFin).getTime() : maxDate?.getTime();

  const chartData = proyectos.map((p) => ({
    ...p,
    offset: p.start - inicioFiltroMs,
    duracion: p.end - p.start,
  }));

  const getColor = (estado) => {
    switch (estado) {
      case "VIG":
        return "#4caf50";
      case "FIN":
        return "#9e9e9e";
      case "ACT":
        return "#4cafef";
      case "PEN":
      default:
        return "#ff9800";
    }
  };

  return (
    <div className="cronograma-container">
      <h2>📅 Cronograma de Proyectos</h2>

      {/* filtros de rango */}
      <div className="filtros-fechas">
        <label>
          Inicio:
          <input
            type="date"
            value={filtroInicio}
            onChange={(e) => setFiltroInicio(e.target.value)}
          />
        </label>
        <label>
          Fin:
          <input
            type="date"
            value={filtroFin}
            onChange={(e) => setFiltroFin(e.target.value)}
          />
        </label>
      </div>

      <div className="cronograma-grid">
        {/* Lista */}
        <div className="proyectos-lista">
          <table>
            <thead>
              <tr>
                <th>Proyecto</th>
                <th>Inicio</th>
                <th>Fin</th>
                <th>Duración (días)</th>
                <th>Estado</th>
              </tr>
            </thead>
            <tbody>
              {proyectos.map((p, i) => (
                <tr key={p.id} className={i % 2 === 0 ? "even" : "odd"}>
                  <td>{p.nombre}</td>
                  <td>{p.inicio.toLocaleDateString()}</td>
                  <td>{p.fin.toLocaleDateString()}</td>
                  <td>{p.duracion}</td>
                  <td>
                    <span
                      className="estado-badge"
                      style={{ background: getColor(p.estado) }}
                    >
                      {p.estado}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Gantt con rango de fechas */}
        <div className="grafico-gantt">
          <ResponsiveContainer width="100%" height={500}>
            <BarChart
              data={chartData}
              layout="vertical"
              margin={{ top: 20, right: 30, left: 5, bottom: 20 }}
            >
              <CartesianGrid strokeDasharray="2 3" stroke="#555" />
              <XAxis
                type="number"
                domain={[0, finFiltroMs - inicioFiltroMs]}
                tickFormatter={(val) => formatMonth(inicioFiltroMs + val)}
                stroke="#ddd"
              />
              <YAxis dataKey="nombre" type="category" width={120} stroke="#ddd" />
              <Tooltip
                contentStyle={{
                  background: "#2b2b2b",
                  border: "1px solid #555",
                  borderRadius: "8px",
                  color: "#fff",
                }}
                formatter={(val, name, props) => {
                  if (name === "duracion") {
                    return `${props.payload.duracion / (1000 * 60 * 60 * 24)} días`;
                  }
                  return val;
                }}
                labelFormatter={(label, payload) => {
                  if (payload && payload.length > 0) {
                    const p = payload[0].payload;
                    return `${p.nombre} (${p.inicio.toLocaleDateString()} - ${p.fin.toLocaleDateString()})`;
                  }
                  return "";
                }}
              />
              <Bar dataKey="offset" stackId="a" fill="transparent" />
              <Bar dataKey="duracion" stackId="a" radius={[10, 10, 10, 10]}>
                {chartData.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={getColor(entry.estado)} />
                ))}
              </Bar>
            </BarChart>
          </ResponsiveContainer>
        </div>
      </div>
    </div>
  );
};

export default Cronograma;
