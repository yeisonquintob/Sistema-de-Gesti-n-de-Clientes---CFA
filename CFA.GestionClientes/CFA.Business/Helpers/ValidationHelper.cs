

using System.Text.RegularExpressions;

namespace CFA.Business.Helpers;

public static class ValidationHelper
{
    public static bool ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        
        try
        {
            // Patrón de validación de email
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    public static bool ValidarDocumento(string documento)
    {
        if (string.IsNullOrWhiteSpace(documento)) return false;
        
        // Verifica que solo contenga dígitos y longitud máxima
        return Regex.IsMatch(documento, @"^\d+$") && documento.Length <= 11;
    }
}