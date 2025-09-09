import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import Navbar from "./components/Navbar";
import ProyectosList from "./pages/Proyectos/ProyectosList";

const App = () => {
  return (
    <Router>
      <Navbar />
      <div className="container">
        <Routes>
          <Route path="/" element={<Navigate to="/proyectos" />} />

          <Route path="/proyectos" element={<ProyectosList />} />

        </Routes>
      </div>
    </Router>
  );
};

export default App;
