using Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public interface IUsuarioRepository
    {
        void Insertar(Usuario usuario);
        Usuario? ObtenerPorNombre(string nombreUsuario);
        int Contar();
    }
}