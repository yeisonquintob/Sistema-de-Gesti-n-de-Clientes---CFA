Sistema de Gestión de Clientes - CFA
📋 Descripción del Proyecto
Sistema desarrollado para la Cooperativa Financiera de Antioquia (CFA) enfocado en el registro, gestión y control de clientes y/o asociados de sus diferentes sedes y oficinas.

Información General
ID Requerimiento: 0
Proyecto: Registro, gestión y control de los clientes de CFA
Cliente: Cooperativa Financiera de Antioquia
Objetivo: Registrar y controlar las transacciones realizadas por los clientes de CFA
Alcance: Registrar los clientes en el sistema de la entidad
Entregables
Código Fuente de la solución implementada
Scripts de creación de Base de datos, tablas y población de datos
Documentación de configuraciones adicionales
Proyecto en POSTMAN/SOAPUI para pruebas
🚀 Características Principales
Precondiciones Técnicas
Desarrollo en C# (.NET 8)
Base de datos SQL Server relacional
Conexiones parametrizadas en archivo de configuración
CRUD con implementación de stored procedures
Implementación de LINQ para filtros
Validaciones con expresiones regulares
Funcionalidades Clave
Gestión completa de clientes (CRUD)
Manejo múltiple de direcciones y teléfonos
Validaciones específicas por tipo de documento
Búsquedas especializadas
API REST documentada
🛠️ Tecnologías Utilizadas
Backend: .NET 8
Base de Datos: SQL Server
Documentación: Swagger/OpenAPI
ORM: Entity Framework Core 8
Validación: FluentValidation
Mapeo: AutoMapper
Testing: xUnit
📁 Estructura del Proyecto
Solución .NET
CFA.GestionClientes/
├── CFA.API                 # Web API
├── CFA.Business           # Lógica de negocio
├── CFA.Common             # Utilidades compartidas
├── CFA.DataAccess        # Acceso a datos
├── CFA.Entities          # Modelos y DTOs
└── CFA.Tests             # Pruebas unitarias
Base de Datos
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
⚙️ Configuración del Entorno
Requisitos Previos
SQL Server 2019 o superior
.NET SDK 8.0
Visual Studio 2022/VS Code
Git
Instalación
1. Base de Datos
Ejecutar scripts en orden:
/database/create/tables.sql
/database/create/constraints.sql
/database/stored-procedures/**/*.sql
/database/data/initial_data.sql
2. Aplicación
Clonar repositorio:
git clone [url-repositorio]
cd cfa-gestion-clientes
Configurar conexión en appsettings.json:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DB_CFA;User Id=sa;Password=your-password;TrustServerCertificate=True"
  }
}
Restaurar y ejecutar:
dotnet restore
dotnet build
cd CFA.API
dotnet run
📝 Especificaciones Funcionales
Campos de Cliente
Campo	Tipo	Obligatorio	Longitud	Observaciones
Código	Numérico	Sí	11	Autoincremental
Tipo documento	Lista	Sí	-	CC, TI, RC
Número documento	Numérico	Sí	11	-
Nombres	Alfanumérico	Sí	30	-
Apellido 1	Texto	Sí	30	-
Apellido 2	Texto	No	30	-
Género	Lista	Sí	-	F, M
Fecha nacimiento	Fecha	Sí	-	Formato dd/mm/aaaa
Direcciones	Colección	Sí	-	Mínimo una
Teléfonos	Colección	Sí	-	Mínimo uno
Email	Texto	Sí	-	Formato válido
Validaciones de Negocio
Tipo Documento por Edad
RC: 0 – 7 años
TI: 8 – 17 años
CC: >18 años
Validaciones Adicionales
Documento único en el sistema
Código único autoincremental
Email con formato válido
Mínimo una dirección y teléfono
Funcionalidades de Búsqueda
Por nombre completo (orden A-Z)
Por número documento (mayor a menor)
Por rango de fecha nacimiento
Clientes con múltiples teléfonos
Clientes con múltiples direcciones
📚 API Endpoints
Catálogos
GET /api/v1/catalogos/tipos-documento
GET /api/v1/catalogos/generos
Clientes
GET    /api/v1/clientes
GET    /api/v1/clientes/{codigo}
POST   /api/v1/clientes
PUT    /api/v1/clientes/{codigo}
DELETE /api/v1/clientes/{codigo}
Búsquedas Especializadas
GET /api/v1/clientes/buscar/nombre/{texto}
GET /api/v1/clientes/buscar/documento/{numero}
GET /api/v1/clientes/buscar/fecha-nacimiento?fechaInicial={fecha}&fechaFinal={fecha}
GET /api/v1/clientes/multiples-telefonos
GET /api/v1/clientes/multiples-direcciones
Ejemplo de Creación
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
🧪 Pruebas
Ejecutar Pruebas
cd CFA.Tests
dotnet test
Colección Postman
Importar docs/CFA.postman_collection.json
Configurar variables de ambiente
Ejecutar pruebas
📖 Documentación
Swagger UI: https://localhost:7001/swagger
Documentación técnica: docs/
Scripts SQL: database/
🔄 Control de Versiones
Main: Código en producción
Develop: Yeison Quinto
Feature/*: Nuevas características
Bugfix/*: Correcciones
👥 Equipo y Soporte
[Información de contacto del equipo]

📄 Licencia
[Especificar licencia]
