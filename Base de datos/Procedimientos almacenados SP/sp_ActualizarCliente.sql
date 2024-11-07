-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_ActualizarCliente
    @p_Codigo BIGINT,
    @p_TipoDocumento VARCHAR(2),
    @p_NumeroDocumento VARCHAR(11),
    @p_Nombres VARCHAR(30),
    @p_Apellido1 VARCHAR(30),
    @p_Apellido2 VARCHAR(30) = NULL,
    @p_Genero CHAR(1),
    @p_FechaNacimiento DATE,
    @p_Email VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validar que el cliente exista
        IF NOT EXISTS (SELECT 1 FROM Clientes WHERE Codigo = @p_Codigo)
            THROW 50007, 'El cliente especificado no existe.', 1;
        
        -- Validar que el tipo de documento exista
        IF NOT EXISTS (SELECT 1 FROM TipoDocumento WHERE IdTipoDocumento = @p_TipoDocumento)
            THROW 50001, 'El tipo de documento no es válido.', 1;
        
        -- Validar que el género exista
        IF NOT EXISTS (SELECT 1 FROM Genero WHERE IdGenero = @p_Genero)
            THROW 50002, 'El género especificado no es válido.', 1;
        
        -- Validar documento único (excluyendo el cliente actual)
        IF EXISTS (
            SELECT 1 
            FROM Clientes 
            WHERE TipoDocumento = @p_TipoDocumento 
                AND NumeroDocumento = @p_NumeroDocumento 
                AND Codigo != @p_Codigo
        )
            THROW 50003, 'Ya existe otro cliente con el tipo y número de documento especificado.', 1;
        
        -- Validar edad según tipo de documento
        DECLARE @v_Edad INT = DATEDIFF(YEAR, @p_FechaNacimiento, GETDATE());
        
        IF @p_TipoDocumento = 'RC' AND @v_Edad > 7
            THROW 50004, 'Para Registro Civil la edad debe ser menor a 7 años.', 1;
        IF @p_TipoDocumento = 'TI' AND (@v_Edad < 8 OR @v_Edad > 17)
            THROW 50005, 'Para Tarjeta de Identidad la edad debe estar entre 8 y 17 años.', 1;
        IF @p_TipoDocumento = 'CC' AND @v_Edad < 18
            THROW 50006, 'Para Cédula de Ciudadanía la edad debe ser mayor a 18 años.', 1;
        
        -- Actualizar el cliente
        UPDATE Clientes
        SET TipoDocumento = @p_TipoDocumento,
            NumeroDocumento = @p_NumeroDocumento,
            Nombres = UPPER(@p_Nombres),
            Apellido1 = UPPER(@p_Apellido1),
            Apellido2 = UPPER(@p_Apellido2),
            Genero = @p_Genero,
            FechaNacimiento = @p_FechaNacimiento,
            Email = @p_Email
        WHERE Codigo = @p_Codigo;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
