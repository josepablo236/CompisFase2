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
        List<string> listaErrores = new List<string>();
        Dictionary<string[], List<AnalizadorSemanticoModel>> TablaDeSimbolos = new Dictionary<string[], List<AnalizadorSemanticoModel>>();

        public void LeerTokens(List<string[]> listaTokens)
        {
            tokenList = listaTokens;
            AnalizarS();
        }

        public void AnalizarS()
        {
            string[] ambito = new string[2];
            int contAmbitos = 0;
            string tempType = "";
            string tempIdent = "";
            //Recorrer la lista de tokens
            foreach (var item in tokenList)
            {
                string tipo = item[0];
                var valor = item[1];
                //Evaluar si viene int, double, string, bool y asignarlo a variable temporal lasttype
                if (tipos.Contains(valor))
                {
                    tempType = valor;
                }
                //Evaluar si viene un identificador
                if (tipo == "Identificador")
                {
                    tempIdent = valor;
                }
                //Cuando termina una declaracion u operacion
                if (valor == ";")
                {
                    //Evaluar que no esten vacios
                    if (!String.IsNullOrEmpty(tempType) && !String.IsNullOrEmpty(tempIdent))
                    {
                        CrearObjeto(tempType, tempIdent, contAmbitos.ToString(), "0");
                        tempType = "";
                        tempIdent = "";
                    }
                }
                //Evaluar declaraciones
                if (valor == "class" || valor == "interface" || valor == "void" || valor == "const")
                {
                    tempType = valor;
                    if(contAmbitos == 0)
                    {
                        ambito[0] = "cero";
                        ambito[1] = "principal";
                        string[] ambit = new string[2];
                        ambit[0] = "cero";
                        ambit[1] = "principal";
                        List<AnalizadorSemanticoModel> analiza = new List<AnalizadorSemanticoModel>();
                        foreach (var x in analizadors){ analiza.Add(x); }
                        TablaDeSimbolos.Add(ambit, analiza);
                        analizadors.Clear();
                        ambito[0] = "";
                        ambito[1] = "";
                    }
                }
                //Evaluar si comienza la llave
                if (valor == "{" || valor == "(")
                {
                    if (!String.IsNullOrEmpty(tempType) && !String.IsNullOrEmpty(tempIdent))
                    {
                        contAmbitos++;
                        ambito[0] = tempType;
                        ambito[1] = tempIdent;
                        tempType = "";
                        tempIdent = "";
                    }
                }
                //Evaluar si termina el ambito
                if (valor == "}")
                {
                    string[] ambit = new string[2];
                    ambit[0] = ambito[0];
                    ambit[1] = ambito[1];
                    List<AnalizadorSemanticoModel> analiza = new List<AnalizadorSemanticoModel>();
                    foreach (var x in analizadors) { analiza.Add(x); }
                    TablaDeSimbolos.Add(ambit, analiza);
                    analizadors.Clear();
                    ambito[0] = "";
                    ambito[1] = "";
                }
            }
            Dictionary<string[], List<AnalizadorSemanticoModel>> Tablatemp = TablaDeSimbolos;

        }

        public void CrearObjeto(string tipo, string ident, string ambitonum, string operacion)
        {
            bool existe = false;
            AnalizadorSemanticoModel model = new AnalizadorSemanticoModel();
            model.tipo = tipo;
            model.valor = ident;
            model.ambito = ambitonum;
            model.operacion = operacion;
            foreach (var item in analizadors)
            {
                if(item.ambito == model.ambito && item.tipo == model.tipo && item.valor == model.valor)
                {
                    existe = true;
                }
            }
            if(!existe)
            {
                analizadors.Add(model);
            }
            else
            {
                listaErrores.Add("La variable '" + model.valor + "' de tipo '" + model.tipo + "' ya fue declarada en el ámbito '" + model.ambito + "'");
            }
        }
    }
}

