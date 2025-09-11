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
        </Route>

        {/* Default */}
        <Route path="/" element={<Navigate to="/main" />} />
      </Routes>
    </Router>
  );
};

export default App;
