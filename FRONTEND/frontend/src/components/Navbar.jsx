import { Link, useNavigate } from "react-router-dom";

const Navbar = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  return (
    <nav style={{ padding: "1rem", background: "#282c34", color: "#fff" }}>
      <Link to="/proyectos" style={{ margin: "0 1rem", color: "#61dafb" }}>Proyectos</Link>
      <Link to="/contratos" style={{ margin: "0 1rem", color: "#61dafb" }}>Contratos</Link>
      <Link to="/entregables" style={{ margin: "0 1rem", color: "#61dafb" }}>Entregables</Link>
      <button onClick={handleLogout} style={{ marginLeft: "1rem" }}>Cerrar sesión</button>
    </nav>
  );
};

export default Navbar;
