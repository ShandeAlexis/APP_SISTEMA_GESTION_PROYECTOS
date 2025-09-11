import axios from "axios";

const baseAPI = axios.create({
  baseURL: "http://localhost:5000/api",
});

// Añadir token automáticamente a cada petición
baseAPI.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default baseAPI;
