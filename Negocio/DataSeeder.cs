using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Negocio
{
    public static class DataSeeder
    {
        private const string Ingenieria = "Ingeniería en Informática";
        private const string Gestion = "Licenciatura en Gestión de la Tecnología";
        private const string TecWeb = "Tecnicatura Universitaria en Web";

        public static List<string> Sembrar(string cadenaLiteDb, string cadenaMongo)
        {
            var log = new List<string>();

            try
            {
                if (new UsuarioService().CrearUsuarioInicial(cadenaLiteDb))
                    log.Add("Usuario inicial creado: admin / admin123");
            }
            catch (Exception ex)
            {
                log.Add($"No se pudo crear el usuario inicial: {ex.Message}");
            }

            SembrarAlumnos("LiteDB", cadenaLiteDb, AlumnosLiteDb(), log);
            SembrarAlumnos("MongoDB", cadenaMongo, AlumnosMongo(), log);
            return log;
        }

        private static void SembrarAlumnos(string tipoBD, string cadena, List<(Alumno, List<Curso>)> datos, List<string> log)
        {
            try
            {
                IAlumnoRepository repo = DatabaseFactory.GetRepository(tipoBD, cadena);

                if (repo.ListarTodos().Count > 0)
                {
                    log.Add($"{tipoBD}: ya tiene datos, no se cargan ejemplos.");
                    return;
                }

                foreach (var (alumno, cursos) in datos)
                {
                    repo.Insertar(alumno); // asigna alumno.Id
                    foreach (var curso in cursos)
                    {
                        curso.AlumnoId = alumno.Id!;
                        repo.InsertarCurso(curso);
                    }
                }
                log.Add($"{tipoBD}: se cargaron {datos.Count} alumnos de ejemplo.");
            }
            catch (TimeoutException)
            {
                log.Add($"{tipoBD}: no se pudo conectar, se omiten los datos de ejemplo.");
            }
            catch (Exception ex)
            {
                log.Add($"{tipoBD}: error al cargar ejemplos: {ex.Message}");
            }
        }

        private static List<(Alumno, List<Curso>)> AlumnosLiteDb() => new()
        {
            Crear("Lucía", "Fernández", "40123456", "L-1001", Ingenieria, new DateTime(2001, 3, 14), 850,
                  "lucia.fernandez@alumno.unlam.edu.ar", "11 4455-1020", "Av. Rivadavia 1450, San Justo",
                  ("Programación Avanzada II", 2026), ("Bases de Datos", 2026)),
            Crear("Matías", "González", "41234567", "L-1002", Gestion, new DateTime(2000, 7, 22), 720,
                  "matias.gonzalez@alumno.unlam.edu.ar", "11 5566-2031", "Pueyrredón 320, Ramos Mejía",
                  ("Programación Avanzada II", 2026)),
            Crear("Camila", "Rodríguez", "42345678", "L-1003", TecWeb, new DateTime(2002, 11, 5), 560,
                  "camila.rodriguez@alumno.unlam.edu.ar", "11 4488-3042", "Italia 780, Isidro Casanova",
                  ("Diseño Web", 2026)),
            Crear("Nicolás", "Pereyra", "39456789", "L-1004", Ingenieria, new DateTime(1999, 1, 30), 940,
                  "nicolas.pereyra@alumno.unlam.edu.ar", "11 6677-4053", "General Paz 55, Ciudadela",
                  ("Estructuras de Datos", 2025), ("Programación Avanzada II", 2026))
        };

        private static List<(Alumno, List<Curso>)> AlumnosMongo() => new()
        {
            Crear("Sofía", "Martínez", "43111222", "M-2001", Gestion, new DateTime(2003, 5, 18), 780,
                  "sofia.martinez@alumno.unlam.edu.ar", "11 4411-5064", "Illia 1200, González Catán",
                  ("Programación Avanzada II", 2026), ("Gestión de Proyectos", 2026)),
            Crear("Joaquín", "Ramírez", "42222333", "M-2002", Ingenieria, new DateTime(2002, 9, 9), 650,
                  "joaquin.ramirez@alumno.unlam.edu.ar", "11 5522-6075", "Brown 90, San Justo",
                  ("Bases de Datos", 2026)),
            Crear("Valentina", "Sosa", "41333444", "M-2003", TecWeb, new DateTime(2001, 12, 1), 890,
                  "valentina.sosa@alumno.unlam.edu.ar", "11 4433-7086", "Belgrano 410, Morón",
                  ("Diseño Web", 2026), ("Programación Avanzada II", 2026)),
            Crear("Tomás", "Acosta", "40444555", "M-2004", Ingenieria, new DateTime(2000, 4, 27), 430,
                  "tomas.acosta@alumno.unlam.edu.ar", "11 6644-8097", "Salta 2300, Laferrere")
        };

        private static (Alumno, List<Curso>) Crear(
            string nombre, string apellido, string dni, string legajo, string carrera,
            DateTime nacimiento, int puntaje, string email, string telefono, string direccion,
            params (string curso, int anio)[] cursos)
        {
            var alumno = new Alumno
            {
                Nombre = nombre,
                Apellido = apellido,
                DNI = dni,
                Legajo = legajo,
                Carrera = carrera,
                FechaNacimiento = nacimiento,
                PuntajeIngreso = puntaje,
                Email = email,
                Telefono = telefono,
                Direccion = direccion
            };

            var listaCursos = new List<Curso>();
            foreach (var (curso, anio) in cursos)
                listaCursos.Add(new Curso { NombreCurso = curso, Anio = anio });

            return (alumno, listaCursos);
        }
    }
}