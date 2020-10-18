using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectCompis2.Models
{
    class ParseoModel
    {
        public Stack<int> pila { get; set; }
        public Stack<string> SimbolosLeidos { get; set; }
        public List<string> Accion { get; set; }
    }
}
