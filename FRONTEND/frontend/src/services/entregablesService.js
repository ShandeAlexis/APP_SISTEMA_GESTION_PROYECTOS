import baseAPI from "./api";

export const getEntregablesByContrato = (contratoId) => baseAPI.get(`/entregable/contrato/${contratoId}`);
export const getEntregableById = (id) => baseAPI.get(`/entregable/${id}`);
export const createEntregable = (data) => baseAPI.post("/entregable",data)
export const updateEntregable = (id, data) => baseAPI.put(`/entregable/${id}`, data);
export const deleteEntregable = (id) => baseAPI.delete(`/entregable/${id}`);

