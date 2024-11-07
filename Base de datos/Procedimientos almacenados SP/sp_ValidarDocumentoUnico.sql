-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_ValidarDocumentoUnico
    @p_TipoDocumento VARCHAR(2),
    @p_NumeroDocumento VARCHAR(11),
    @p_CodigoCliente BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Variable para almacenar si existe o no el documento
        DECLARE @v_Existe BIT = 0;
        
        -- Validar que el tipo de documento exista
        IF NOT EXISTS (SELECT 1 FROM TipoDocumento WHERE IdTipoDocumento = @p_TipoDocumento)
            THROW 50001, 'El tipo de documento no es válido.', 1;
        
        -- Verificar existencia del documento
        IF @p_CodigoCliente IS NULL
        BEGIN
            -- Caso nuevo cliente
            SET @v_Existe = CASE 
                                WHEN EXISTS (
                                    SELECT 1 
                                    FROM Clientes 
                                    WHERE TipoDocumento = @p_TipoDocumento 
                                    AND NumeroDocumento = @p_NumeroDocumento
                                ) THEN 1
                                ELSE 0
                            END;
        END
        ELSE
        BEGIN
            -- Caso actualización cliente
            SET @v_Existe = CASE 
                                WHEN EXISTS (
                                    SELECT 1 
                                    FROM Clientes 
                                    WHERE TipoDocumento = @p_TipoDocumento 
                                    AND NumeroDocumento = @p_NumeroDocumento 
                                    AND Codigo != @p_CodigoCliente
                                ) THEN 1
                                ELSE 0
                            END;
        END
        
        IF @v_Existe = 1
            THROW 50002, 'Ya existe un cliente registrado con el tipo y número de documento especificado.', 1;
        
        -- Si llega aquí, el documento es único
        SELECT 'OK' AS Resultado;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO