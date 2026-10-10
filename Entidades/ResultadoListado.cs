using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    public class ResultadoListado
    {
        public List<AlumnoListadoItem> Items { get; set; } = new List<AlumnoListadoItem>();

        // Mensajes si alguna base no pudo consultarse (ej: MongoDB apagado)
        public List<string> Advertencias { get; set; } = new List<string>();
    }
}
