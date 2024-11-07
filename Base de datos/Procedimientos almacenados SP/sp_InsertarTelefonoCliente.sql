-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_InsertarTelefonoCliente
    @p_CodigoCliente BIGINT,
    @p_NumeroTelefono VARCHAR(15),
    @p_TipoTelefono VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validar que el cliente exista
        IF NOT EXISTS (SELECT 1 FROM Clientes WHERE Codigo = @p_CodigoCliente)
            THROW 50001, 'El cliente especificado no existe.', 1;
            
        -- Validar formato de teléfono (solo números)
        IF @p_NumeroTelefono LIKE '%[^0-9]%'
            THROW 50002, 'El número de teléfono solo debe contener dígitos.', 1;
            
        -- Validar longitud mínima del teléfono
        IF LEN(@p_NumeroTelefono) < 7
            THROW 50003, 'El número de teléfono debe tener al menos 7 dígitos.', 1;
            
        -- Insertar el teléfono
        INSERT INTO TelefonosCliente (CodigoCliente, NumeroTelefono, TipoTelefono)
        VALUES (@p_CodigoCliente, @p_NumeroTelefono, UPPER(@p_TipoTelefono));
        
        -- Retornar el ID generado
        SELECT SCOPE_IDENTITY() AS IdTelefono;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO