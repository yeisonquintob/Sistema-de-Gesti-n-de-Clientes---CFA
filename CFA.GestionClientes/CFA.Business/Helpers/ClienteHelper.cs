
namespace CFA.Business.Helpers;

public static class ClienteHelper
{
    public static int CalcularEdad(DateTime fechaNacimiento)
    {
        var hoy = DateTime.Today;
        var edad = hoy.Year - fechaNacimiento.Year;
        if (fechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
        return edad;
    }

    public static bool ValidarTipoDocumentoPorEdad(string tipoDocumento, int edad)
    {
        return tipoDocumento switch
        {
            "RC" => edad >= 0 && edad <= 7,
            "TI" => edad >= 8 && edad <= 17,
            "CC" => edad >= 18,
            _ => false
        };
    }
}