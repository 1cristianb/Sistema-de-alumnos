using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly string _mongoConnection;
        private readonly string _liteDbConnection;

        public RepositoryFactory(
            string mongoConnection,
            string liteDbConnection)
        {
            _mongoConnection = mongoConnection;
            _liteDbConnection = liteDbConnection;
        }

        public IAlumnoRepository CrearAlumnoRepository(string tipoBD)
        {
            return tipoBD switch
            {
                "MongoDB" => new MongoRepository(_mongoConnection),
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