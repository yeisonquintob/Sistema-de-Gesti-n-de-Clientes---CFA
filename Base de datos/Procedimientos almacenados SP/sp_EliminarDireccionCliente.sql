-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_EliminarDireccionCliente
    @p_IdDireccion BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @v_CodigoCliente BIGINT;
        DECLARE @v_CantidadDirecciones INT;
        
        -- Obtener el código del cliente y validar que la dirección exista
        SELECT @v_CodigoCliente = CodigoCliente
        FROM DireccionesCliente
        WHERE IdDireccion = @p_IdDireccion;
        
        IF @v_CodigoCliente IS NULL
            THROW 50001, 'La dirección especificada no existe.', 1;
            
        -- Contar cuántas direcciones tiene el cliente
        SELECT @v_CantidadDirecciones = COUNT(*)
        FROM DireccionesCliente
        WHERE CodigoCliente = @v_CodigoCliente;
        
        -- Validar que no sea la única dirección
        IF @v_CantidadDirecciones = 1
            THROW 50002, 'No se puede eliminar la única dirección del cliente.', 1;
        
        -- Eliminar la dirección
        DELETE FROM DireccionesCliente
        WHERE IdDireccion = @p_IdDireccion;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO