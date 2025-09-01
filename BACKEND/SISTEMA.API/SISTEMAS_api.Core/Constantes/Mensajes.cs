using System;

namespace SISTEMA.API.SISTEMAS_api.Core.Constantes;

public static class Mensajes
{
    public static class Entregable
    {
        public const string NoEncontrado = "No se encontró un entregable con el ID especificado.";
        public const string IdInvalido = "El ID de la URL no coincide con el del objeto.";
        public const string ErrorActualizar = "Ocurrió un error al actualizar el entregable.";
        public const string Eliminado = "El entregable fue eliminado correctamente.";
    }

    public static class General
    {
        public const string ErrorInterno = "Ocurrió un error inesperado en el servidor.";
    }
}
