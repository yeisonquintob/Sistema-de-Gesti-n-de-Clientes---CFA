-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_ObtenerTelefonosCliente
    @p_CodigoCliente BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Validar que el cliente exista
        IF NOT EXISTS (SELECT 1 FROM Clientes WHERE Codigo = @p_CodigoCliente)
            THROW 50001, 'El cliente especificado no existe.', 1;
            
        -- Obtener información del cliente
        DECLARE @v_NombreCompleto VARCHAR(92);
        
        SELECT @v_NombreCompleto = CONCAT(
            Nombres, ' ',
            Apellido1,
            CASE 
                WHEN Apellido2 IS NOT NULL THEN ' ' + Apellido2
                ELSE ''
            END
        )
        FROM Clientes
        WHERE Codigo = @p_CodigoCliente;
        
        -- Retornar los teléfonos del cliente
        SELECT 
            t.IdTelefono,
            t.NumeroTelefono,
            t.TipoTelefono,
            t.FechaRegistro,
            @v_NombreCompleto AS NombreCliente,
            @p_CodigoCliente AS CodigoCliente,
            CASE 
                WHEN EXISTS (
                    SELECT 1 
                    FROM TelefonosCliente 
                    WHERE CodigoCliente = @p_CodigoCliente 
                    HAVING COUNT(*) > 1
                ) THEN 'S'
                ELSE 'N'
            END AS TieneMultiplesTelefonos
        FROM TelefonosCliente t
        WHERE t.CodigoCliente = @p_CodigoCliente
        ORDER BY 
            CASE t.TipoTelefono 
                WHEN 'Casa' THEN 1
                WHEN 'Celular' THEN 2
                WHEN 'Trabajo' THEN 3
                ELSE 4
            END,
            t.FechaRegistro;
            
        -- Si no hay teléfonos, lanzar advertencia
        IF NOT EXISTS (SELECT 1 FROM TelefonosCliente WHERE CodigoCliente = @p_CodigoCliente)
            THROW 50002, 'El cliente no tiene teléfonos registrados.', 1;
            
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO