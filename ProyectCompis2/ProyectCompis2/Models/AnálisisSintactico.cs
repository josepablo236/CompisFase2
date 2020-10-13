using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Dictionary<string, string[]> simbolAction = new Dictionary<string, string[]>();
            //List<string> simbolos = new List<string> { ";","Identificador","const", "int", "double", "bool", "string",
            //"[]", "(", ")", "void", ",", "class", "{", "}", ":", "interface", "if", "else", "while", "for", "return", "break",
            //"Console", ".","WriteLine", "=", "this", "+", "*", "%", "-","<","<=" , "==", "&&" , "!", "New" , "intCon"
            //,"doubleCon", "boolCon", "stringCon", "null" , "$", "Program", "Decl" , "VariableDecl", "Variable", "ConstDecl"
            //, "ConstType", "Type" , "Type_P", "Type_R", "FunctionsDecl" , "Formals", "Formals_P", "ClassDecl", "ClassDecl_P" , "ClassDecl_R" ,
            //  "ClassDecl_O", "ClassDecl_Q", "Field" , "InterfaceDecl", "InterfaceDecl_P" ,"Prototype" , "StmtBlock", "StmtBlock_P" , "StmtBlock_R"
            //, "StmtBlock_O" , "Stmt", "Stmt_P" , "IfStmt" , "IfStmt_P", "WhileStmt" , "ForStmt" , "ReturnStmt", "BreakStmt", "PrintStmt", "PrintStmt_P", "Expr" , "LValue", "Constant"};
            Simbolos simbol = new Simbolos("Identificador", "s19", "", 0);
            simbol = new Simbolos("const", "s11", "", 0);
            simbol = new Simbolos("int", "s15", "", 0);
            simbol = new Simbolos("double", "s16", "", 0);
            simbol = new Simbolos("bool", "s17", "", 0);
            simbol = new Simbolos("string", "s18", "", 0);
            simbol = new Simbolos("void", "s10", "", 0);
            simbol = new Simbolos("class", "s12", "", 0);
            simbol = new Simbolos("interface", "s13", "", 0);
            simbol = new Simbolos("Program", "1", "", 0);
            simbol = new Simbolos("Decl", "2", "", 0);

            dicGeneral.Add(0, simbolAction);
            
        }
        public void Parse_Program()
        {
            //Código para parsear
        }
    }
}
