import baseAPI from "./api";

export const getEntregablesByContrato = (contratoId) => baseAPI.get(`/entregable/contrato/${contratoId}`);
export const deleteEntregable = (id) => baseAPI.delete(`/entregable/${id}`);