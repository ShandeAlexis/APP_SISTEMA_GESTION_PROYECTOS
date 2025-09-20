// src/hooks/useAuth.js
import { useMemo } from "react";
import { jwtDecode } from "jwt-decode";


export const useAuth = () => {
  const token = localStorage.getItem("token");

  const user = useMemo(() => {
    if (!token) return null;
    try {
      const decoded = jwtDecode(token);
      return {
        email: decoded.sub,
        nombre: decoded.nombre,
        role: decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"], 
      };
    } catch (err) {
      console.error("Error decoding token", err);
      return null;
    }
  }, [token]);

  return { user, isAuthenticated: !!user };
};
