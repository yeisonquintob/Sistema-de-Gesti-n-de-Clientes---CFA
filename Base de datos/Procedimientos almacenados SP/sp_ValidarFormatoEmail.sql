-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_ValidarFormatoEmail
    @p_Email VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Variable para almacenar si el formato es válido
        DECLARE @v_FormatoValido BIT = 0;
        
        -- Validar formato de email usando expresión regular
        SET @v_FormatoValido = CASE 
            WHEN @p_Email LIKE '%_@__%.__%' -- Patrón básico: algo@algo.algo
            AND @p_Email NOT LIKE '@%'       -- No puede empezar con @
            AND @p_Email NOT LIKE '%@%@%'    -- No puede tener más de un @
            AND @p_Email NOT LIKE '%..%'     -- No puede tener puntos consecutivos
            AND @p_Email NOT LIKE '.%'       -- No puede empezar con punto
            AND @p_Email NOT LIKE '%.'       -- No puede terminar con punto
            THEN 1
            ELSE 0
        END;
        
        IF @v_FormatoValido = 0
            THROW 50001, 'El formato del email no es válido.', 1;
            
        -- Si llega aquí, el formato es válido
        SELECT 'OK' AS Resultado;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO