using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaVirtual.Entidades
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public Autor Autor { get; set; }
        public int AnoPublicado { get; set; }
        public bool Disponivel { get; set; } = true;
    }
}
