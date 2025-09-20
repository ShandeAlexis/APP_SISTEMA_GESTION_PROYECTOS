import baseAPI from "./api";

export const getIncidencias = () => baseAPI.get("/incidencia");
export const createIncidencia = (data) => baseAPI.post("/incidencia", data);
export const getIncidenciaById = (id) => baseAPI.get(`/incidencia/${id}`);
export const updateIncidencia = (id, data) => baseAPI.put(`/incidencia/${id}`, data);
export const deleteIncidencia = (id) => baseAPI.delete(`/incidencia/${id}`);