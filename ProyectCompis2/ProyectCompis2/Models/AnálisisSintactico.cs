using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProyectCompis2.Models;
using System.Threading.Tasks;

namespace ProyectCompis2
{
    public class AnálisisSintactico
    {
        public bool banderaError = false;
        bool fin = false;
        bool error = false;
        bool recupera = true;
        int CantidadPasos = 1;
        List<string> SyntaxError = new List<string>();
        List<string[]> tokenList = new List<string[]>();
        List<string[]> listaconlineas = new List<string[]>();
        Dictionary<int, Dictionary<string, string[]>> dicGeneral = new Dictionary<int, Dictionary<string, string[]>>();
        Dictionary<string, string[]> listaGramatica = new Dictionary<string, string[]>();
        List<string> recuperarValores = new List<string>
        {
            "int", "string", "double", "bool", "const", "class", "interface", "void"
        };
        List<string> recuperarValores2 = new List<string>
        {
            "if", "while", "for", "break", "return", "Console"
        };
        public void LeerTokens(List<string[]> listaTokens)
        {
            tokenList = listaTokens;
            LlenarTabla();
            ListaGramatica();
            Parse_Program(tokenList);
            AnalizadorSintatico analizador = new AnalizadorSintatico();
            int temp = CantidadPasos;
            analizador.MostrarErrores(SyntaxError);

        }

        public void LeerTokensLineas(List<string[]> lista)
        {
            listaconlineas = lista;

        }
        //public void LeerTokensLINEAS(List<string[]> listaTokens)
        //[0] valor, [1] linea, [2] columnas

