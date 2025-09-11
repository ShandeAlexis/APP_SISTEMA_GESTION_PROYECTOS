import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { loginUser } from "../../services/auth";
import "./Login.css";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    try {
      const res = await loginUser(email, password);

      // tu backend debería devolver un token
      const token = res.data.token;

      console.log("Token recibido:", token); // para depuración

      if (token) {
        localStorage.setItem("token", token);
        navigate("/main"); // redirige a la página privada
      } else {
        setError("El servidor no devolvió token");
      }
    } catch (err) {
      console.error(err);
      setError("Credenciales inválidas");
    }
  };

  return (
    <div className="login-page">
      <div className="login-container">
        <img
          src="https://tse1.mm.bing.net/th/id/OIP.94YmQr4Cch20fnLLJ2RAtgHaHa?r=0&cb=ucfimgc2&rs=1&pid=ImgDetMain&o=7&rm=3"
          alt="Imagen de perfil"
          className="profile-img"
        />

        <h2>Iniciar Sesión</h2>

        <form onSubmit={handleSubmit}>
          <input
            type="email"
            placeholder="Correo electrónico"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />

          <input
            type="password"
            placeholder="Contraseña"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />

          <button type="submit">Entrar</button>
        </form>

        {error && <p id="message">{error}</p>}
      </div>
    </div>
  );
};

export default Login;
