using System;
using System.Collections.Generic;
using System.IO;
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
        List<string> tiposValores = new List<string> { "Base10", "Double", "Booleano", "String", "null", };
        List<string> operadores = new List<string> { "+", "-", "*","/","%","<","<=",">",">=","=","==","!=","&&","||","!",";",",",
                                                     ".","[","]","(",")","{","}","[]","()","{}"};
        Analizar analizar = new Analizar();
        AnalizadorSemanticoModel SemanticoModel = new AnalizadorSemanticoModel();
        List<AnalizadorSemanticoModel> analizadors = new List<AnalizadorSemanticoModel>();
        List<string[]> listaOperacion = new List<string[]>();
        List<string> listaErrores = new List<string>();
        Dictionary<string, List<AnalizadorSemanticoModel>> TablaDeSimbolos = new Dictionary<string, List<AnalizadorSemanticoModel>>();
        List<string> TablaImprimir = new List<string>();

        public void LeerTokens(List<string[]> listaTokens)
        {
            tokenList = listaTokens;
            AnalizarS();
            AnalizadorSemanticofrm analizadorSemanticofrm = new AnalizadorSemanticofrm();
            EscribirTablaDeSimbolos();
            analizadorSemanticofrm.MostrarErrores(listaErrores);
        }
        public void EscribirTablaDeSimbolos()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\\TablaDeSimbolos.txt";
            var numEstado = 0;
            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (var stw = new StreamWriter(stream))
                {
                    foreach (var item in TablaImprimir)
                    {
                        stw.WriteLine(item);
                    }
                }
            }
        }
        public void AnalizarS()
        {
            List<string> parametros = new List<string>();
            string[] ambito = new string[2];
            string[] valFuncion = new string[2];
            int contAmbitos = 0;
            string tempType = "";
            string tempIdent = "";
            bool declClase = false;
            bool errorAmbito = false;
            bool hereda = false;
            bool ambitoAbierto = false;
            bool funEnAmbito = false;
            bool declInterface = false;
            bool declVoid = false;
            bool declFuncion = false;
            bool declararVariable = false;
            bool asignarVariable = false;
            bool declararConstante = false;
            bool nuevaDecl = false;
            bool error = false;
            //Recorrer la lista de tokens
            foreach (var item in tokenList)
            {
                string tipo = item[0];
                var valor = item[1];
                //Evaluar si viene const
                if(valor == "const")
                {
                    tempType = valor;
                    declararConstante = true;
                }
                //Evaluar si viene int, double, string, bool y asignarlo a variable temporal temptype
                if (tipos.Contains(valor))
                {
                    if (declararConstante)
                    {
                        tempType += "_" + valor;
                        declararConstante = false;
                        declararVariable = true;
                    }
                    else
                    {
                        tempType = valor;
                        declararVariable = true;
                    }
                }
                //Evaluar si viene un identificador
                if (tipo == "Identificador")
                {
                    //Declarar nuevo ambito
                    if (nuevaDecl)
                    {
                        if(hereda)
                        {
                            if (TablaDeSimbolos.ContainsKey(valor))
                            {
                                tempIdent += valor;
                            }
                            else
                            {
                                listaErrores.Add("La clase o interfaz heredada: " + valor + " no existe");
                            }
                        }
                        else
                        {
                            tempIdent = valor;
                        }
                    }
                    //Declarar variables
                    else if(declararVariable)
                    {
                        tempIdent = valor;
                    }
                    //Declarar void
                    else if(declVoid)
                    {
                        tempIdent = valor;
                    }
                    //Asignacion u operacion
                    else
                    {
                        var identObject = BuscarEnTabla(ambito, valor);
                        if(identObject != null)
                        {
                            //Si es un ident de operacion guarda el valor
                            if (asignarVariable)
                            {
                                var ope = new string[2];
                                ope[0] = identObject.tipo;
                                ope[1] = identObject.valor;
                                listaOperacion.Add(ope);
                            }
                            //Si es el que recibe el valor guarda el nombre del ident
                            else
                            {
                                var ope = new string[2];
                                ope[0] = identObject.tipo;
                                ope[1] = identObject.nombre;
                                listaOperacion.Add(ope);
                            }
                        }
                        else
                        {
                            listaErrores.Add("La variable '" + valor + " no existe en el ámbito '" + contAmbitos + "'");
                            error = true;
                        }
                    }
                }
                //Evaluar una asignacion
                if(valor == "=")
                {
                    var ope = new string[2];
                    ope[0] = "asigna";
                    ope[1] = "=";
                    listaOperacion.Add(ope);
                    asignarVariable = true;
                }
                //Evaluar valores numericos o strings
                if(tiposValores.Contains(tipo))
                {
                    var ope = new string[2];
                    if(tipo == "Base10")
                    {
                        tipo = "int";
                    }
                    if(tipo == "Double")
                    {
                        tipo = "double";
                    }
                    if (tipo == "String")
                    {
                        tipo = "string";
                    }
                    if(tipo == "Booleano")
                    {
                        tipo = "bool";
                    }
                    ope[0] = tipo;
                    ope[1] = valor;
                    listaOperacion.Add(ope);
                }
                //Cuando termina una declaracion u operacion
                if (valor == ";")
                {
                    //Evaluar que no esten vacios
                    if(declararVariable)
                    {
                        InsertarEnTabla(tempType, tempIdent, contAmbitos.ToString(), parametros);
                        tempType = "";
                        tempIdent = "";
                        declararVariable = false;
                    }
                    else if(asignarVariable)
                    {
                        AsignarValor(listaOperacion);
                        asignarVariable = false;
                    }
                    else if(declInterface)
                    {
                        declInterface = false;
                        funEnAmbito = false;
                    }
                }
                //Evaluar declaraciones de clases e interfaces 
                if (valor == "class" || valor == "interface" || valor == "void")
                {
                    if(ambitoAbierto)
                    {
                        if(valor == "void")
                        {
                            funEnAmbito = true;
                        }
                    }
                    nuevaDecl = true;
                    tempType = valor;
                    //Meter ambito cero al diccionario
                    if(contAmbitos == 0)
                    {
                        ambito[0] = "cero";
                        ambito[1] = "principal";
                        string[] ambit = new string[2];
                        ambit[0] = "cero";
                        ambit[1] = "principal";
                        List<AnalizadorSemanticoModel> analiza = new List<AnalizadorSemanticoModel>();
                        foreach (var x in analizadors){ analiza.Add(x); }
                        TablaDeSimbolos.Add(ambit[1], analiza);
                        analizadors.Clear();
                        ambito[0] = "";
                        ambito[1] = "";
                    }
                    switch (valor)
                    {
                        case "class":
                            declClase = true;
                            ambitoAbierto = true;
                            break;
                        case "interface":
                            declInterface = true;
                            ambitoAbierto = true;
                            break;
                        case "void":
                            declVoid = true;
                            break;
                        default:
                            break;
                    }
                }
                //Evaluar si comienza la llave
                if (valor == "{" || valor == "(")
                {
                    declClase = false;
                    if(ambitoAbierto && tipos.Contains(tempType))
                    {
                        declFuncion = true;
                        declararVariable = false;
                        funEnAmbito = true;
                    }
                    if (!String.IsNullOrEmpty(tempType) && !String.IsNullOrEmpty(tempIdent))
                    {
                        nuevaDecl = false;
                        if(hereda) { hereda = false; }
                        if (!funEnAmbito)
                        {
                            contAmbitos++;
                            ambito[0] = tempType;
                            ambito[1] = tempIdent;
                            if(tempType == "void" || tipos.Contains(tempType))
                            {
                                valFuncion[0] = tempType;
                                valFuncion[1] = tempIdent;
                                declFuncion = true;
                                declararVariable = false;
                            }
                            if (TablaDeSimbolos.Keys.Contains(ambito[1]))
                            {
                                listaErrores.Add("Ya existe un ambito con el nombre: " + ambito[1]);
                                errorAmbito = true;
                            }
                            else
                            {
                                TablaImprimir.Add("Creación de ámbito: " + ambito[1] + " de tipo: " + ambito[0]);
                            }
                        }
                        else
                        {
                            valFuncion[0] = tempType;
                            valFuncion[1] = tempIdent;
                        }
                        tempType = "";
                        tempIdent = "";
                    }
                }
                //Evaluar si termina el ambito
                if (valor == "}")
                {
                    if(!funEnAmbito)
                    {
                        ambitoAbierto = false;
                        if (!errorAmbito)
                        {
                            string[] ambit = new string[2];
                            ambit[0] = ambito[0];
                            ambit[1] = ambito[1];
                            List<AnalizadorSemanticoModel> analiza = new List<AnalizadorSemanticoModel>();
                            foreach (var x in analizadors) { analiza.Add(x); }
                            TablaDeSimbolos.Add(ambit[1], analiza);
                        }
                        analizadors.Clear();
                        ambito[0] = "";
                        ambito[1] = "";
                    }
                    else
                    {
                        funEnAmbito = false;
                    }
                }
                //Evaluar si la clase hereda
                if(valor == ":" && declClase)
                {
                    hereda = true;
                }
                //Evaluar parametros en funcion
                if(valor == "," || valor == ")")
                {
                    if(declVoid || declFuncion)
                    {
                        var listaTemp = new List<string>();
                        InsertarEnTabla(tempType, tempIdent, contAmbitos.ToString(), listaTemp);
                        parametros.Add(tempType);
                        tempType = "";
                        tempIdent = "";
                        declararVariable = false;
                        if(valor == ")")
                        {
                            InsertarEnTabla(valFuncion[0], valFuncion[1], contAmbitos.ToString(), parametros);
                            valFuncion[0] = "";
                            valFuncion[1] = "";
                            declVoid = false;
                            parametros.Clear();
                        }
                    }
                }
            }
            Dictionary<string, List<AnalizadorSemanticoModel>> Tablatemp = TablaDeSimbolos;
            var list = TablaImprimir;

        }

        public void InsertarEnTabla(string tipo, string ident, string ambitonum, List<string> parametros)
        {
            bool existe = false;
            bool esFuncion = false;
            AnalizadorSemanticoModel model = new AnalizadorSemanticoModel();
            //Si es una funcion
            if(parametros.Count > 0)
            {
                esFuncion = true;
                List<string> param = new List<string>();
                foreach (var item in parametros)
                {
                    param.Add(item);
                }
                model.parametros = param;
            }
            model.tipo = tipo;
            model.nombre = ident;
            model.ambito = ambitonum;
            if(tipo == "int" || tipo == "double")
            {
                model.valor = "0";
            }
            else if (tipo == "string")
            {
                model.valor = "\"\"";
            }
            else if(tipo == "bool")
            {
                model.valor = "false";
            }
            else
            {
                model.valor = "0";
            }
            foreach (var item in analizadors)
            {
                if(item.ambito == model.ambito && item.nombre == model.nombre)
                {
                    existe = true;
                }
            }
            if(!existe)
            {
                analizadors.Add(model);
                if(esFuncion)
                {
                    TablaImprimir.Add("Creacion de la funcion " + model.nombre + " de tipo: " + model.tipo + " en el ámbito: " + model.ambito);
                }
                else
                {
                    TablaImprimir.Add("Declaracion de variable " + model.nombre + " de tipo: " + model.tipo + " en el ámbito: " + model.ambito);
                }
            }
            else
            {
                listaErrores.Add("La variable '" + model.nombre + "' de tipo '" + model.tipo + "' ya fue declarada en el ámbito '" + model.ambito + "'");
            }
        }

        public AnalizadorSemanticoModel BuscarEnTabla(string[] ambito, string ident)
        {
            AnalizadorSemanticoModel objeto = new AnalizadorSemanticoModel();
            foreach (var item in analizadors)
            {
                if(item.nombre == ident)
                {
                    objeto = item;
                }
            }
            return objeto;
        }

        public void AsignarValor(List<string[]> lista)
        {
            var tipoRecibe = lista[0][0];
            var identRecibe = lista[0][1];
            var igual = lista[1][1];
            for (int i = 2; i < lista.Count; i++)
            {
                var tipoIdent = lista[i][0];
                var valorIdent = lista[i][1];
                if (tipoIdent == tipoRecibe)
                {
                    foreach (var item in analizadors)
                    {
                        if (item.nombre == identRecibe)
                        {
                            item.valor = valorIdent;
                            TablaImprimir.Add("Asignacion de valor: " + item.valor + " a la variable " + item.nombre + " del ambito: " + item.ambito);
                        }
                    }
                }
                else
                {
                    listaErrores.Add("El tipo de dato asignado no coincide con el de la variable: " + identRecibe);
                }
            }
        }

    }

}

