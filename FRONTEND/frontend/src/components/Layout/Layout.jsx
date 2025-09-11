import { Outlet, useNavigate } from "react-router-dom";
import "./Layout.css"; // tu estilo del header

const Layout = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  return (
    <div className="app-layout">
      <header className="main-header">
        <h1>SISTEMA SYCP</h1>
        <div>
          <button onClick={() => navigate("/main")} className="nav-btn">
            Inicio
          </button>
          <button onClick={handleLogout} className="logout-btn">
            Cerrar Sesión
          </button>
        </div>
      </header>

      {/* 👇 aquí cambia el contenido */}
      <main className="main-content">
        <Outlet />
      </main>
    </div>
  );
};

export default Layout;
