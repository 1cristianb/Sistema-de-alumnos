using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Entidades;

namespace Datos
{
    public class MongoRepository : IAlumnoRepository
    {
        private readonly IMongoCollection<Alumno> _alumnos;
        private readonly IMongoCollection<Curso> _cursos;

        public MongoRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("UniversidadDB");
            _alumnos = database.GetCollection<Alumno>("Alumnos");
            _cursos = database.GetCollection<Curso>("Cursos");
        }

        public void Insertar(Alumno alumno)
        {
            alumno.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            _alumnos.InsertOne(alumno);
        }

        public List<Alumno> ListarTodos()
        {
            return _alumnos.Find(_ => true).ToList();
        }

        public Alumno? ObtenerPorId(string id)
        {
            return _alumnos.Find(a => a.Id == id).FirstOrDefault();
        }

        public void Actualizar(Alumno alumno)
        {
            _alumnos.ReplaceOne(a => a.Id == alumno.Id, alumno);
        }

        public void Eliminar(string id)
        {
            _cursos.DeleteMany(c => c.AlumnoId == id);
            _alumnos.DeleteOne(a => a.Id == id);
        }

        public void InsertarCurso(Curso curso)
        {
            curso.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            _cursos.InsertOne(curso);
        }

        public List<Curso> ListarCursos()
        {
            return _cursos.Find(_ => true).ToList();
        }

        public void EliminarCurso(string cursoId)
        {
            _cursos.DeleteOne(c => c.Id == cursoId);
        }
    }
}
