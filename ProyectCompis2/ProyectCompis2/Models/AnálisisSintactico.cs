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
        List<string> SyntaxError = new List<string>();
        int posicionTemporal = 0;
        bool fin = false;
        bool masDeUnCamino = false;
        bool backtrack = false;
        List<string[]> tokenList = new List<string[]>();
        int lookahead = 0;
        int contador = 0;
        Dictionary<int,Dictionary<string,string[]>> dicGeneral = new Dictionary<int, Dictionary<string, string[]>>();
        public void LeerTokens(List<string[]> listaTokens)
        {
            tokenList = listaTokens;
            LlenarTabla();
            Parse_Program(tokenList);
            AnalizadorSintatico analizador = new AnalizadorSintatico();
            analizador.MostrarErrores(SyntaxError);
        }

        public void LlenarTabla()
        {
            List<string> simbolos = new List<string> { ";","Identificador","const", "int", "double", "bool", "string",
            "[]", "(", ")", "void", ",", "class", "{", "}", ":", "interface", "if", "else", "while", "for", "return", "break",
            "Console", ".","WriteLine", "=", "this", "+", "*", "%", "-","<","<=" , "==", "&&" , "!", "New" , "Base10"
            ,"Double", "Booleano", "String", "null" , "$", "Program", "Decl" , "VariableDecl", "Variable", "ConstDecl"
            , "ConstType", "Type" , "Type_P", "Type_R", "FunctionsDecl" , "Formals", "Formals_P", "ClassDecl", "ClassDecl_P" , "ClassDecl_R" ,
              "ClassDecl_O", "ClassDecl_Q", "Field" , "InterfaceDecl", "InterfaceDecl_P" ,"Prototype" , "StmtBlock", "StmtBlock_P" , "StmtBlock_R"
            , "StmtBlock_O" , "Stmt", "Stmt_P" , "IfStmt" , "IfStmt_P", "WhileStmt" , "ForStmt" , "ReturnStmt", "BreakStmt", "PrintStmt", "PrintStmt_P", "Expr" ,
                "E1",  "E1P", "E2",  "E2P", "E3",  "E3P", "E4",  "E4P" };

            List<string> prueba = new List<string> { "ident", "=", "==", "&&", "<", "<=", "+", "*", "%",
                "-", "!", ".", "(", ")", "this", "New", "intConstant", "doubleConstant", "boolConstant", "stringConstant", "null", "$", 
                "Expr" ,"E1",  "E1P", "E2",  "E2P", "E3",  "E3P", "E4",  "E4P" };

            string path = System.IO.Directory.GetCurrentDirectory() + "\\datosTabla.txt";
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
                            if(separar[contSimbol] != "n")
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
            FuncionParseo(pila, SimbolosLeidos, Entrada, 0);

        }
        public void FuncionParseo(Stack<int> pila, Stack<string> simbolosLeidos, List<string[]> Entrada, int num)
        {
            if (dicGeneral[pila.Peek()].ContainsKey(Entrada[num][0]))
            {
                var sim = dicGeneral[pila.Peek()].FirstOrDefault(x => x.Key == Entrada[num][0]);
                if (sim.Value[1] != null)
                {
                    var comienzo = sim.Value[0].Substring(0, 1);
                    if (comienzo == "s")
                    {
                        var valor = sim.Value[0].Substring(1, sim.Value[0].Length - 1);
                        pila.Push(Convert.ToInt32(valor));
                        simbolosLeidos.Push(Entrada[num][0]);
                        num++;
                        FuncionParseo(pila, simbolosLeidos, Entrada, num);
                    }
                    else if (comienzo == "r")
                    {
                        //Codigo de reduccion
                    }
                    else
                    {
                        pila.Push(Convert.ToInt32(sim.Value[0]));
                        num++;
                        FuncionParseo(pila, simbolosLeidos, Entrada, num);
                    }
                }
                else
                {
                    //codigo para ver que camino tomar
                }
            }
            else if (dicGeneral[pila.Peek()].ContainsKey(Entrada[num][1]))
            {
                dicGeneral[pila.Peek()].FirstOrDefault(x => x.Key == Entrada[num][1]);
            }
            else
            {
                //Error
            }
        }
    }
}
