using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectCompis2.Models
{
    
    public class AnalizadorSemantico
    {
        List<string[]> tokenList = new List<string[]>();
        List<string> tipos = new List<string> { "int", "string", "double", "bool" };
        List<string> declaraciones = new List<string> { "int", "string", "double", "bool", "const", "class", "interface", "void" };
        List<string> operadores = new List<string> { "+", "-", "*","/","%","<","<=",">",">=","=","==","!=","&&","||","!",";",",",
                                                     ".","[","]","(",")","{","}","[]","()","{}"};
        Analizar analizar = new Analizar();
        AnalizadorSemanticoModel SemanticoModel = new AnalizadorSemanticoModel();
        List<AnalizadorSemanticoModel> analizadors = new List<AnalizadorSemanticoModel>();
        


        public void LeerTokens(List<string[]> listaTokens)
        {
            tokenList = listaTokens;
            AnalizarS();
        }

        public void AnalizarS()
        {
            int contAmbitos = 0;
            string tempType = "";
            string tempIdent = "";
            foreach(var item in tokenList)
            {
                string tipo = item[0];
                var valor = item[1];
                //Evaluar si viene int, double, string, bool y asignarlo a variable temporal lasttype
                if(tipos.Contains(valor))
                {
                    tempType = valor;
                }
                //Evaluar si viene un identificador
                if(tipo == "Identificador")
                {
                    tempIdent = valor;
                }
                if(valor == ";")
                {
                    if(!String.IsNullOrEmpty(tempType) && !String.IsNullOrEmpty(tempIdent))
                    {
                        CrearObjeto(tempType, tempIdent, contAmbitos.ToString(), "0");
                        tempType = "";
                        tempIdent = "";
                    }

                }
            }
        }

        public void CrearObjeto(string tipo, string ident, string ambito, string operacion)
        {
            AnalizadorSemanticoModel analizadorSemanticoModel = new AnalizadorSemanticoModel();
            analizadorSemanticoModel.tipo = tipo;
            analizadorSemanticoModel.valor = ident;
            analizadorSemanticoModel.ambito = ambito;
            analizadorSemanticoModel.operacion = operacion;
            analizadors.Add(analizadorSemanticoModel);

        }
    }
}
