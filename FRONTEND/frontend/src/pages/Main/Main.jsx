// import { useEffect, useState } from "react";
// import { jwtDecode } from "jwt-decode";
// import { getProyectos } from "../../services/proyectosService";
// import { PieChart, Pie, Cell, Tooltip, Legend, ResponsiveContainer } from "recharts";
// import "./Main.css";

// const Main = () => {
//   const [nombre, setNombre] = useState("Usuario");
//   const [estadistica, setEstadistica] = useState({ total: 0, act: 0, fin: 0 });

//   useEffect(() => {
//     const token = localStorage.getItem("token");
//     if (token) {
//       try {
//         const decoded = jwtDecode(token);
//         setNombre(decoded.nombre || "Usuario");
//       } catch (error) {
//         console.error("Error al decodificar token", error);
//         setNombre("Usuario");
//       }
//     }
//   }, []);

//   useEffect(() => {
//     const loadProyectos = async () => {
//       try {
//         const res = await getProyectos();
//         const data = res.data;

//         const total = data.length;
//         const act = data.filter((p) => p.codigoEstado === "ACT").length;
//         const fin = data.filter((p) => p.codigoEstado === "FIN").length;

//         setEstadistica({ total, act, fin });
//       } catch (err) {
//         console.error("❌ Error al cargar proyectos", err);
//       }
//     };

//     loadProyectos();
//   }, []);

//   const pieData = [
//     { name: "Activos (ACT)", value: estadistica.act },
//     { name: "Finalizados (FIN)", value: estadistica.fin },
//   ];

//   const COLORS = ["#4CAF50", "#FF5733"];

//   return (
//     <div className="main-page">
//       <h2 id="welcomeMsg">Bienvenido, {nombre} 👋</h2>

//       <div className="dashboard">
//         <div className="card total-card">
//           <h3>Total de Proyectos</h3>
//           <p className="total-number">{estadistica.total}</p>
//         </div>

//         <div className="card chart-card">
//           <h3>Proyectos por Estado</h3>
//           <ResponsiveContainer width="100%" height={250}>
//             <PieChart>
//               <Pie
//                 data={pieData}
//                 cx="50%"
//                 cy="50%"
//                 outerRadius={80}
//                 dataKey="value"
//                 label
//               >
//                 {pieData.map((entry, index) => (
//                   <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
//                 ))}
//               </Pie>
//               <Tooltip />
//               <Legend />
//             </PieChart>
//           </ResponsiveContainer>
//         </div>
//       </div>
//     </div>
//   );
// };

// export default Main;
import { useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";
import { getProyectos } from "../../services/proyectosService";
import { getUsuarios } from "../../services/usuariosService";
import { getIncidencias } from "../../services/incidenciaService";
import {
  PieChart,
  Pie,
  Cell,
  Tooltip,
  Legend,
  ResponsiveContainer,
  BarChart,
  XAxis,
  YAxis,
  CartesianGrid,
  Bar,
} from "recharts";
import "./Main.css";

const Main = () => {
  const [nombre, setNombre] = useState("Usuario");
  const [estadistica, setEstadistica] = useState({ total: 0, act: 0, fin: 0 });
  const [usuariosCount, setUsuariosCount] = useState(0);
  const [incidenciasEstado, setIncidenciasEstado] = useState([]);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      try {
        const decoded = jwtDecode(token);
        setNombre(decoded.nombre || "Usuario");
      } catch (error) {
        console.error("Error al decodificar token", error);
        setNombre("Usuario");
      }
    }
  }, []);

  useEffect(() => {
    const loadData = async () => {
      try {
        // 🔹 Proyectos
        const res = await getProyectos();
        console.log("👉 Proyectos:", res.data);
        const data = Array.isArray(res.data) ? res.data : res.data?.data || [];
        const total = data.length;
        const act = data.filter((p) => p.codigoEstado === "ACT").length;
        const fin = data.filter((p) => p.codigoEstado === "FIN").length;
        setEstadistica({ total, act, fin });

        // 🔹 Usuarios
        const resUsers = await getUsuarios();
        console.log("👉 Usuarios:", resUsers.data);
        const dataUsers = Array.isArray(resUsers.data)
          ? resUsers.data
          : resUsers.data?.data || [];
        setUsuariosCount(dataUsers.length);

        // 🔹 Incidencias
        const resInc = await getIncidencias();
        console.log("👉 Incidencias:", resInc.data);
        const dataInc = Array.isArray(resInc.data)
          ? resInc.data
          : resInc.data?.data || [];

        const estados = ["ABIERTA", "EN_PROCESO", "CERRADA"];
        const incidenciasAgrupadas = estados.map((estado) => ({
          name: estado,
          value: dataInc.filter((i) => i.estado === estado).length,
        }));
        setIncidenciasEstado(incidenciasAgrupadas);
      } catch (err) {
        console.error("❌ Error al cargar datos", err);
      }
    };

    loadData();
  }, []);

  const pieData = [
    { name: "Activos (ACT)", value: estadistica.act },
    { name: "Finalizados (FIN)", value: estadistica.fin },
  ];

  const COLORS = ["#4CAF50", "#FF5733", "#3498DB", "#F1C40F"];

  return (
    <div className="main-page">
      <h2 id="welcomeMsg">Bienvenido, {nombre} 👋</h2>

      <div className="dashboard">
        {/* 🔹 Total de Proyectos */}
        <div className="card total-card">
          <h3>Total de Proyectos</h3>
          <p className="total-number">{estadistica.total}</p>
        </div>

        {/* 🔹 Proyectos por Estado */}
        <div className="card chart-card">
          <h3>Proyectos por Estado</h3>
          <ResponsiveContainer width="100%" height={250}>
            <PieChart>
              <Pie
                data={pieData}
                cx="50%"
                cy="50%"
                outerRadius={80}
                dataKey="value"
                label
              >
                {pieData.map((entry, index) => (
                  <Cell
                    key={`cell-${index}`}
                    fill={COLORS[index % COLORS.length]}
                  />
                ))}
              </Pie>
              <Tooltip />
              <Legend />
            </PieChart>
          </ResponsiveContainer>
        </div>

        {/* 🔹 Total de Usuarios */}
        <div className="card total-card">
          <h3>Total de Usuarios</h3>
          <p className="total-number">{usuariosCount}</p>
        </div>

        {/* 🔹 Incidencias por Estado */}
        <div className="card chart-card">
          <h3>Incidencias por Estado</h3>
          <ResponsiveContainer width="100%" height={250}>
            <BarChart data={incidenciasEstado}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="name" />
              <YAxis allowDecimals={false} />
              <Tooltip />
              <Legend />
              <Bar dataKey="value" fill="#3498DB" />
            </BarChart>
          </ResponsiveContainer>
        </div>
      </div>
    </div>
  );
};

export default Main;
