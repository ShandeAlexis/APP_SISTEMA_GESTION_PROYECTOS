using System;

namespace SISTEMA.API.SISTEMAS_api.Core.Constantes;

public static class Mensajes
{
    public static class Usuario
    {
        public const string ErrorObtener = "Error al obtener los usuarios.";
        public const string ErrorCrear = "Error al crear el usuario.";
        public const string ErrorActualizar = "Error al actualizar el usuario.";
        public const string ErrorEliminar = "Error al eliminar el usuario.";
        public const string ErrorLogin = "Error en el proceso de login.";
        public const string NoEncontrado = "Usuario no encontrado.";
        public const string CredencialesInvalidas = "Email o contraseña incorrectos.";
    }

    public static class Proyecto
    {
        public const string NoEncontrado = "No se encontró el proyecto con ese ID.";

        public const string ErrorCrear = "Error al crear el proyecto.";
        public const string ErrorObtener = "Error al obtener el proyecto.";
        public const string ErrorActualizar = "Ocurrió un error al actualizar el proyecto.";
        public const string Eliminado = "El proyecto fue eliminado correctamente.";
        public const string ErrorEliminar = "Error al eliminar el proyecto.";
    }
    public static class Contrato
    {
        public const string NoEncontrado = "No se encontró el contrato con ese ID.";

        public const string ErrorCrear = "Error al crear el contrato.";
        public const string ErrorObtener = "Error al obtener el contrato.";
        public const string ErrorActualizar = "Ocurrió un error al actualizar el contrato.";
        public const string Eliminado = "El contrato fue eliminado correctamente.";
        public const string ErrorEliminar = "Error al eliminar el contrato.";
    }
    public static class Entregable
    {
        public const string NoEncontrado = "No se encontró el entregable con ese ID.";

        public const string ErrorCrear = "Error al crear el entregable.";
        public const string ErrorObtener = "Error al obtener el entregable.";
        public const string ErrorActualizar = "Ocurrió un error al actualizar el entregable.";
        public const string Eliminado = "El entregable fue eliminado correctamente.";
        public const string ErrorEliminar = "Error al eliminar el entregable.";
    }

    public static class General
    {
        public const string ErrorInterno = "Ocurrió un error inesperado en el servidor.";
    }
}
