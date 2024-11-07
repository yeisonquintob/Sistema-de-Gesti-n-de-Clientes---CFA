-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_ValidarEdadTipoDocumento
    @p_TipoDocumento VARCHAR(2),
    @p_FechaNacimiento DATE
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Calcular la edad
        DECLARE @v_Edad INT = DATEDIFF(YEAR, @p_FechaNacimiento, GETDATE());
        
        -- Validar que el tipo de documento exista
        IF NOT EXISTS (SELECT 1 FROM TipoDocumento WHERE IdTipoDocumento = @p_TipoDocumento)
            THROW 50001, 'El tipo de documento no es válido.', 1;
        
        -- Validaciones según tipo de documento
        IF @p_TipoDocumento = 'RC' AND @v_Edad > 7
            THROW 50002, 'Para Registro Civil la edad debe ser menor a 7 años.', 1;
            
        IF @p_TipoDocumento = 'TI' AND (@v_Edad < 8 OR @v_Edad > 17)
            THROW 50003, 'Para Tarjeta de Identidad la edad debe estar entre 8 y 17 años.', 1;
            
        IF @p_TipoDocumento = 'CC' AND @v_Edad < 18
            THROW 50004, 'Para Cédula de Ciudadanía la edad debe ser mayor a 18 años.', 1;
            
        -- Si llega aquí, la edad es válida para el tipo de documento
        SELECT 'OK' AS Resultado, @v_Edad AS Edad;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
