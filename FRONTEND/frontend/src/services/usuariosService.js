import baseAPI from "./api";

export const getUsuarios = () => baseAPI.get("/usuario");
export const createUsuario = (data) => baseAPI.post("/usuario", data);
export const getUsuarioById = (id) => baseAPI.get(`/usuario/${id}`);
export const updateUsuario = (id, data) => baseAPI.put(`/usuario/${id}`, data);
export const deleteUsuario = (id) => baseAPI.delete(`/usuario/${id}`);