        public void LlenarTabla()
        {
            List<string> simbolos = new List<string> { ";", "Identificador", "const", "int", "double", "bool", "string",
                "[]", "(", ")", "void", ",", "class", "{", "}", ":", "interface", "if", "else", "while", "for",
                "return", "break", "Console", ".", "WriteLine", "=", "==", "&&", "<", "<=", "+", "*", "%", "-", "!",
                "this", "New", "Base10", "Double", "Booleano", "String", "null", "$",
                "Program", "Decl", "VariableDecl", "Variable", "ConstDecl", "ConstType", "Type", "Type_P", "Type_R",
                "FunctionsDecl", "Formals", "Formals_P", "ClassDecl", "ClassDecl_P", "ClassDecl_R", "ClassDecl_O",
                "ClassDecl_Q", "Field", "InterfaceDecl", "InterfaceDecl_P", "Prototype", "StmtBlock", "StmtBlock_P",
                "StmtBlock_R", "StmtBlock_O", "Stmt", "Stmt_P", "IfStmt", "IfStmt_P", "WhileStmt", "ForStmt",
                "ReturnStmt", "BreakStmt", "PrintStmt", "PrintStmt_P", "Expr", "E1", "E1P", "E2", "E2P", "E3",
                "E3P", "E4", "E4P" };

            //List<string> simbolos = new List<string> { "Identificador", "=", "==", "&&", "<", "<=", "+", "*", "%",
            //    "-", "!", ".", "(", ")", "this", "New", "Base10", "Double", "Booleano", "String", "null", "$",
            //    "Expr" ,"E1",  "E1P", "E2",  "E2P", "E3",  "E3P", "E4",  "E4P" };

            string path = System.IO.Directory.GetCurrentDirectory() + "\\TablaSLR.txt";
            var numEstado = 0;
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        Dictionary<string, string[]> simbolAction = new Dictionary<string, string[]>();
                        var estado = reader.ReadLine();
                        var separar = estado.Split(';');
                        Simbolos simbol = new Simbolos();
                        var contSimbol = 0;
                        foreach (var item in simbolos)
                        {
                            if (separar[contSimbol] != "n")
                            {
                                string[] valores = new string[2];
                                if (separar[contSimbol].Contains("/"))
                                {
                                    var dosCaminos = separar[contSimbol].Split('/');
                                    valores[0] = dosCaminos[0];
                                    valores[1] = dosCaminos[1];
                                }
                                else
                                {
                                    valores[0] = separar[contSimbol];
                                    valores[1] = null;
                                }
                                simbolAction.Add(item, valores);
                            }
                            contSimbol++;
                        }
                        dicGeneral.Add(numEstado, simbolAction);
                        numEstado++;
                    }
                }
            }
        }
        public void ListaGramatica()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\\listaGramatica.txt";
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var linea = reader.ReadLine();
                        var separar = linea.Split('/');
                        var vector = new string[3];
                        vector[0] = separar[1];
                        vector[1] = separar[2];
                        vector[2] = separar[3];
                        listaGramatica.Add(separar[0], vector);
                    }
                }
            }
        }
        public void Parse_Program(List<string[]> Entrada)
        {
            //Código para parsear
            var dolar = new string[2];
            dolar[0] = "Dolar";
            dolar[1] = "$";
            Entrada.Add(dolar);
            var pila = new Stack<int>();
            pila.Push(0);
            var SimbolosLeidos = new Stack<string>();
            FuncionParseo(pila, SimbolosLeidos, Entrada, 0, 0);

        }
        public void FuncionParseo(Stack<int> pila, Stack<string> simbolosLeidos, List<string[]> Entrada, int num, int dosCaminosR)
        {
            if(!fin)
            {
                var simEvaluar = "";
                //Evaluar segun el tipo de grupo
                if (dicGeneral[pila.Peek()].ContainsKey(Entrada[num][0]))
                {
                    simEvaluar = Entrada[num][0];
                }
                //Evaluar según el valor 
                else if (dicGeneral[pila.Peek()].ContainsKey(Entrada[num][1]))
                {
                    simEvaluar = Entrada[num][1];
                }
                //Si no lo contiene es error
                else
                {
                    if (dosCaminosR != 0)
                    {
                        pila.Pop();
                        simbolosLeidos.Pop();
                        num--;
                        CantidadPasos--;
                        //Reducir
                        var produccion = listaGramatica.FirstOrDefault(x => x.Key == dosCaminosR.ToString());
                        var vector = produccion.Value;
                        var noterminal = vector[0];
                        var cantSimbolos = Convert.ToInt32(vector[1]);
                        var vecSimbolos = vector[2].Split('|');
                        //Desapilar segun la cantidad de simbolos
                        if (cantSimbolos > 0)
                        {
                            for (int i = 0; i < cantSimbolos; i++)
                            {
                                if (pila.Count > 1)
                                {
                                    pila.Pop();
                                }
                            }
                            for (int i = vecSimbolos.Length - 1; i >= 0; i--)
                            {
                                if (vecSimbolos[i] == simbolosLeidos.Peek())
                                {
                                    simbolosLeidos.Pop();
                                }
                            }
                        }
                        simbolosLeidos.Push(noterminal);
                        CantidadPasos++;
                        dosCaminosR = 0;
                        FuncionReducir(pila, simbolosLeidos, Entrada, num);
                    }
                    else
                    {
                        error = true;
                    }
                }
                //Si no es error
                if (!error)
                {
                    if(!fin)
                    {
                        var sim = dicGeneral[pila.Peek()].FirstOrDefault(x => x.Key == simEvaluar);
                        //Evalua si solo hay una accion
                        if (sim.Value[1] == null)
                        {
                            var comienzo = sim.Value[0].Substring(0, 1);
                            //Desplazamiento
                            if (comienzo == "s")
                            {
                                var valor = sim.Value[0].Substring(1, sim.Value[0].Length - 1);
                                pila.Push(Convert.ToInt32(valor));
                                simbolosLeidos.Push(simEvaluar);
                                num++;
                                CantidadPasos++;
                                FuncionParseo(pila, simbolosLeidos, Entrada, num, 0);

                            }
                            //Reduccion
                            else if (comienzo == "r")
                            {
                                var valor = sim.Value[0].Substring(1, sim.Value[0].Length - 1);
                                var produccion = listaGramatica.FirstOrDefault(x => x.Key == valor);
                                var vector = produccion.Value;
                                var noterminal = vector[0];
                                var cantSimbolos = Convert.ToInt32(vector[1]);
                                var vecSimbolos = vector[2].Split('|');
                                //Desapilar segun la cantidad de simbolos
                                if (cantSimbolos > 0)
                                {
                                    for (int i = 0; i < cantSimbolos; i++)
                                    {
                                        if(pila.Count > 1)
                                        {
                                            pila.Pop();
                                        }
                                    }
                                    for (int i = vecSimbolos.Length - 1; i >= 0; i--)
                                    {
                                        if (vecSimbolos[i] == simbolosLeidos.Peek())
                                        {
                                            simbolosLeidos.Pop();
                                        }
                                    }
                                }
                                simbolosLeidos.Push(noterminal);
                                CantidadPasos++;
                                FuncionReducir(pila, simbolosLeidos, Entrada, num);

                            }
                            else if (sim.Value[0] == "acc")
                            {
                                fin = true;
                                FuncionParseo(pila, simbolosLeidos, Entrada, num, 0);
                            }
                            //Si solo es un numero
                            else
                            {
                                CantidadPasos++;
                                pila.Push(Convert.ToInt32(sim.Value[0]));
                                FuncionParseo(pila, simbolosLeidos, Entrada, num, 0);
                            }
                        }
                        //Si tiene dos caminos
                        else
                        {
                            //codigo para ver que camino tomar
                            var valorDesplazar = sim.Value[0].Substring(1, sim.Value[0].Length - 1);
                            var numReduccion = sim.Value[1].Substring(1, sim.Value[1].Length - 1);
                            //Desplazar
                            pila.Push(Convert.ToInt32(valorDesplazar));
                            simbolosLeidos.Push(simEvaluar);
                            num++;
                            CantidadPasos++;
                            FuncionParseo(pila, simbolosLeidos, Entrada, num, Convert.ToInt32(numReduccion));
                        }
                    }
                }
                //Error
                else
                {
                    if(recupera == true)
                    {
                        SyntaxError.Add("Error de sintaxis: " + Entrada[num][0] + " " + Entrada[num][1] + "Linea: " + listaconlineas[num][1] + " Columna: " + listaconlineas[num][2]);
                        recupera = false;
                    }
                    if (recuperarValores.Contains(Entrada[num][1]))
                    {
                        pila.Clear();
                        pila.Push(0);
                        simbolosLeidos.Clear();
                        error = false;
                        recupera = true;
                        FuncionParseo(pila, simbolosLeidos, Entrada, num, 0);
                    }
                    else
                    {
                        if(num < Entrada.Count()-2)
                        {
                            num++;
                            error = false;
                            FuncionParseo(pila, simbolosLeidos, Entrada, num, 0);
                        }
                    }
                }
            }
        }

        public void FuncionReducir(Stack<int> pila, Stack<string> simbolosLeidos, List<string[]> Entrada, int num)
        {
            if (!fin)
            {
                if (dicGeneral[pila.Peek()].ContainsKey(simbolosLeidos.Peek()))
                {
                    var sim = dicGeneral[pila.Peek()].FirstOrDefault(x => x.Key == simbolosLeidos.Peek());
                    if (sim.Value[1] == null)
                    {
                        if (sim.Value[0].Substring(0, 1) != "r" && sim.Value[0].Substring(0, 1) != "s")
                        {
                            var irA = sim.Value[0];
                            pila.Push(Convert.ToInt32(sim.Value[0]));
                            CantidadPasos++;
                            FuncionParseo(pila, simbolosLeidos, Entrada, num, 0);
                        }
                        else
                        {
                            //Codigo de error
                            SyntaxError.Add("Valores de reduccion invalidos: " + Entrada[num][0] + " " + Entrada[num][1] + " Linea: " + listaconlineas[num][1] + " Columna: " + listaconlineas[num][2]);
                            if (recuperarValores.Contains(Entrada[num][1]))
                            {
                                pila.Clear();
                                pila.Push(0);
                                simbolosLeidos.Clear();
                                error = false;
                                FuncionParseo(pila, simbolosLeidos, Entrada, num, 0);
                            }
                            else
                            {
                                num++;
                                error = false;
                                FuncionParseo(pila, simbolosLeidos, Entrada, num, 0);
                            }
                        }
                    }
                }
                else
                {
                    //Codigo de error
                    SyntaxError.Add("Valores de reduccion invalidos: " + Entrada[num][0] + " " + Entrada[num][1] + " Linea: " + listaconlineas[num][1] + " Columna: " + listaconlineas[num][2]);
                    if (recuperarValores.Contains(Entrada[num][1]))
                    {
                        pila.Clear();
                        pila.Push(0);
                        simbolosLeidos.Clear();
                        error = false;
                        FuncionParseo(pila, simbolosLeidos, Entrada, num, 0);
                    }
                    else
                    {
                        num++;
                        error = false;
                        FuncionParseo(pila, simbolosLeidos, Entrada, num, 0);
                    }
                }
            }
        }

    }
}