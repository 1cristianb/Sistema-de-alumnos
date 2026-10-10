using Entidades;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class LiteDbUsuarioRepository : IUsuarioRepository
    {
        private readonly string _dbPath;

        public LiteDbUsuarioRepository(string connectionString)
        {
            _dbPath = connectionString;
        }

        public void Insertar(Usuario usuario)
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                var col = db.GetCollection<Usuario>("Usuarios");
                col.EnsureIndex(u => u.NombreUsuario, true); // índice único
                usuario.Id = ObjectId.NewObjectId().ToString();
                col.Insert(usuario);
            }
        }

        public Usuario? ObtenerPorNombre(string nombreUsuario)
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                return db.GetCollection<Usuario>("Usuarios").FindOne(u => u.NombreUsuario == nombreUsuario);
            }
        }

        public int Contar()
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                return db.GetCollection<Usuario>("Usuarios").Count();
            }
        }
    }
}