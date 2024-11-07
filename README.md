# Sistema de Gestión de Clientes - CFA

## 📋 Descripción del Proyecto
Sistema desarrollado para la Cooperativa Financiera de Antioquia (CFA) enfocado en el registro, gestión y control de clientes y/o asociados de sus diferentes sedes y oficinas.

### Información General
- **ID Requerimiento**: 0
- **Proyecto**: Registro, gestión y control de los clientes de CFA
- **Cliente**: Cooperativa Financiera de Antioquia
- **Objetivo**: Registrar y controlar las transacciones realizadas por los clientes de CFA
- **Alcance**: Registrar los clientes en el sistema de la entidad
- **Responsables**: Jefe de Procesos - Jefe de Operaciones

### Entregables
1. Código Fuente de la solución implementada
2. Scripts de creación de Base de datos, tablas y población de datos
3. Documentación de configuraciones adicionales
4. Proyecto en POSTMAN/SOAPUI para pruebas

## 🚀 Características Principales

### Precondiciones Técnicas
1. Desarrollo en C# (.NET 8)
2. Base de datos SQL Server relacional
3. Conexiones parametrizadas en archivo de configuración
4. CRUD con implementación de stored procedures
5. Implementación de LINQ para filtros
6. Validaciones con expresiones regulares

### Funcionalidades Clave
- Gestión completa de clientes (CRUD)
- Manejo múltiple de direcciones y teléfonos
- Validaciones específicas por tipo de documento
- Búsquedas especializadas
- API REST documentada

## 🛠️ Tecnologías Utilizadas
- **Backend**: .NET 8
- **Base de Datos**: SQL Server
- **Documentación**: Swagger/OpenAPI
- **ORM**: Entity Framework Core 8
- **Validación**: FluentValidation
- **Mapeo**: AutoMapper
- **Testing**: xUnit

## 📁 Estructura del Proyecto

### Solución .NET
```
CFA.GestionClientes/
├── CFA.API                 # Web API
├── CFA.Business           # Lógica de negocio
├── CFA.Common             # Utilidades compartidas
├── CFA.DataAccess        # Acceso a datos
├── CFA.Entities          # Modelos y DTOs
└── CFA.Tests             # Pruebas unitarias
```

### Base de Datos
```
database/
├── create/
│   ├── tables/
│   └── constraints/
├── stored-procedures/
│   ├── cliente/
│   ├── direccion/
│   └── telefono/
└── data/
    └── initial_data.sql
```

## ⚙️ Configuración del Entorno

### Requisitos Previos
1. SQL Server 2019 o superior
2. .NET SDK 8.0
3. Visual Studio 2022/VS Code
4. Git

### Instalación

#### 1. Base de Datos
1. Ejecutar scripts en orden:
```sql
/database/create/tables.sql
/database/create/constraints.sql
/database/stored-procedures/**/*.sql
/database/data/initial_data.sql
```

#### 2. Aplicación
1. Clonar repositorio:
```bash
git clone [url-repositorio]
cd cfa-gestion-clientes
```

2. Configurar conexión en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DB_CFA;User Id=sa;Password=your-password;TrustServerCertificate=True"
  }
}
```

3. Restaurar y ejecutar:
```bash
dotnet restore
dotnet build
cd CFA.API
dotnet run
```

## 📝 Especificaciones Funcionales

### Campos de Cliente
| Campo | Tipo | Obligatorio | Longitud | Observaciones |
|-------|------|-------------|-----------|---------------|
| Código | Numérico | Sí | 11 | Autoincremental |
| Tipo documento | Lista | Sí | - | CC, TI, RC |
| Número documento | Numérico | Sí | 11 | - |
| Nombres | Alfanumérico | Sí | 30 | - |
| Apellido 1 | Texto | Sí | 30 | - |
| Apellido 2 | Texto | No | 30 | - |
| Género | Lista | Sí | - | F, M |
| Fecha nacimiento | Fecha | Sí | - | Formato dd/mm/aaaa |
| Direcciones | Colección | Sí | - | Mínimo una |
| Teléfonos | Colección | Sí | - | Mínimo uno |
| Email | Texto | Sí | - | Formato válido |

### Validaciones de Negocio

#### Tipo Documento por Edad
- RC: 0 – 7 años
- TI: 8 – 17 años
- CC: >18 años

#### Validaciones Adicionales
- Documento único en el sistema
- Código único autoincremental
- Email con formato válido
- Mínimo una dirección y teléfono

### Funcionalidades de Búsqueda
1. Por nombre completo (orden A-Z)
2. Por número documento (mayor a menor)
3. Por rango de fecha nacimiento
4. Clientes con múltiples teléfonos
5. Clientes con múltiples direcciones

## 📚 API Endpoints

### Catálogos
```http
GET /api/v1/catalogos/tipos-documento
GET /api/v1/catalogos/generos
```

### Clientes
```http
GET    /api/v1/clientes
GET    /api/v1/clientes/{codigo}
POST   /api/v1/clientes
PUT    /api/v1/clientes/{codigo}
DELETE /api/v1/clientes/{codigo}
```

### Búsquedas Especializadas
```http
GET /api/v1/clientes/buscar/nombre/{texto}
GET /api/v1/clientes/buscar/documento/{numero}
GET /api/v1/clientes/buscar/fecha-nacimiento?fechaInicial={fecha}&fechaFinal={fecha}
GET /api/v1/clientes/multiples-telefonos
GET /api/v1/clientes/multiples-direcciones
```

### Ejemplo de Creación
```json
POST /api/v1/clientes
{
  "tipoDocumento": "CC",
  "numeroDocumento": "1234567890",
  "nombres": "Juan",
  "apellido1": "Pérez",
  "apellido2": "Gómez",
  "genero": "M",
  "fechaNacimiento": "1990-01-01",
  "email": "juan.perez@example.com",
  "direcciones": [
    {
      "direccion": "Calle 123 #45-67",
      "tipoDireccion": "Casa"
    }
  ],
  "telefonos": [
    {
      "numeroTelefono": "3001234567",
      "tipoTelefono": "Celular"
    }
  ]
}
```

## 🧪 Pruebas

### Ejecutar Pruebas
```bash
cd CFA.Tests
dotnet test
```

### Colección Postman
1. Importar `docs/CFA.postman_collection.json`
2. Configurar variables de ambiente
3. Ejecutar pruebas

## 📖 Documentación
- Swagger UI: `https://localhost:7001/swagger`
- Documentación técnica: `docs/`
- Scripts SQL: `database/`

## 🔄 Control de Versiones
- Main: Código en producción
- Develop: Desarrollo activo
- Feature/*: Nuevas características
- Bugfix/*: Correcciones

## 👥 Equipo y Soporte
[Información de contacto del equipo]

## 📄 Licencia
[Especificar licencia]
