-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_ObtenerClientePorCodigo
    @p_Codigo BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Obtener datos del cliente
    SELECT 
        c.Codigo,
        c.TipoDocumento,
        td.Descripcion AS TipoDocumentoDescripcion,
        c.NumeroDocumento,
        c.Nombres,
        c.Apellido1,
        c.Apellido2,
        c.Genero,
        g.Descripcion AS GeneroDescripcion,
        c.FechaNacimiento,
        c.Email,
        c.FechaRegistro
    FROM Clientes c
    INNER JOIN TipoDocumento td ON c.TipoDocumento = td.IdTipoDocumento
    INNER JOIN Genero g ON c.Genero = g.IdGenero
    WHERE c.Codigo = @p_Codigo;
    
    -- Obtener direcciones del cliente
    SELECT 
        IdDireccion,
        Direccion,
        TipoDireccion,
        FechaRegistro
    FROM DireccionesCliente
    WHERE CodigoCliente = @p_Codigo;
    
    -- Obtener teléfonos del cliente
    SELECT 
        IdTelefono,
        NumeroTelefono,
        TipoTelefono,
        FechaRegistro
    FROM TelefonosCliente
    WHERE CodigoCliente = @p_Codigo;
END;
GO
