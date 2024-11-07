
namespace CFA.Business.Helpers;

public static class TelefonoHelper
{
    private static readonly string[] TiposTelefonoValidos = { "Casa", "Celular", "Trabajo", "Otro" };
    
    public static bool ValidarTipoTelefono(string tipoTelefono)
    {
        return TiposTelefonoValidos.Contains(tipoTelefono);
    }

    public static bool ValidarFormatoTelefono(string telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono)) return false;
        
        // Verifica que solo contenga dígitos
        if (!System.Text.RegularExpressions.Regex.IsMatch(telefono, @"^\d+$")) 
            return false;
        
        // Verifica longitud mínima y máxima
        return telefono.Length >= 7 && telefono.Length <= 15;
    }

    public static bool EsCelular(string telefono)
    {
        // Verifica si es un número celular (Colombia)
        return telefono.Length == 10 && telefono.StartsWith("3");
    }
}