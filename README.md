# Sistema de Gestión de Alumnos - UNLAM

Este proyecto fue desarrollado como Trabajo Práctico para la materia **Programación Avanzada II** de la Licenciatura en Gestión Tecnológica (Universidad Nacional de La Matanza - DIIT)

El sistema consiste en una aplicación web desarrollada con **ASP.NET Core MVC** que permite la administración completa (ABM) de una entidad principal (`Alumno`) y una entidad de relación (`Curso`), implementando además un sistema de autenticación (Login).

El enfoque técnico principal de este proyecto es demostrar el **acceso a bases de datos NoSQL desde .NET**, consumiendo dos motores de bases de datos NoSQL distintos (MongoDB y LiteDB) en la misma solución.

---

## 🚀 Arquitectura del Proyecto

El sistema está estructurado bajo una **Arquitectura de N-Capas** para garantizar el bajo acoplamiento y la alta cohesión, permitiendo la comunicación mediante el intercambio de objetos

1. **Presentación (`/Presentacion`):** Interfaz de usuario desarrollada con ASP.NET Core MVC. Incluye controladores (`AlumnoController.cs`, `CursoController.cs`, `AccountController.cs`), vistas con Layout (`_Layout.cshtml`), y vistas parciales (`_BuscadorPartial.cshtml`, `_FormAlumno.cshtml`)
2. **Negocio (`/Negocio`):** Contiene la lógica principal de la aplicación mediante servicios (`AlumnoService.cs`, `CursoService.cs`, `UsuarioService.cs`)
3. **Datos (`/Datos`):** Capa de acceso a datos que implementa el patrón Repository mediante interfaces (`IAlumnoRepository.cs`, `IUsuarioRepository.cs`). Utiliza un `DataBaseFactory.cs` para orquestar y decidir la conexión a las distintas bases de datos NoSQL.
4. **Entidades (`/Entidades`):** Clases y modelos de dominio que transitan por todas las capas (`Alumno.cs`, `Curso.cs`, `Usuario.cs`)
---

## 🗄️ Acceso a Bases de Datos NoSQL

Este proyecto cumple con el objetivo de **comprender cómo consumir bases de datos NoSQL utilizando proyectos .NET**. Para ello, la aplicación consume **dos bases de datos NoSQL distintas**:

### 1. MongoDB (Base de Datos Documental)
Se utiliza el driver oficial `MongoDB.Driver` para .NET. En la capa de datos (`MongoRepository.cs`), la conexión se gestiona instanciando el cliente a través de una cadena de conexión y accediendo a las colecciones de documentos (equivalente a tablas). Es ideal para entornos de producción escalables.

### 2. LiteDB (Base de Datos Documental Embebida)
LiteDB es una base de datos NoSQL rápida, liviana y *serverless* (sin servidor) escrita completamente en C#. Todo el almacenamiento se guarda en un único archivo físico (por ejemplo, `universidad.db`). 
En el proyecto, la implementación se encuentra en `LiteDbRepository.cs` y `LiteDbUsuarioRepository.cs`. 

**Ejemplo de uso de LiteDB en el código:**
```csharp
// El acceso se realiza mapeando las clases (POCOs) directamente a BSON documents
using (var db = new LiteDatabase(@"Datos\universidad.db"))
{
    var alumnosCollection = db.GetCollection<Alumno>("Alumnos");
    // Al insertar, LiteDB genera automáticamente un ObjectId o utiliza el Id definido
    alumnosCollection.Insert(nuevoAlumno); 
}
```
• > [!TIP]
Nota de Arquitectura: La clase DataBaseFactory.cs actúa como orquestador. Dependiendo de la configuración seleccionada (por ejemplo, desde el appsettings.json), la factoría inyectará la implementación de MongoRepository o LiteDbRepository en los servicios de la capa de Negocio, demostrando una arquitectura flexible e independiente del motor de base de datos.
