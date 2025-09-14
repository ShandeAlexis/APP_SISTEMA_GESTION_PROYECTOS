import baseAPI from "./api";


export const getCurvasByEntregable = (entregableId) => baseAPI.get(`/curva/entregable/${entregableId}`);

export const updateCurva = (curvaId, data) => baseAPI.put(`/curva/${curvaId}`, data);