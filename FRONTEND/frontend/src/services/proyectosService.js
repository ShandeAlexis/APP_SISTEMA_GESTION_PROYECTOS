import baseAPI from "./api";

export const getProyectos = () => baseAPI.get("/proyecto");
export const createProyecto = (data) => baseAPI.post("/proyecto", data);
export const getProyectoById = (id) => baseAPI.get(`/proyecto/${id}`);
export const updateProyecto = (id, data) => baseAPI.put(`/proyecto/${id}`, data);
export const deleteProyecto = (id) => baseAPI.delete(`/proyecto/${id}`);

export const getReporte = (proyectoId, fecha) => 
  baseAPI.get(`/proyecto/${proyectoId}/reporte`, {
    params: { fechaCorte: fecha }  
  });