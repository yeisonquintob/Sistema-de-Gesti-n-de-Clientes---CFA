-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_ObtenerDireccionesCliente
    @p_CodigoCliente BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Validar que el cliente exista
    IF NOT EXISTS (SELECT 1 FROM Clientes WHERE Codigo = @p_CodigoCliente)
        THROW 50001, 'El cliente especificado no existe.', 1;
    
    -- Retornar todas las direcciones del cliente
    SELECT 
        IdDireccion,
        Direccion,
        TipoDireccion,
        FechaRegistro
    FROM DireccionesCliente
    WHERE CodigoCliente = @p_CodigoCliente
    ORDER BY TipoDireccion, FechaRegistro;
END;
GO