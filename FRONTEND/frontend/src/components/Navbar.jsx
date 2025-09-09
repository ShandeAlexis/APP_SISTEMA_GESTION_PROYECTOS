import { Link } from "react-router-dom";

const Navbar = () => (
  <nav style={{ padding: "1rem", background: "#282c34", color: "#fff" }}>
    <Link to="/proyectos" style={{ margin: "0 1rem", color: "#61dafb" }}>Proyectos</Link>
    <Link to="/contratos" style={{ margin: "0 1rem", color: "#61dafb" }}>Contratos</Link>
    <Link to="/entregables" style={{ margin: "0 1rem", color: "#61dafb" }}>Entregables</Link>
  </nav>
);

export default Navbar;
