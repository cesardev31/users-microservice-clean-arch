# Microservicio de Usuarios

Este microservicio gestiona los usuarios, perfiles y eventos de autenticación dentro del sistema distribuido. Está construido siguiendo los principios de Clean Architecture para asegurar una alta mantenibilidad, testabilidad y desacoplamiento.

## Capas de la Arquitectura

La solución está organizada en las siguientes capas:

1.  **Core.Domain**: Entidades de negocio e interfaces principales. Contiene la definición de la entidad Usuario y los contratos de acceso a datos.
2.  **Core.Application**: Casos de uso y lógica de negocio. Se encarga de la orquestación de datos entre la interfaz de usuario y la capa de persistencia.
3.  **Infrastructure**: Detalles de implementación. Incluye la persistencia con MongoDB y el cliente de mensajería para RabbitMQ.
4.  **API**: Punto de entrada de la aplicación. Gestiona las peticiones HTTP, middleware e inyección de dependencias.

## Tecnologías Utilizadas

- **Framework**: .NET 9.0 (ASP.NET Core)
- **Base de Datos**: MongoDB (NoSQL) para almacenamiento de perfiles de usuario de alto rendimiento.
- **Mensajería**: RabbitMQ como agente de mensajes para sincronización basada en eventos.
- **Documentación**: Scalar API Reference (OpenAPI 3.1) para una documentación moderna e interactiva.
- **Validación**: FluentValidation para asegurar la integridad de los datos de entrada.
- **Inyección de Dependencias**: Contenedor nativo de .NET.

## Características Principales

- **Manejo Global de Excepciones**: Middleware personalizado para asegurar respuestas de error consistentes.
- **Comunicación Basada en Eventos**: Publica automáticamente un evento `UserCreated` en RabbitMQ al registrar nuevos usuarios.
- **Arquitectura Limpia (Clean Architecture)**: Separación estricta de responsabilidades para permitir la evolución independiente de las capas.
- **Alto Rendimiento**: Uso de controladores de MongoDB optimizados y procesamiento asíncrono.

## Primeros Pasos

### Requisitos Previos

- Docker y Docker Compose
- SDK de .NET 9

### Infraestructura Local

Inicie los servicios necesarios utilizando Docker Compose (desde el repositorio de infraestructura):

```bash
docker compose up -d
```

Esto levantará:

- MongoDB en `localhost:27017`
- RabbitMQ en `localhost:5672` (Panel de administración en `http://localhost:15672`)

### Ejecución de la Aplicación

Ejecute el siguiente comando en el directorio raíz:

```bash
dotnet run
```

El servicio estará disponible en `http://localhost:5057`.

## Documentación

La documentación interactiva de la API se puede consultar en:

- **Scalar**: `http://localhost:5057/scalar/v1`

## Endpoints de la API

- `GET /api/users`: Recuperar todos los usuarios.
- `GET /api/users/{id}`: Recuperar un usuario específico por su GUID.
- `POST /api/users`: Crear un nuevo usuario (dispara validaciones y eventos).
- `GET /api/users/health`: Verificación de salud del servicio.
- `GET /api/users/status`: Estado operativo del servicio.
