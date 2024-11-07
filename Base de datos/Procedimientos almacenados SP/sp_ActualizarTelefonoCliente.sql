-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.1
-- =============================================
CREATE OR ALTER PROCEDURE sp_ActualizarTelefonoCliente
    @p_IdTelefono BIGINT,
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

        -- Validar que el teléfono exista y pertenezca al cliente
        IF NOT EXISTS (SELECT 1 FROM TelefonosCliente 
                      WHERE IdTelefono = @p_IdTelefono 
                      AND CodigoCliente = @p_CodigoCliente)
            THROW 50002, 'El teléfono especificado no existe o no pertenece al cliente indicado.', 1;
            
        -- Validar formato de teléfono (solo números)
        IF @p_NumeroTelefono LIKE '%[^0-9]%'
            THROW 50003, 'El número de teléfono solo debe contener dígitos.', 1;
            
        -- Validar longitud mínima del teléfono
        IF LEN(@p_NumeroTelefono) < 7
            THROW 50004, 'El número de teléfono debe tener al menos 7 dígitos.', 1;

        -- Validar el tipo de teléfono
        IF NOT @p_TipoTelefono IN ('Casa', 'Celular', 'Trabajo', 'Otro')
            THROW 50005, 'El tipo de teléfono no es válido. Debe ser: Casa, Celular, Trabajo u Otro.', 1;
        
        -- Actualizar el teléfono
        UPDATE TelefonosCliente
        SET NumeroTelefono = @p_NumeroTelefono,
            TipoTelefono = UPPER(@p_TipoTelefono)
        WHERE IdTelefono = @p_IdTelefono
        AND CodigoCliente = @p_CodigoCliente;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO