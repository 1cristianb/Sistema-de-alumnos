using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using Entidades;

namespace Datos
{
    public class LiteDbRepository : IAlumnoRepository
    {
        private string _dbPath;

        //constructor pide la ruta del archivo
        public LiteDbRepository(string connectionString)
        {
            _dbPath = connectionString;
        }

        public void Insertar(Alumno alumno)
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                var col = db.GetCollection<Alumno>("Alumnos");
                alumno.Id = ObjectId.NewObjectId().ToString();
                col.Insert(alumno);
            }
        }

        public List<Alumno> ListarTodos()
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                var col = db.GetCollection<Alumno>("Alumnos");
                var lista = col.FindAll().ToList();
                return lista;
            }
        }
        public Alumno? ObtenerPorId(string id)
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                return db.GetCollection<Alumno>("Alumnos").FindById(id);
            }
        }
        public void Actualizar(Alumno alumno)
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                db.GetCollection<Alumno>("Alumnos").Update(alumno);
            }
        }
        public void Eliminar(string id)
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                db.GetCollection<Curso>("Cursos").DeleteMany(c => c.AlumnoId == id);
                db.GetCollection<Alumno>("Alumnos").Delete(id);
            }
        }
        public void InsertarCurso(Curso curso)
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                var col = db.GetCollection<Curso>("Cursos");
                curso.Id = ObjectId.NewObjectId().ToString();
                col.Insert(curso);
            }
        }
        public List<Curso> ListarCursos()
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                return db.GetCollection<Curso>("Cursos").FindAll().ToList();
            }
        }
        public void EliminarCurso(string cursoId)
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                db.GetCollection<Curso>("Cursos").Delete(cursoId);
            }
        }
    }
}