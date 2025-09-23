import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import Login from "./pages/Login/Login";
import Main from "./pages/Main/Main";
import ProyectosList from "./pages/Proyectos/ProyectosList";
import Layout from "./components/Layout/Layout";

import ContratosList from "./pages/Contrato/ContratosList.jsx";
import EntregablesList from "./pages/Entregable/EntregablesList.jsx";
import ReporteMain from "./pages/Reporte/ReporteMain.jsx";
import Cronograma from "./pages/Cronograma/Cronograma.jsx";
import Usuario from "./pages/Usuario/Usuario.jsx";
import Incidencias from "./pages/Incidencias/Incidencias.jsx";

// Protege rutas privadas
const PrivateRoute = ({ children }) => {
  const token = localStorage.getItem("token");
  return token ? children : <Navigate to="/login" />;
};

const App = () => {
  return (
    <Router>
      <Routes>
        {/* Login público */}
        <Route path="/login" element={<Login />} />

        {/* Rutas privadas con layout */}
        <Route
          element={
            <PrivateRoute>
              <Layout />
            </PrivateRoute>
          }
        >
          <Route path="/main" element={<Main />} />
          <Route path="/proyectos" element={<ProyectosList />} />

          {/* Contratos de un proyecto */}
          <Route path="/proyectos/:id/contratos" element={<ContratosList />} />
          {/* Entregables de un contrato */}
          <Route
            path="/proyectos/:id/contratos/:contratoId/entregables"
            element={<EntregablesList />}
          />
          <Route path="/reportes" element={<ReporteMain />} />
          <Route path="/cronograma" element={<Cronograma />} />
          <Route path="/incidencias" element={<Incidencias />} />


          <Route path="/usuarios" element={<Usuario />} />
        </Route>

        {/* Default */}
        <Route path="/" element={<Navigate to="/main" />} />
      </Routes>
    </Router>
  );
};

export default App;
