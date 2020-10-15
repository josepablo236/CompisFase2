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
            LlenarTabla(tokenList);
            Parse_Program();
            AnalizadorSintatico analizador = new AnalizadorSintatico();
            analizador.MostrarErrores(SyntaxError);
        }

        public void LlenarTabla(List<string[]> tokenList)
        {
            List<string> simbolos = new List<string> { ";","Identificador","const", "int", "double", "bool", "string",
            "[]", "(", ")", "void", ",", "class", "{", "}", ":", "interface", "if", "else", "while", "for", "return", "break",
            "Console", ".","WriteLine", "=", "this", "+", "*", "%", "-","<","<=" , "==", "&&" , "!", "New" , "intCon"
            ,"doubleCon", "boolCon", "stringCon", "null" , "$", "Program", "Decl" , "VariableDecl", "Variable", "ConstDecl"
            , "ConstType", "Type" , "Type_P", "Type_R", "FunctionsDecl" , "Formals", "Formals_P", "ClassDecl", "ClassDecl_P" , "ClassDecl_R" ,
              "ClassDecl_O", "ClassDecl_Q", "Field" , "InterfaceDecl", "InterfaceDecl_P" ,"Prototype" , "StmtBlock", "StmtBlock_P" , "StmtBlock_R"
            , "StmtBlock_O" , "Stmt", "Stmt_P" , "IfStmt" , "IfStmt_P", "WhileStmt" , "ForStmt" , "ReturnStmt", "BreakStmt", "PrintStmt", "PrintStmt_P", "Expr" , "LValue", "Constant"};

            string path = System.IO.Directory.GetCurrentDirectory() + "\\Tabla.txt";
            var numEstado = 0;
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
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
        public void Parse_Program()
        {
            //Código para parsear
        }
    }
}
