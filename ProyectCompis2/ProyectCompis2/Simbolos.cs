using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectCompis2
{
    class Simbolos
    {

        public Dictionary<string, string[]> simbolAction = new Dictionary<string, string[]>();
        Dictionary<int, Dictionary<string, string[]>> dicGeneral = new Dictionary<int, Dictionary<string, string[]>>();
        public  string[] vector = new string[2];

        public  Simbolos(string NombreSimbolo, string ValorV1,string ValorV2, int Estado)
        {
            vector[0] = ValorV1;
            vector[1] = ValorV2;
            simbolAction.Add(NombreSimbolo, vector);
            dicGeneral.Add(Estado, simbolAction);
        }
        public Dictionary<int, Dictionary<string, string[]>> ObtenerDicGeneral()
        {
            return dicGeneral;
        }

    }
}
