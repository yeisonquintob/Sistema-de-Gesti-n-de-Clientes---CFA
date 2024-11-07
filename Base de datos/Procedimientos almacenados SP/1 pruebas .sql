BEGIN TRAN -- Insertar 
    EXEC sp_InsertarCliente
        'CC',  -- TipoDocumento: Cédula de Ciudadanía
        '12345678901',  -- NumeroDocumento
        'María Fernanda',  -- Nombres
        'González',  -- Apellido1
        'Lopez',  -- Apellido2
        'F',  -- Sexo: Femenino
        '1990/10/10',  -- CiudadResidencia
        'maria.gonzalez@example.com';  -- Email
    SELECT * FROM Clientes

    EXEC sp_InsertarDireccionCliente
        10005,  -- CódigoCliente (asumiendo que es el código del cliente que se acaba de insertar)
        'Calle 123 #45-67, Medellín',  -- Dirección
        'Casa';  -- TipoDireccion: Casa    
    SELECT * FROM DireccionesCliente

    EXEC sp_InsertarTelefonoCliente
        10002,  -- CódigoCliente
        '3001234567',  -- NumeroTelefono
        'Celular';  -- TipoTelefono
    SELECT * FROM TelefonosCliente
ROLLBACK TRAN

BEGIN TRAN -- Actualizar 
    EXEC sp_ActualizarCliente 
        10005, 
        'CC', 
        '123456789', 
        'Juanita', 
        'Pérez', 
        'Gómez', 
        'F', 
        '1998-10-10', 
        'juanita.perez@email.com';
    SELECT * FROM Clientes;

    EXEC sp_ActualizarDireccionCliente 
        10002, 
        10005, 
        'Calle 46 #25-123', 
        'Casa';
    SELECT * FROM DireccionesCliente;

    EXEC sp_ActualizarTelefonoCliente 
        @p_IdTelefono = 1,               -- ID del teléfono a actualizar
        @p_CodigoCliente = 10005,        -- Código del cliente JUANITA PÉREZ GÓMEZ
        @p_NumeroTelefono = '3216549870',-- Nuevo número de teléfono
        @p_TipoTelefono = 'Celular';      -- Tipo de teléfono
    SELECT * FROM TelefonosCliente
ROLLBACK TRAN

BEGIN TRAN -- Validar  
    EXEC sp_ValidarDocumentoUnico 
        'CC',  -- TipoDocumento: Cédula de Ciudadanía
        '12345678901';  -- NumeroDocumento

    EXEC sp_ValidarEdadTipoDocumento 
        'CC',  -- TipoDocumento: Cédula de Ciudadanía
        '1998-10-10';  -- FechaNacimiento (persona de 25 años)

    EXEC sp_ValidarFormatoEmail 
    'usuario.ejemplo@dominio.com'; 
ROLLBACK TRAN

BEGIN TRAN -- Obtener   
    EXEC sp_ObtenerClientePorCodigo 
        @p_Codigo = 10005;
    SELECT * FROM Clientes;

    EXEC sp_ObtenerDireccionesCliente 
        @p_CodigoCliente = 10005;
    SELECT * FROM DireccionesCliente;

    EXEC sp_ObtenerTelefonosCliente 
        @p_CodigoCliente = 10005;
    SELECT * FROM TelefonosCliente;
ROLLBACK TRAN

BEGIN TRAN -- Buscar  
    EXEC sp_BuscarClientesPorDocumento
        @p_NumeroDocumento = '123456789';
    SELECT * FROM Clientes;

    EXEC sp_BuscarClientesPorNombre
        @p_TextoBusqueda = 'JUANITA PÉREZ';
    SELECT * FROM Clientes;

    EXEC sp_BuscarClientesPorRangoFechaNacimiento
        @p_FechaInicial = '1986-01-01',
        @p_FechaFinal = '2020-12-31';
    SELECT * FROM Clientes;
ROLLBACK TRAN

BEGIN TRAN -- Buscar Especificas
    EXEC sp_ClientesConMultiplesDirecciones;
    EXEC sp_ClientesConMultiplesTelefonos;
ROLLBACK TRAN

BEGIN TRAN -- Eliminar
    EXEC sp_EliminarCliente @p_Codigo = 10005;
    SELECT * FROM Clientes

    EXEC sp_EliminarDireccionCliente @p_IdDireccion = 1;  -- Cambia 1 por el IdDireccion real
    
    EXEC sp_EliminarTelefonoCliente @p_IdTelefono = 1;  -- Cambia 1 por el IdTelefono real

ROLLBACK TRAN