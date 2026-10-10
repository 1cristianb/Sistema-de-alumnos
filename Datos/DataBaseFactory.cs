using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public static class DatabaseFactory
    {
        public static IAlumnoRepository GetRepository(string tipoBD, string cadenaConexion)
        {
            return tipoBD switch
            {
                "MongoDB" => new MongoRepository(cadenaConexion),
                "LiteDB" => new LiteDbRepository(cadenaConexion),
                _ => throw new ArgumentException("Base de datos no soportada")
            };
        }

        public static IUsuarioRepository GetUsuarioRepository(string cadenaConexionLiteDb)
        {
            return new LiteDbUsuarioRepository(cadenaConexionLiteDb);
        }
    }
}