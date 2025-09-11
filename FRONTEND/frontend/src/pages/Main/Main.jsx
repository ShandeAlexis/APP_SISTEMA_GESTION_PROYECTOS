import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import {jwtDecode} from "jwt-decode"; // import correcto
import "./Main.css";

const Main = () => {
  const navigate = useNavigate();
  const [nombre, setNombre] = useState("Usuario");

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

  return (
    <div className="main-page">
      <h2 id="welcomeMsg">Bienvenido, {nombre} 👋</h2>

      <div className="card-container">
        <div className="card" onClick={() => navigate("/proyectos")}>
          📊 Proyectos
        </div>
        <div className="card">⚙️ Configuración</div>
        <div className="card">📂 Mis Archivos</div>
        <div className="card">💬 Mensajes</div>
        <div className="card">📊 Reportes</div>
        <div className="card">📈 Estadísticas</div>
      </div>
    </div>
  );
};

export default Main;
