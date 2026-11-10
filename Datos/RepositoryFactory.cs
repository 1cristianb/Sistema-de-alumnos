using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly MongoClient _mongoClient;
        private readonly string _liteDbConnection;

        public RepositoryFactory(
            string mongoConnection,
            string liteDbConnection)
        {
            _mongoClient = new MongoClient(mongoConnection);
            _liteDbConnection = liteDbConnection;
        }

        public IAlumnoRepository CrearAlumnoRepository(string tipoBD)
        {
            return tipoBD switch
            {
                "MongoDB" => new MongoRepository(_mongoClient),
                "LiteDB" => new LiteDbRepository(_liteDbConnection),
                _ => throw new ArgumentException(
                    "Base de datos no soportada")
            };
        }

        public IUsuarioRepository CrearUsuarioRepository()
        {
            return new LiteDbUsuarioRepository(_liteDbConnection);
        }
    }
}