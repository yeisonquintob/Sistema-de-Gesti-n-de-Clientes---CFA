-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_EliminarTelefonoCliente
    @p_IdTelefono BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Obtener el código del cliente
        DECLARE @v_CodigoCliente BIGINT;
        SELECT @v_CodigoCliente = CodigoCliente
        FROM TelefonosCliente
        WHERE IdTelefono = @p_IdTelefono;
        
        -- Validar que el teléfono exista
        IF @v_CodigoCliente IS NULL
            THROW 50001, 'El teléfono especificado no existe.', 1;
            
        -- Validar que no sea el único teléfono del cliente
        IF (SELECT COUNT(*) FROM TelefonosCliente WHERE CodigoCliente = @v_CodigoCliente) <= 1
            THROW 50002, 'No se puede eliminar el único teléfono del cliente.', 1;
            
        -- Eliminar el teléfono
        DELETE FROM TelefonosCliente
        WHERE IdTelefono = @p_IdTelefono;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO