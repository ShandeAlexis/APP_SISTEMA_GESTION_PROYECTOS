import baseAPI from "./api";

// Login → recibe email y password
export const loginUser = (email, password) => {
  return baseAPI.post("/usuario/login", { email, password });
};
