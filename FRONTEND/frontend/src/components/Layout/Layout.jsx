import { Outlet, useNavigate } from "react-router-dom";
import { useState } from "react";
import "./Layout.css";

const Layout = () => {
  const navigate = useNavigate();
  const [menuOpen, setMenuOpen] = useState(false);

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  return (
    <div className="app-layout">
      {/* Sidebar */}
      <aside className={`sidebar ${menuOpen ? "open" : ""}`}>
        <div className="logo">
          <img src="https://pdci.com.pe/wp-content/uploads/2022/10/Mesa-de-trabajo-11-1024x328.png" alt="Logo" />
        </div>

        <div className="menu">
          <button onClick={() => { navigate("/main"); setMenuOpen(false); }}>🏠 Inicio</button>
          <button onClick={() => { navigate("/proyectos"); setMenuOpen(false); }}>📂 Proyectos</button>
          <button onClick={() => { navigate("/reportes"); setMenuOpen(false); }}>📊 Reportes</button>
          <button>📈 Curvas</button>
          <button  onClick={() => { navigate("/cronograma"); setMenuOpen(false); }}>📅 Cronograma </button>
          <button>👤 Usuario </button>
        </div>

        <div className="logout">
          <button onClick={handleLogout}>➜]</button>
        </div>
      </aside>

      {/* Contenido */}
      <main className="main-content">
        {/* Botón hamburguesa para móviles */}
        <button className="hamburger" onClick={() => setMenuOpen(!menuOpen)}>
          ☰
        </button>

        <Outlet />
      </main>
    </div>
  );
};

export default Layout;
