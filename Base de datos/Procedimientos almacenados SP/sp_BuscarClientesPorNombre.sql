-- =============================================
-- Prueba técnica CFA
-- Desarrollado por YDQ
-- Fecha: Noviembre 2024
-- Versión: 1.0
-- =============================================
CREATE OR ALTER PROCEDURE sp_BuscarClientesPorNombre
    @p_TextoBusqueda VARCHAR(90)  -- Permite buscar en nombres y apellidos
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Normalizar el texto de búsqueda
    SET @p_TextoBusqueda = UPPER(LTRIM(RTRIM(@p_TextoBusqueda)));
    
    -- Realizar la búsqueda
    SELECT 
        c.Codigo,
        c.TipoDocumento,
        c.NumeroDocumento,
        c.Nombres,
        c.Apellido1,
        c.Apellido2,
        c.Genero,
        c.FechaNacimiento,
        c.Email
    FROM Clientes c
    WHERE 
        UPPER(c.Nombres) LIKE '%' + @p_TextoBusqueda + '%'
        OR UPPER(c.Apellido1) LIKE '%' + @p_TextoBusqueda + '%'
        OR UPPER(c.Apellido2) LIKE '%' + @p_TextoBusqueda + '%'
        OR UPPER(c.Nombres + ' ' + c.Apellido1 + ISNULL(' ' + c.Apellido2, '')) 
            LIKE '%' + @p_TextoBusqueda + '%'
    ORDER BY 
        c.Nombres ASC,
        c.Apellido1 ASC,
        c.Apellido2 ASC;
END;
GO
