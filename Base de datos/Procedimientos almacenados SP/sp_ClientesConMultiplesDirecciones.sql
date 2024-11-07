-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_ClientesConMultiplesDirecciones
AS
BEGIN
    SET NOCOUNT ON;
    
    WITH PrimeraDireccion AS (
        SELECT 
            CodigoCliente,
            Direccion,
            ROW_NUMBER() OVER (PARTITION BY CodigoCliente ORDER BY IdDireccion) AS RN
        FROM DireccionesCliente
    )
    SELECT 
        c.Codigo,
        c.Nombres + ' ' + c.Apellido1 + ISNULL(' ' + c.Apellido2, '') AS NombreCompleto,
        pd.Direccion AS PrimeraDireccion,
        COUNT(d.IdDireccion) AS CantidadDirecciones
    FROM 
        Clientes c
        INNER JOIN DireccionesCliente d ON c.Codigo = d.CodigoCliente
        INNER JOIN PrimeraDireccion pd ON c.Codigo = pd.CodigoCliente AND pd.RN = 1
    GROUP BY 
        c.Codigo,
        c.Nombres,
        c.Apellido1,
        c.Apellido2,
        pd.Direccion
    HAVING 
        COUNT(d.IdDireccion) > 1
    ORDER BY 
        NombreCompleto ASC;
END;
GO