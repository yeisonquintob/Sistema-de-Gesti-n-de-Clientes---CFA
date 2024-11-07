-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_EliminarCliente
    @p_Codigo BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validar que el cliente exista
        IF NOT EXISTS (SELECT 1 FROM Clientes WHERE Codigo = @p_Codigo)
            THROW 50007, 'El cliente especificado no existe.', 1;
        
        -- Eliminar teléfonos del cliente
        DELETE FROM TelefonosCliente WHERE CodigoCliente = @p_Codigo;
        
        -- Eliminar direcciones del cliente
        DELETE FROM DireccionesCliente WHERE CodigoCliente = @p_Codigo;
        
        -- Eliminar el cliente
        DELETE FROM Clientes WHERE Codigo = @p_Codigo;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
