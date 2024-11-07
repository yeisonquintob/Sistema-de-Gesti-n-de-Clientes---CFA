-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_BuscarClientesPorRangoFechaNacimiento
    @p_FechaInicial DATE,
    @p_FechaFinal DATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Validar que la fecha inicial no sea mayor que la final
    IF @p_FechaInicial > @p_FechaFinal
        THROW 50001, 'La fecha inicial no puede ser mayor que la fecha final.', 1;
        
    SELECT 
        c.FechaNacimiento,
        c.Nombres + ' ' + c.Apellido1 + ISNULL(' ' + c.Apellido2, '') AS NombreCompleto,
        DATEDIFF(YEAR, c.FechaNacimiento, GETDATE()) AS Edad
    FROM 
        Clientes c
    WHERE 
        c.FechaNacimiento BETWEEN @p_FechaInicial AND @p_FechaFinal
    ORDER BY 
        c.FechaNacimiento ASC;
END;
GO