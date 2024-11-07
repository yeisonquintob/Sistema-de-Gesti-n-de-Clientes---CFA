-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_ClientesConMultiplesTelefonos
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        c.Codigo,
        c.Nombres + ' ' + c.Apellido1 + ISNULL(' ' + c.Apellido2, '') AS NombreCompleto,
        COUNT(t.IdTelefono) AS CantidadTelefonos
    FROM 
        Clientes c
        INNER JOIN TelefonosCliente t ON c.Codigo = t.CodigoCliente
    GROUP BY 
        c.Codigo,
        c.Nombres,
        c.Apellido1,
        c.Apellido2
    HAVING 
        COUNT(t.IdTelefono) > 1
    ORDER BY 
        CantidadTelefonos DESC,
        NombreCompleto ASC;
END;
GO
