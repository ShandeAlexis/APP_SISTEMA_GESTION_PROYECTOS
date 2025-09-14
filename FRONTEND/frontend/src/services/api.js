import axios from "axios";

const baseAPI = axios.create({
  baseURL: "http://localhost:5000/api",
});

baseAPI.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default baseAPI;
