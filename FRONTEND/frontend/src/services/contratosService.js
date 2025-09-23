import baseAPI from "./api";

export const getContratosByProyecto = (proyectoId) => baseAPI.get(`/contrato/proyecto/${proyectoId}`);
export const createContrato = (data) => baseAPI.post("/contrato", data);
export const getContratoById = (idContrato) => baseAPI.get(`/contrato/${idContrato}`);
export const updateContrato = (idContrato, data) => baseAPI.put(`/contrato/${idContrato}`, data);
export const deleteContrato = (idContrato) => baseAPI.delete(`/contrato/${idContrato}`);