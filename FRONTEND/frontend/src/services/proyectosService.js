import baseAPI from "./api";

export const getProyectos = () => baseAPI.get("/proyecto");
export const deleteProyecto = (id) => baseAPI.delete(`/proyecto/${id}`);