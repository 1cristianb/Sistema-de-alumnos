# Sistema de Gestión de Alumnos - UNLaM

Trabajo práctico de **Programación Avanzada II** desarrollado con ASP.NET Core MVC y .NET 10.

La aplicación permite:

- Registrar, listar, buscar, editar y eliminar alumnos.
- Asociar y eliminar cursos de un alumno.
- Registrar usuarios e iniciar sesión mediante autenticación por cookies.
- Persistir alumnos, cursos y usuarios en MongoDB mediante Entity Framework Core.

## Arquitectura

La solución está organizada en capas:

1. **Presentación (`Presentacion`)**: controladores y vistas Razor de ASP.NET Core MVC.
2. **Negocio (`Negocio`)**: servicios y reglas de negocio.
3. **Datos (`Datos`)**: `UniversidadDbContext` y repositorios implementados con Entity Framework Core.
4. **Entidades (`Entidades`)**: clases de dominio y modelos utilizados entre las capas.

Los servicios reciben las interfaces `IAlumnoRepository` e `IUsuarioRepository` mediante inyección de dependencias. Las implementaciones concretas se registran en `Program.cs`.

## Persistencia

Se utiliza una única base MongoDB llamada `UniversidadDB`, con estas colecciones:

- `Alumnos`
- `Cursos`
- `Usuarios`

El acceso se implementa con el proveedor oficial `MongoDB.EntityFrameworkCore`. La cadena de conexión y el nombre de la base se configuran en `Presentacion/appsettings.json`.

## Requisitos

- SDK de .NET 10.
- Docker con Docker Compose, o una instancia de MongoDB disponible en `localhost:27017`.

## Ejecución

Desde la raíz del proyecto:

```bash
docker compose -f MongoDB-dc/docker-compose.yaml up -d
dotnet restore
dotnet run --project Presentacion
```

La URL local se muestra en la salida de `dotnet run`. Con la configuración incluida normalmente es:

```text
http://localhost:5260
```

Si la colección de usuarios está vacía, el sistema crea este usuario inicial:

```text
Usuario: admin
Contraseña: admin123
```

El usuario inicial es únicamente para uso local y académico.

## Pruebas de integración

Las pruebas de `Datos.IntegrationTests` verifican el acceso a MongoDB mediante Entity Framework Core y requieren que MongoDB esté disponible.

Cada prueba utiliza una base temporal independiente y la elimina al finalizar. No modifica `UniversidadDB`.

```bash
dotnet test
```

Por defecto se conectan a `mongodb://localhost:27017`. Para utilizar otra instancia:

```bash
MONGODB_TEST_CONNECTION="mongodb://servidor:27017" dotnet test
```
