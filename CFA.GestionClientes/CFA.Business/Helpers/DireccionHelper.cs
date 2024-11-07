

namespace CFA.Business.Helpers;

public static class DireccionHelper
{
    private static readonly string[] TiposDireccionValidos = { "Casa", "Trabajo", "Otro" };
    
    public static bool ValidarTipoDireccion(string tipoDireccion)
    {
        return TiposDireccionValidos.Contains(tipoDireccion);
    }

    public static bool ValidarFormatoDireccion(string direccion)
    {
        if (string.IsNullOrWhiteSpace(direccion)) return false;
        
        // Verifica longitud
        if (direccion.Length > 100) return false;
        
        // Verifica caracteres válidos: letras, números, espacios y caracteres especiales comunes en direcciones
        return System.Text.RegularExpressions.Regex.IsMatch(
            direccion, 
            @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\-\#\,\.]+$"
        );
    }
}