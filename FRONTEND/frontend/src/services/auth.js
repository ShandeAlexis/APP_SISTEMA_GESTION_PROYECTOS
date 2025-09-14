import baseAPI from "./api";

export const loginUser = (email, password) => {
  return baseAPI.post("/usuario/login", { email, password });
};
