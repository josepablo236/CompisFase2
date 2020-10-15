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
        public  string[] vector = new string[2];

        public void Simbol(string NombreSimbolo, string ValorV1,string ValorV2, int Estado)
        {
            vector[0] = ValorV1;
            vector[1] = ValorV2;
            simbolAction.Add(NombreSimbolo, vector);
        }
        public Dictionary<string, string[]> ObtenerDic()
        {
            return simbolAction;
        }

    }
}
