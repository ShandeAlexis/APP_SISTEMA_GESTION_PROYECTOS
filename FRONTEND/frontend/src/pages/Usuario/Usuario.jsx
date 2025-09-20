import { useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth";
import { useNavigate } from "react-router-dom";
import {
  getUsuarios,
  createUsuario,
  updateUsuario,
  deleteUsuario,
} from "../../services/usuariosService";
import "./Usuario.css";

const Usuarios = () => {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [usuarios, setUsuarios] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [editando, setEditando] = useState(null);

  const rolMap = { ADMIN: 1, USER: 2, INVITADO: 3 };
  const rolOptions = [
    { id: 1, nombre: "ADMIN" },
    { id: 2, nombre: "USER" },
    { id: 3, nombre: "INVITADO" },
  ];

  const [formUsuario, setFormUsuario] = useState({
    nombre: "",
    email: "",
    password: "",
    rolId: 2,
  });

  useEffect(() => {
    if (user?.role !== "ADMIN") {
      navigate("/main");
    } else {
      cargarUsuarios();
    }
  }, [user, navigate]);

  const cargarUsuarios = async () => {
    try {
      const res = await getUsuarios();
      setUsuarios(res.data);
    } catch (err) {
      console.error("Error cargando usuarios", err);
    }
  };

  const handleInputChange = (e) => {
    setFormUsuario({ ...formUsuario, [e.target.name]: e.target.value });
  };

  const handleGuardar = async (e) => {
    e.preventDefault();
    try {
      if (editando) {
        // PUT → no enviar password
        const payload = {
          nombre: formUsuario.nombre,
          email: formUsuario.email,
          rolId: formUsuario.rolId,
        };
        await updateUsuario(editando.id, payload);
      } else {
        // POST → enviar con password
        await createUsuario(formUsuario);
      }
      setShowModal(false);
      setEditando(null);
      setFormUsuario({ nombre: "", email: "", password: "", rolId: 2 });
      cargarUsuarios();
    } catch (err) {
      console.error("Error guardando usuario", err);
    }
  };

  const handleEliminar = async (id) => {
    if (!window.confirm("¿Seguro que deseas eliminar este usuario?")) return;
    try {
      await deleteUsuario(id);
      cargarUsuarios();
    } catch (err) {
      console.error("Error eliminando usuario", err);
    }
  };

  const abrirModalEditar = (usuario) => {
    setEditando(usuario);
    setFormUsuario({
      nombre: usuario.nombre,
      email: usuario.email,
      password: "",
      rolId: rolMap[usuario.rol] || 2, // mapear string a rolId
    });
    setShowModal(true);
  };

  const abrirModalNuevo = () => {
    setEditando(null);
    setFormUsuario({ nombre: "", email: "", password: "", rolId: 2 });
    setShowModal(true);
  };

  return (
    <div className="usuarios-page">
      <h1>Gestión de Usuarios</h1>

      <button className="btn-agregar" onClick={abrirModalNuevo}>
        ➕ Agregar Usuario
      </button>

      <table className="usuarios-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Email</th>
            <th>Rol</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          {usuarios.map((u) => (
            <tr key={u.id}>
              <td>{u.id}</td>
              <td>{u.nombre}</td>
              <td>{u.email}</td>
              <td>{u.rol}</td>
              <td>
                <button
                  className="btn-editar"
                  onClick={() => abrirModalEditar(u)}
                >
                  ✏️ Editar
                </button>
                <button
                  className="btn-eliminar"
                  onClick={() => handleEliminar(u.id)}
                >
                  🗑️ Eliminar
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {/* Modal */}
      {showModal && (
        <div className="modal">
          <div className="modal-content">
            <h2>{editando ? "Editar Usuario" : "Nuevo Usuario"}</h2>
            <form onSubmit={handleGuardar}>
              <label>
                Nombre:
                <input
                  type="text"
                  name="nombre"
                  value={formUsuario.nombre}
                  onChange={handleInputChange}
                  required
                />
              </label>

              <label>
                Email:
                <input
                  type="email"
                  name="email"
                  value={formUsuario.email}
                  onChange={handleInputChange}
                  required
                />
              </label>

              {!editando && (
                <label>
                  Contraseña:
                  <input
                    type="password"
                    name="password"
                    value={formUsuario.password}
                    onChange={handleInputChange}
                    required
                  />
                </label>
              )}

              <label>
                Rol:
                <select
                  name="rolId"
                  value={formUsuario.rolId}
                  onChange={handleInputChange}
                >
                  {rolOptions.map((r) => (
                    <option key={r.id} value={r.id}>
                      {r.nombre}
                    </option>
                  ))}
                </select>
              </label>

              <div className="modal-actions">
                <button type="submit" className="btn-guardar">
                  Guardar
                </button>
                <button
                  type="button"
                  className="btn-cancelar"
                  onClick={() => setShowModal(false)}
                >
                  Cancelar
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Usuarios;
