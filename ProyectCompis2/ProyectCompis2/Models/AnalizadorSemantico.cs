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
        List<string> operadores = new List<string> { "+", "*", "%" };
        Analizar analizar = new Analizar();
        AnalizadorSemanticoModel SemanticoModel = new AnalizadorSemanticoModel();
        List<AnalizadorSemanticoModel> analizadors = new List<AnalizadorSemanticoModel>();

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
            List<string[]> listaOperacion = new List<string[]>();  //Lista para guardar la operacion
            List<string[]> listaOpeClase = new List<string[]>();
            List<string> parametros = new List<string>();
            string identRecibeTemp = "";
            string[] ambito = new string[2];
            string[] valFuncion = new string[2];
            int contAmbitos = 0;
            string tempType = "";
            string tempIdent = "";
            string clase = "";
            string nombreFuncion = "";
            bool asigEnClase = false;
            bool funcionClase = false;
            bool declClase = false;
            bool errorAmbito = false;
            bool hereda = false;
            bool ambitoAbierto = false;
            bool llamarFuncion = false;
            bool funEnAmbito = false;
            bool declInterface = false;
            bool declVoid = false;
            bool declFuncion = false;
            bool declararVariable = false;
            bool asignarVariable = false;
            bool declararConstante = false;
            bool nuevaDecl = false;
            bool identSolo = false;
            bool variosIdent = false;
            bool comparaciones = false;
            bool error = false;
            //Recorrer la lista de tokens
            foreach (var item in tokenList)
            {
                string tipo = item[0];
                var valor = item[1];
                //Evaluar si viene const
                if (valor == "const")
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
                        if (hereda)
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
                    else if (declararVariable)
                    {
                        tempIdent = valor;
                    }
                    //Declarar void
                    else if (declVoid)
                    {
                        tempIdent = valor;
                    }
                    //Asignacion u operacion
                    else if (asignarVariable)
                    {
                        //Si no hay idents que no existan
                        if(!error)
                        {
                            var identObject = BuscarEnTabla(ambito, valor);
                            if (identObject != null)
                            {
                                //Si es un ident de operacion guarda el valor
                                var ope = new string[2];
                                ope[0] = identObject.tipo;
                                ope[1] = identObject.valor;
                                if(asigEnClase)
                                {
                                    listaOpeClase.Add(ope);
                                }
                                else
                                {
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
                    //Si hay un punto antes
                    else if (variosIdent)
                    {
                        identRecibeTemp += valor;
                    }
                    //Si es una llamada a funcion
                    else if(llamarFuncion)
                    {
                        //Si no hay idents que no existan
                        if (!error)
                        {
                            var identObject = BuscarEnTabla(ambito, valor);
                            if (identObject != null)
                            {
                                parametros.Add(identObject.tipo);
                            }
                            else
                            {
                                listaErrores.Add("La variable '" + valor + " no existe en el ámbito '" + contAmbitos + "'");
                                error = true;
                            }
                        }
                    }
                    //Si es una comparacion
                    else if(comparaciones)
                    {
                        var identObject = BuscarEnTabla(ambito, valor);
                        if (identObject == null)
                        {
                            listaErrores.Add("La variable '" + valor + " no existe en el ámbito '" + contAmbitos + "'");
                        }
                    }
                    //El identificador viene solo lo guardo en una temporal
                    else
                    {
                        identSolo = true;
                        identRecibeTemp = valor;
                    }
                }
                //Evaluar una asignacion
                if (valor == "=")
                {
                    if(identSolo)
                    {
                        //Si solo es un ident el que recibe el valor
                        if (!variosIdent)
                        {
                            var identObject = BuscarEnTabla(ambito, identRecibeTemp);
                            if (identObject != null)
                            {
                                var recibe = new string[2];
                                recibe[0] = identObject.tipo;
                                recibe[1] = identObject.nombre;
                                listaOperacion.Add(recibe);
                            }
                            else
                            {
                                listaErrores.Add("La variable '" + identRecibeTemp + " no existe en el ámbito '" + contAmbitos + "'");
                                error = true;
                            }
                        }
                        //Si es ident . ident
                        else
                        {
                            var separar = identRecibeTemp.Split('.');
                            //Si existe esa clase
                            if(TablaDeSimbolos.ContainsKey(separar[0]))
                            {
                                var ambi = TablaDeSimbolos.FirstOrDefault(x => x.Key == separar[0]);
                                var exis = false;
                                foreach (var a in ambi.Value)
                                {
                                    if(a.nombre == separar[1])
                                    {
                                        exis = true;
                                        var recibe = new string[2];
                                        recibe[0] = a.tipo;
                                        recibe[1] = a.nombre;
                                        listaOpeClase.Add(recibe);
                                        clase = separar[0];
                                        asigEnClase = true;
                                    }
                                }
                                if(exis == false)
                                {
                                    listaErrores.Add("La variable: " + separar[1] + " no existe en el ambito " + separar[0]);
                                }
                                exis = false;
                            }
                            else
                            {
                                listaErrores.Add("El ambito: " + separar[0] + " no existe.");
                            }
                            variosIdent = false;
                        }
                        var ope = new string[2];
                        ope[0] = "asigna";
                        ope[1] = "=";
                        if(asigEnClase)
                        {
                            listaOpeClase.Add(ope);
                        }
                        else
                        {
                            listaOperacion.Add(ope);
                        }
                        asignarVariable = true;
                        identSolo = false;
                    }
                }
                //Evaluar valores numericos o strings
                if (tiposValores.Contains(tipo))
                {
                    var ope = new string[2];
                    if (tipo == "Base10")
                    {
                        tipo = "int";
                    }
                    if (tipo == "Double")
                    {
                        tipo = "double";
                    }
                    if (tipo == "String")
                    {
                        tipo = "string";
                    }
                    if (tipo == "Booleano")
                    {
                        tipo = "bool";
                    }
                    if (llamarFuncion)
                    {
                        parametros.Add(tipo);
                    }
                    else
                    {
                        ope[0] = tipo;
                        ope[1] = valor;
                        if (asigEnClase)
                        {
                            listaOpeClase.Add(ope);
                        }
                        else
                        {
                            listaOperacion.Add(ope);
                        }
                    }
                }
                //Cuando termina una declaracion u operacion
                if (valor == ";")
                {
                    //Evaluar que no esten vacios
                    if (declararVariable)
                    {
                        InsertarEnTabla(tempType, tempIdent, contAmbitos.ToString(), parametros);
                        tempType = "";
                        tempIdent = "";
                        declararVariable = false;
                    }
                    else if (asignarVariable)
                    {
                        //Si no hay errores asigna la operacion
                        if(!error)
                        {
                            if(asigEnClase)
                            {
                                AsignarValorEnClase(clase, listaOpeClase);
                                asigEnClase = false;
                            }
                            else
                            {
                                AsignarValor(listaOperacion);
                            }
                        }
                        error = false;
                        asignarVariable = false;
                        listaOperacion.Clear();
                        listaOpeClase.Clear();
                    }
                    else if (declInterface)
                    {
                        declInterface = false;
                        funEnAmbito = false;
                    }
                    //Si ya se termino de llamar a la funcion
                    else if (llamarFuncion)
                    {
                        if (!error)
                        {
                            //Si solo se llama a una funcion
                            if(!funcionClase)
                            {
                                var amb = TablaDeSimbolos.FirstOrDefault(x => x.Key == identRecibeTemp);
                                var function = amb.Value.FirstOrDefault(y => y.nombre == identRecibeTemp);
                                TablaImprimir.Add("Llamada a la función: " + identRecibeTemp);
                                if (function.parametros.Count() == parametros.Count())
                                {
                                    for (int i = 0; i < parametros.Count; i++)
                                    {
                                        if (parametros[i] == function.parametros[i])
                                        {
                                            TablaImprimir.Add("Parámetro válido de tipo: " + parametros[i]);
                                        }
                                        else
                                        {
                                            listaErrores.Add("Parámetro inválido, se esperaba tipo de dato: " + function.parametros[i] + " en la funcion: " + identRecibeTemp);
                                        }
                                    }
                                }
                                else
                                {
                                    listaErrores.Add("Se esperaban " + function.parametros.Count().ToString() + " parametros en la funcion: " + identRecibeTemp);
                                }
                            }
                            //Si se llama a una funcion de una clase
                            else
                            {
                                var amb = TablaDeSimbolos.FirstOrDefault(x => x.Key == clase);
                                var function = amb.Value.FirstOrDefault(y => y.nombre == nombreFuncion);
                                TablaImprimir.Add("Llamada a la función: " + nombreFuncion + " del ambito " + clase);
                                if (function.parametros.Count() == parametros.Count())
                                {
                                    for (int i = 0; i < parametros.Count; i++)
                                    {
                                        if (parametros[i] == function.parametros[i])
                                        {
                                            TablaImprimir.Add("Parámetro válido de tipo: " + parametros[i]);
                                        }
                                        else
                                        {
                                            listaErrores.Add("Parámetro inválido, se esperaba tipo de dato: " + function.parametros[i] + " en la funcion: " + identRecibeTemp);
                                        }
                                    }
                                }
                                else
                                {
                                    listaErrores.Add("Se esperaban " + function.parametros.Count().ToString() + " parametros en la funcion: " + identRecibeTemp);
                                }
                                nombreFuncion = "";
                                clase = "";
                                funcionClase = false;
                            }
                        }
                        parametros.Clear();
                        llamarFuncion = false;
                        error = false;
                    }
                }
                //Evaluar declaraciones de clases e interfaces 
                if (valor == "class" || valor == "interface" || valor == "void")
                {
                    if (ambitoAbierto)
                    {
                        if (valor == "void")
                        {
                            funEnAmbito = true;
                        }
                    }
                    nuevaDecl = true;
                    tempType = valor;
                    //Meter ambito cero al diccionario
                    if (contAmbitos == 0)
                    {
                        ambito[0] = "cero";
                        ambito[1] = "principal";
                        string[] ambit = new string[2];
                        ambit[0] = "cero";
                        ambit[1] = "principal";
                        List<AnalizadorSemanticoModel> analiza = new List<AnalizadorSemanticoModel>();
                        foreach (var x in analizadors) { analiza.Add(x); }
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
                    //Si es parte de una operacion
                    if(valor == "(" && asignarVariable)
                    {
                        var ope = new string[2];
                        ope[0] = "ope";
                        ope[1] = valor;
                        if(asigEnClase)
                        {
                            listaOpeClase.Add(ope);
                        }
                        else
                        {
                            listaOperacion.Add(ope);
                        }
                    }
                    else
                    {
                        //Si no es llamada a funcion
                        if (!identSolo)
                        {
                            declClase = false;
                            if (ambitoAbierto && tipos.Contains(tempType))
                            {
                                declFuncion = true;
                                declararVariable = false;
                                funEnAmbito = true;
                            }
                            if (!String.IsNullOrEmpty(tempType) && !String.IsNullOrEmpty(tempIdent))
                            {
                                nuevaDecl = false;
                                if (hereda) { hereda = false; }
                                if (!funEnAmbito)
                                {
                                    contAmbitos++;
                                    ambito[0] = tempType;
                                    ambito[1] = tempIdent;
                                    if (tempType == "void" || tipos.Contains(tempType))
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
                                        TablaImprimir.Add("Creación de ámbito: " + ambito[1] + " de tipo: " + ambito[0] + " ambito no. " + contAmbitos);
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
                        //Si es llamada a funcion 
                        else
                        {
                            //Si solo es un ident
                            if (!variosIdent)
                            {
                                //Si existe el ambito
                                if (TablaDeSimbolos.ContainsKey(identRecibeTemp))
                                {
                                    llamarFuncion = true;
                                }
                                else
                                {
                                    listaErrores.Add("El ambito: " + identRecibeTemp + " no existe en el contexto");
                                    error = true;
                                }
                            }
                            //ident.ident
                            else
                            {
                                var separar = identRecibeTemp.Split('.');
                                //Si existe esa clase
                                if (TablaDeSimbolos.ContainsKey(separar[0]))
                                {
                                    var ambi = TablaDeSimbolos.FirstOrDefault(x => x.Key == separar[0]);
                                    var exis = false;
                                    foreach (var a in ambi.Value)
                                    {
                                        if (a.nombre == separar[1])
                                        {
                                            exis = true;
                                            clase = separar[0];
                                            nombreFuncion = separar[1];
                                            funcionClase = true;
                                            llamarFuncion = true;
                                        }
                                    }
                                    if (exis == false)
                                    {
                                        listaErrores.Add("La funcion: " + separar[1] + " no existe en el ambito " + separar[0]);
                                    }
                                    exis = false;
                                }
                                else
                                {
                                    listaErrores.Add("El ambito: " + separar[0] + " no existe.");
                                }
                                variosIdent = false;
                            }
                            identSolo = false;
                        }
                    }
                }
                //Evaluar si termina el ambito
                if (valor == "}")
                {
                    if (!funEnAmbito)
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
                if (valor == ":" && declClase)
                {
                    hereda = true;
                }
                //Evaluar parametros en funcion
                if (valor == "," || valor == ")")
                {
                    //Si es parte de una operacion
                    if (valor == ")" && asignarVariable)
                    {
                        var ope = new string[2];
                        ope[0] = "ope";
                        ope[1] = valor;
                        if (asigEnClase)
                        {
                            listaOpeClase.Add(ope);
                        }
                        else
                        {
                            listaOperacion.Add(ope);
                        }
                    }
                    else
                    {
                        if (declVoid || declFuncion)
                        {
                            var listaTemp = new List<string>();
                            InsertarEnTabla(tempType, tempIdent, contAmbitos.ToString(), listaTemp);
                            parametros.Add(tempType);
                            tempType = "";
                            tempIdent = "";
                            declararVariable = false;
                            if (valor == ")")
                            {
                                InsertarEnTabla(valFuncion[0], valFuncion[1], contAmbitos.ToString(), parametros);
                                valFuncion[0] = "";
                                valFuncion[1] = "";
                                declVoid = false;
                                declFuncion = false;
                                parametros.Clear();
                            }
                        }
                        comparaciones = false;
                    }
                }
                //Evaluar si vienen varios ident
                if(valor == ".")
                {
                    identRecibeTemp += valor;
                    variosIdent = true;
                }
                //Evaluar si vienen signos operadores
                if(operadores.Contains(valor))
                {
                    var ope = new string[2];
                    ope[0] = "ope";
                    ope[1] = valor;
                    if (asigEnClase)
                    {
                        listaOpeClase.Add(ope);
                    }
                    else
                    {
                        listaOperacion.Add(ope);
                    }
                }
                //Evaluar si vienen comparaciones
                if(valor == "if" || valor == "while" || valor == "Console")
                {
                    comparaciones = true;
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
            if (parametros.Count > 0)
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
            if (tipo == "int" || tipo == "double")
            {
                model.valor = "0";
            }
            else if (tipo == "string")
            {
                model.valor = "\"\"";
            }
            else if (tipo == "bool")
            {
                model.valor = "false";
            }
            else
            {
                model.valor = "0";
            }
            foreach (var item in analizadors)
            {
                if (item.ambito == model.ambito && item.nombre == model.nombre)
                {
                    existe = true;
                }
            }
            if (!existe)
            {
                analizadors.Add(model);
                if (esFuncion)
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
                if (item.nombre == ident)
                {
                    objeto = item;
                }
            }
            return objeto;
        }

        public void AsignarValor(List<string[]> listaOpe)
        {
            //El primero de la lista es el que recibe el valor
            var error = false;
            var operacion = "";
            var concatena = "";
            var tipoRecibe = listaOpe[0][0];
            var identRecibe = listaOpe[0][1];
            //El segundo de la lista es el signo =
            var igual = listaOpe[1][1];
            var lista = new List<string[]>();
            for (int i = 2; i < listaOpe.Count; i++)
            {
                lista.Add(listaOpe[i]);
                operacion += listaOpe[i][1];
            }
            //Si es una operacion
            if(lista.Count > 1)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    var tipo = lista[i][0];
                    var valor = lista[i][1];
                    if (tipo != "ope")
                    {
                        //valida que el tipo sea igual al tipo del que recibe
                        if (tipoRecibe == "string")
                        {
                            if (tipo != tipoRecibe)
                            {
                                error = true;
                            }
                            else
                            {
                                concatena += valor;
                            }
                        }
                        //Si es numero
                        else
                        {
                            if(tipo == "string")
                            {
                                error = true;
                            }
                        }
                    }
                }
                //Si son del mismo tipo de dato
                if(!error)
                {
                    //Si son strings
                    if(tipoRecibe == "string")
                    {
                        foreach (var item in analizadors)
                        {
                            if (item.nombre == identRecibe)
                            {
                                item.valor = concatena;
                                TablaImprimir.Add("Asignacion de valor: " + item.valor + " a la variable " + item.nombre + " del ambito: " + item.ambito);
                            }
                        }
                    }
                    //Si son operaciones
                    else
                    {
                        var valorAsignado = Operar(lista);
                        foreach (var item in analizadors)
                        {
                            if (item.nombre == identRecibe)
                            {
                                item.valor = valorAsignado;
                                item.operacion = operacion;
                                TablaImprimir.Add("Asignacion de valor: " + item.valor + " a la variable " + item.nombre + " del ambito: " + item.ambito + " operacion: " + item.operacion);
                            }
                        }
                    }
                }
                else
                {
                    listaErrores.Add("Los tipos de dato no coinciden con el tipo de dato: " + tipoRecibe + " de la variable: " + identRecibe);
                }
            }
            //Si solo es una asignacion simple
            else
            {
                var tipo = lista[0][0];
                var valor = lista[0][1];
                //valida que el tipo sea igual al tipo del que recibe
                if (tipo == tipoRecibe)
                {
                    foreach (var item in analizadors)
                    {
                        if (item.nombre == identRecibe)
                        {
                            item.valor = valor;
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

        public void AsignarValorEnClase(string clase, List<string[]> listaOpe)
        {
            //El primero de la lista es el que recibe el valor
            var error = false;
            var operacion = "";
            var concatena = "";
            var tipoRecibe = listaOpe[0][0];
            var identRecibe = listaOpe[0][1];
            //El segundo de la lista es el signo =
            var igual = listaOpe[1][1];
            var lista = new List<string[]>();
            for (int i = 2; i < listaOpe.Count; i++)
            {
                lista.Add(listaOpe[i]);
                operacion += listaOpe[i][1];
            }
            //Si es una operacion
            if (lista.Count > 1)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    var tipo = lista[i][0];
                    var valor = lista[i][1];
                    if (tipo != "ope")
                    {
                        //valida que el tipo sea igual al tipo del que recibe
                        if (tipoRecibe == "string")
                        {
                            if (tipo != tipoRecibe)
                            {
                                error = true;
                            }
                            else
                            {
                                concatena += valor;
                            }
                        }
                        //Si es numero
                        else
                        {
                            if (tipo == "string")
                            {
                                error = true;
                            }
                        }
                    }
                }
                //Si son del mismo tipo de dato
                if (!error)
                {
                    //Si son strings
                    if (tipoRecibe == "string")
                    {
                        var ambit = TablaDeSimbolos.FirstOrDefault(x => x.Key == clase);
                        foreach (var item in ambit.Value)
                        {
                            if (item.nombre == identRecibe)
                            {
                                item.valor = concatena;
                                TablaImprimir.Add("Asignacion de valor: " + item.valor + " a la variable " + item.nombre + " del ambito: " + item.ambito);
                            }
                        }
                    }
                    //Si son operaciones
                    else
                    {
                        var valorAsignado = Operar(lista);
                        var ambit = TablaDeSimbolos.FirstOrDefault(x => x.Key == clase);
                        foreach (var item in ambit.Value)
                        {
                            if (item.nombre == identRecibe)
                            {
                                item.valor = valorAsignado;
                                item.operacion = operacion;
                                TablaImprimir.Add("Asignacion de valor: " + item.valor + " a la variable " + item.nombre + " del ambito: " + item.ambito + " operacion: " + item.operacion);
                            }
                        }
                    }
                }
                else
                {
                    listaErrores.Add("Los tipos de dato no coinciden con el tipo de dato: " + tipoRecibe + " de la variable: " + identRecibe);
                }
            }
            //Si solo es una asignacion simple
            else
            {
                var tipo = lista[0][0];
                var valor = lista[0][1];
                //valida que el tipo sea igual al tipo del que recibe
                if (tipo == tipoRecibe)
                {
                    var ambit = TablaDeSimbolos.FirstOrDefault(x => x.Key == clase);
                    foreach (var item in ambit.Value)
                    {
                        if (item.nombre == identRecibe)
                        {
                            item.valor = valor;
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
        public string Operar(List<string[]> listaOperar)
        {
            var pila = new Stack<string>();
            string izquierdo, derecho;
            var pilaPolaca = new Stack<string>();
            var operadores = new List<string>() { "+", "*", "%", "(" };
            var listaPostFija = new List<string>();
            var lista = new List<string>();
            //lista solo con valores
            foreach (var item in listaOperar)
            {
                lista.Add(item[1]);
            }
            //Recorrer la lista
            foreach (var item in lista)
            {
                //Si es operador lo agrega a la pila
                if(operadores.Contains(item))
                {
                    pila.Push(item);
                }
                else if(item == ")")
                {
                    for (int i = 0; i < pila.Count(); i++)
                    {
                        if (pila.Peek() != "(")
                        {
                            listaPostFija.Add(pila.Peek());
                            pila.Pop();
                        }
                    }
                    if (pila.Peek() == "(")
                    {
                        pila.Pop();
                    }
                }
                else
                {
                    listaPostFija.Add(item);
                }
            }
            if(pila.Count() > 0)
            {
                for (int i = 0; i < pila.Count(); i++)
                {
                    if (pila.Peek() != "(")
                    {
                        listaPostFija.Add(pila.Peek());
                        pila.Pop();
                    }
                }
            }
            //Calculadora polaca inversa
            foreach (var item in listaPostFija)
            {
                if(operadores.Contains(item))
                {
                    derecho = pilaPolaca.Pop();
                    izquierdo = pilaPolaca.Pop();
                    double resultado = Operando(izquierdo, item, derecho);
                    pilaPolaca.Push(resultado.ToString());
                }
                else
                {
                    pilaPolaca.Push(item);
                }
            }
            var devuelve = pilaPolaca.Pop();
            return devuelve;
        }
        public double Operando(string izq, string ope, string der)
        {
            if(izq.Contains('.'))
            {
                var v = izq.Split('.');
                var parteEntera = v[0];
                var parteDecimal = v[1];
                izq = parteEntera + "," + parteDecimal;
            }
            if (der.Contains('.'))
            {
                var v = der.Split('.');
                var parteEntera = v[0];
                var parteDecimal = v[1];
                der = parteEntera + "," + parteDecimal;
            }
            double a = Convert.ToDouble(izq);
            double b = Convert.ToDouble(der);
            switch (ope)
            {
                case "+":
                    return a + b;
                case "*":
                    return a * b;
                case "%":
                    return a % b;
                default:
                    return -1;
            }
        }
    }
}