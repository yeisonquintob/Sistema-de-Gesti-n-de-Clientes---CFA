-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_BuscarClientesPorDocumento
    @p_NumeroDocumento VARCHAR(11)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        c.NumeroDocumento,
        td.Descripcion AS TipoDocumento,
        c.Nombres + ' ' + c.Apellido1 + ISNULL(' ' + c.Apellido2, '') AS NombreCompleto
    FROM 
        Clientes c
        INNER JOIN TipoDocumento td ON c.TipoDocumento = td.IdTipoDocumento
    WHERE 
        c.NumeroDocumento LIKE '%' + @p_NumeroDocumento + '%'
    ORDER BY 
        CAST(c.NumeroDocumento AS BIGINT) DESC;
END;
GO