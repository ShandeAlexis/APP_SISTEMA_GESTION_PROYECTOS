import baseAPI from "./api";

export const getContratosByProyecto = (proyectoId) => baseAPI.get(`/contrato/proyecto/${proyectoId}`);

export const deleteContrato = (idContrato) => baseAPI.delete(`/contrato/${idContrato}`);