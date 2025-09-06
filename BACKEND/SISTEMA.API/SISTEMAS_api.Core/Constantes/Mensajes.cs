using System;

namespace SISTEMA.API.SISTEMAS_api.Core.Constantes;

public static class Mensajes
{
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
