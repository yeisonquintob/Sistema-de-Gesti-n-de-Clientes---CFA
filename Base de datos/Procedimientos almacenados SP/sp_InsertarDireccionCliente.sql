-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_InsertarDireccionCliente
    @p_CodigoCliente BIGINT,
    @p_Direccion VARCHAR(100),
    @p_TipoDireccion VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validar que el cliente exista
        IF NOT EXISTS (SELECT 1 FROM Clientes WHERE Codigo = @p_CodigoCliente)
            THROW 50001, 'El cliente especificado no existe.', 1;
            
        -- Validar que la dirección no esté vacía
        IF LTRIM(RTRIM(@p_Direccion)) = ''
            THROW 50002, 'La dirección no puede estar vacía.', 1;
            
        -- Validar el tipo de dirección
        IF NOT @p_TipoDireccion IN ('Casa', 'Trabajo', 'Otro')
            THROW 50003, 'El tipo de dirección no es válido. Debe ser: Casa, Trabajo u Otro.', 1;
        
        -- Insertar la dirección
        INSERT INTO DireccionesCliente (
            CodigoCliente,
            Direccion,
            TipoDireccion
        ) VALUES (
            @p_CodigoCliente,
            UPPER(LTRIM(RTRIM(@p_Direccion))),
            @p_TipoDireccion
        );
        
        -- Retornar el ID generado
        SELECT SCOPE_IDENTITY() AS IdDireccion;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO