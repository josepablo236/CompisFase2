using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using ProyectCompis2.Models;

namespace ProyectCompis2 
{ 
    public class Analizar
    {
        public List<string> listaTokens = new List<string>();
        PalabrasReservadas palabrasReservadas = new PalabrasReservadas();
        public static List<TokensViewModel> tokensList = new List<TokensViewModel>();
        List<string> comments = new List<string>();
        List<string> strings = new List<string>();
        bool finString = false; bool primeravez = true;
        //ExpresionesRegulares
        List<string> reservadas = new List<string>
        { "void","int","double","bool","string","class","const","interface","null","this","for","while",
            "foreach","if","else","return","break","New","NewArray","Console","WriteLine"};
        string identificadores = "^[a-z | A-Z]+([_]|[0-9]|[a-z|A-Z])*$";
        List<string> booleanos = new List<string>
        { "true","false" };
        List<string> operadores = new List<string>
            { "+", "-", "*","/","%","<","<=",">",">=","=","==","!=","&&","||","!",";",",",
            ".","[","]","(",")","{","}","[]","()","{}"};


        // GET: AL
        public List<TokensViewModel> ReadText(string path)
        {
            
            int numberLine = 0;

            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var palabra = "";
                        var nuevapalabra = true;
                        var inicio = 0;
                        var fin = 0;
                        finString = false;
                        palabrasReservadas.stringon = false;
                        var line = reader.ReadLine();
                        var concatena = "";
                        numberLine++;
                        line = line + " ";
                        //Recorrer la linea
                        for (int i = 0; i < line.Length; i++)
                        {
                            //Validar si no es un espacio
                            if (line[i].ToString() != " ")
                            {
                                if (nuevapalabra == true)
                                {
                                    inicio = i;
                                }
                                nuevapalabra = false;
                                palabra += line[i].ToString();
                                #region Validar strings
                                if (palabra != "")
                                {
                                    //Si la bandera de string esta encendida		palabra	"*"	string

                                    if (palabrasReservadas.stringon)
                                    {
                                        //Valida si el ultimo caracter es comilla
                                        if (palabra.Substring(palabra.Length - 1, 1) == "\"")
                                        {
                                            finString = true;
                                            strings.Add(palabra);
                                            fin += 1;
                                            listaTokens.Add("TokenString " + "'" + string.Join(" ", strings.ToArray()) + "' found at line " + numberLine.ToString() + " col " +
                                                    (palabrasReservadas.inicioString++).ToString() + "-" + (fin++).ToString());
                                            var model = new TokensViewModel();
                                            model.Token = "TokenString";
                                            model.Cadena = string.Join(" ", strings.ToArray());
                                            model.Comentario = "Token reconocido";
                                            model.Linea = numberLine.ToString();
                                            model.Columnas = (palabrasReservadas.inicioString++).ToString() + "-" + (fin++).ToString();
                                            tokensList.Add(model);
                                            strings.Clear();
                                            nuevapalabra = true;
                                            palabra = "";
                                            palabrasReservadas.stringon = false;
                                        }
                                        //Cadena sin terminar
                                        else if (finString == false && i == line.Length - 2)
                                        {
                                            strings.Add(palabra);
                                            var error = string.Join(" ", strings.ToArray());
                                            //Si hay un comentario en el string sin cierre
                                            if (error.Contains("//"))
                                            {
                                                var vector = error.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
                                                fin = fin - vector[1].Length;
                                                error = vector[0];
                                            }
                                            listaTokens.Add("Error invalid string" + "'" + error + "' found at line " + numberLine.ToString() + " col " +
                                                                   (palabrasReservadas.inicioString++).ToString() + "-" + (fin++).ToString());
                                            var model = new TokensViewModel();
                                            model.Token = "Error";
                                            model.Cadena = error;
                                            model.Comentario = "String sin \" de cierre";
                                            model.Linea = numberLine.ToString();
                                            model.Columnas = (palabrasReservadas.inicioString++).ToString() + "-" + (fin++).ToString();
                                            tokensList.Add(model);
                                            strings.Clear();
                                            finString = true;
                                            nuevapalabra = true;
                                            palabra = "";
                                        }
                                    }
                                    //Encuentra comilla inicial
                                    else if (palabra.Substring(0, 1) == "\"" &&
                                        palabrasReservadas.comentarioon == false && palabrasReservadas.comentarioson == false)
                                    {
                                        palabrasReservadas.stringon = true;
                                        finString = false;
                                        palabrasReservadas.inicioString = i;
                                    }
                                    #region Encontró inicio
                                    if (palabra.Length >= 2)
                                    {
                                        if (palabra.Substring(0, 2) == "/*" && !palabrasReservadas.comentarioson)
                                        {
                                            palabrasReservadas.comentarioson = true;
                                            var comment = palabra;
                                            comments.Add(comment);
                                            palabrasReservadas.lineainiciacoment = numberLine;
                                            palabra = "";
                                        }
                                    }

                                    #endregion
                                }
                                #endregion
                            }
                            //Si hay un comentario abierto
                            else if ((i == line.Length - 1) && (palabrasReservadas.comentarioon == true))
                            {
                                comments.Add(palabra);
                                palabra = "";
                                palabrasReservadas.comentarioon = false;
                                var model = new TokensViewModel();
                                model.Token = "TokenComentario";
                                model.Cadena = string.Join(" ", comments.ToArray());
                                model.Comentario = "Token reconocido";
                                model.Linea = palabrasReservadas.lineainiciacoment.ToString();
                                model.Columnas = (palabrasReservadas.iniciaComentario++).ToString() + "-" + (fin++).ToString();
                                tokensList.Add(model); palabrasReservadas.lineainiciacoment = 0;
                                comments.Clear();
                                //return true;
                            }
                            //Si hay un espacio valida la palabra
                            else if (palabra != "")
                            {
                                #region Agregar cadena a token string
                                //Si la bandera esta encendida y no ha encontrado el fin
                                if (palabrasReservadas.stringon == true && finString == false)
                                {
                                    strings.Add(palabra);
                                    nuevapalabra = true;
                                    palabra = "";
                                }
                                #endregion
                                //Si no es un string
                                else if (palabrasReservadas.stringon == false)
                                {
                                    if (palabra.Length >= 2)
                                    {
                                        #region Encontró fin de comentario y nunca se abrió
                                        if (palabra.Substring(palabra.Length - 2, 2) == "*/" && !palabrasReservadas.comentarioson)
                                        {
                                            listaTokens.Add("Error end of comment unpaired" + "'" + palabra + "' found at line " + numberLine.ToString() + " col " +
                                                    (inicio++).ToString() + "-" + (fin++).ToString());
                                            var model = new TokensViewModel();
                                            model.Token = "Error";
                                            model.Cadena = palabra;
                                            model.Comentario = "Comentario nunca se abrio";
                                            model.Linea = numberLine.ToString();
                                            model.Columnas = (inicio++).ToString() + "-" + (fin++).ToString();
                                            palabra = ""; comments.Clear();
                                            tokensList.Add(model); palabrasReservadas.lineainiciacoment = 0;
                                        }
                                        #endregion
                                        #region Encontró fin de comentario y si se abrió
                                        if (palabra.Length >= 2)
                                        {
                                            if (palabra.Substring(palabra.Length - 2, 2) == "*/" && palabrasReservadas.comentarioson)
                                            {
                                                palabrasReservadas.comentarioson = false;
                                                comments.Add(palabra);
                                                var model = new TokensViewModel();
                                                model.Token = "TokenComentario";
                                                model.Cadena = string.Join(" ", comments.ToArray());
                                                model.Comentario = "Token reconocido";
                                                model.Linea = palabrasReservadas.lineainiciacoment.ToString();
                                                model.Columnas = (palabrasReservadas.iniciaComentario++).ToString() + "-" + (fin++).ToString();
                                                tokensList.Add(model); palabrasReservadas.lineainiciacoment = 0;
                                                comments.Clear();
                                                palabra = "";
                                            }
                                        }
                                        #endregion
                                    }
                                    if (palabra != "")
                                    {
                                        #region Agregar comentarios a lista
                                        if (palabrasReservadas.comentarioson || palabrasReservadas.comentarioon)
                                        {
                                            //Agregar palabra a lista comentarios varias lineas
                                            var comentarios = palabra;
                                            if (palabrasReservadas.comentarioson)
                                            {
                                                comments.Add(comentarios);
                                                palabra = "";
                                            }
                                            //Agregar palabra a lista comentario una linea
                                            else if (palabra != "")
                                            {
                                                comments.Add(comentarios);
                                                palabra = "";
                                                if (i == line.Length - 1)
                                                {
                                                    palabrasReservadas.comentarioon = false;
                                                    var model = new TokensViewModel();
                                                    model.Token = "TokenComentario";
                                                    model.Cadena = string.Join(" ", comments.ToArray());
                                                    model.Comentario = "Token reconocido";
                                                    model.Linea = palabrasReservadas.lineainiciacoment.ToString();
                                                    model.Columnas = (palabrasReservadas.iniciaComentario++).ToString() + "-" + (fin++).ToString();
                                                    tokensList.Add(model); palabrasReservadas.lineainiciacoment = 0;
                                                    comments.Clear();
                                                    //return true;
                                                }
                                            }

                                        }
                                        #endregion
                                        #region No hay comentarios abiertos
                                        else if (!palabrasReservadas.comentarioson && !palabrasReservadas.comentarioon)
                                        {
                                            string isMatch = "";
                                            if (palabra != "")
                                            {
                                                isMatch = PerteneceToken(numberLine, palabra);
                                                if (palabra.Length >= 2)
                                                {
                                                    if (palabra.Substring(0, 2) != "//" && palabra.Contains("//"))
                                                    {
                                                        isMatch = "error";
                                                    }

                                                }
                                                #region Dividir palabra
                                                if (isMatch == "error" && palabra != "")
                                                {
                                                    var caracteres = "";
                                                    nuevapalabra = true;
                                                    var nuevoinicio = 0;
                                                    var nuevofin = 0;
                                                    //Valida si la cadena sin el ultimo caracter es error
                                                    for (int j = 0; j < palabra.Length; j++)
                                                    {
                                                        if (nuevapalabra == true)
                                                        {
                                                            nuevoinicio = inicio + j;
                                                        }
                                                        //Si viene un string
                                                        if (palabra[j].ToString() == "\"" && palabrasReservadas.stringon == false &&
                                                            palabrasReservadas.comentarioon == false && palabrasReservadas.comentarioson == false)
                                                        {
                                                            palabrasReservadas.stringon = true;
                                                            finString = false;
                                                            palabrasReservadas.inicioString = inicio + j;
                                                            concatena = "\"";
                                                            var tokenTemporal = PerteneceToken(numberLine, caracteres);
                                                            nuevofin = inicio + (j - 1);
                                                            //Si no se encuentra el token
                                                            if (tokenTemporal == "error")
                                                            {
                                                                listaTokens.Add("Error invalid word" + "'" + caracteres + "' found at line " + numberLine.ToString() + " col " +
                                                            (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString());
                                                                var model = new TokensViewModel();
                                                                model.Token = "Error cadena no reconocida";
                                                                model.Cadena = caracteres;
                                                                model.Comentario = "Token reconocido";
                                                                model.Linea = numberLine.ToString();
                                                                model.Columnas = (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString();
                                                                tokensList.Add(model);
                                                                caracteres = "";
                                                            }
                                                            //Si encuentra el token
                                                            else
                                                            {
                                                                listaTokens.Add(tokenTemporal + "'" + caracteres + "' found at line " + numberLine.ToString() + " col " +
                                                                (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString());
                                                                var model = new TokensViewModel();
                                                                model.Token = tokenTemporal;
                                                                model.Cadena = caracteres;
                                                                model.Comentario = "Token reconocido";
                                                                model.Linea = numberLine.ToString();
                                                                model.Columnas = (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString();
                                                                tokensList.Add(model);
                                                                caracteres = "";
                                                                nuevapalabra = true;
                                                            }
                                                        }
                                                        //Si la bandera de string esta encendida
                                                        else if (palabrasReservadas.stringon)
                                                        {
                                                            //Valida si encuentra comilla de cierre
                                                            if (palabra[j].ToString() == "\"")
                                                            {
                                                                nuevofin = inicio + j;
                                                                finString = true;
                                                                concatena = concatena + "\"";
                                                                strings.Add(concatena);
                                                                concatena = "";
                                                                listaTokens.Add("TokenString " + "'" + string.Join(" ", strings.ToArray()) + "' found at line " + numberLine.ToString() + " col " +
                                                                        (palabrasReservadas.inicioString++).ToString() + "-" + (nuevofin++).ToString());
                                                                var model = new TokensViewModel();
                                                                model.Token = "TokenString";
                                                                model.Cadena = string.Join(" ", strings.ToArray());
                                                                model.Comentario = "Token reconocido";
                                                                model.Linea = numberLine.ToString();
                                                                model.Columnas = (palabrasReservadas.inicioString++).ToString() + "-" + (nuevofin++).ToString();
                                                                tokensList.Add(model);
                                                                strings.Clear();
                                                                palabrasReservadas.stringon = false;
                                                                nuevapalabra = true;
                                                            }
                                                            //Si no encuentra el fin concatena
                                                            else if (palabrasReservadas.stringon == true && finString == false)
                                                            {
                                                                concatena += palabra[j].ToString();
                                                            }
                                                        }
                                                        //Si no hay ningún string
                                                        else if (palabra[j].ToString() != "\"" && palabrasReservadas.stringon == false)
                                                        {
                                                            var tokenSigCaracter = PerteneceToken(numberLine, palabra[j].ToString());
                                                            //Si no es comentario
                                                            if (tokenSigCaracter != "")
                                                            {
                                                                caracteres += palabra[j].ToString();
                                                                var tokenActual = PerteneceToken(numberLine, caracteres);
                                                                if (tokenActual != "error")
                                                                {
                                                                    nuevapalabra = false;
                                                                }
                                                                else if (tokenActual == "error" && caracteres.Length >= 2)
                                                                {
                                                                    caracteres = caracteres.Substring(0, caracteres.Length - 1);
                                                                    var tokenAnterior = PerteneceToken(numberLine, caracteres);
                                                                    nuevofin = inicio + (j - 1);
                                                                    //Si no se encuentra el token
                                                                    if (tokenAnterior == "error")
                                                                    {
                                                                        listaTokens.Add("Error invalid word" + "'" + caracteres + "' found at line " + numberLine.ToString() + " col " +
                                                                    (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString());
                                                                        var model = new TokensViewModel();
                                                                        model.Token = "Error cadena no reconocida";
                                                                        model.Cadena = caracteres;
                                                                        model.Comentario = "Token reconocido";
                                                                        model.Linea = numberLine.ToString();
                                                                        model.Columnas = (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString();
                                                                        tokensList.Add(model);
                                                                        caracteres = "";
                                                                        tokenAnterior = "";
                                                                    }
                                                                    //Si encuentra el token
                                                                    else
                                                                    {
                                                                        if (tokenAnterior == "ErrorID")
                                                                        {
                                                                            var primeros31 = caracteres.Substring(0, 31);
                                                                            listaTokens.Add("TokenIdentificador" + "'" + primeros31 + "' found at line " + numberLine.ToString() + " col " +
                                                                        (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString());
                                                                            var model2 = new TokensViewModel();
                                                                            model2.Token = "TokenIdentificador";
                                                                            model2.Cadena = primeros31;
                                                                            model2.Comentario = "Token reconocido";
                                                                            model2.Linea = numberLine.ToString();
                                                                            model2.Columnas = (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString();
                                                                            tokensList.Add(model2);
                                                                        }
                                                                        listaTokens.Add(tokenAnterior + "'" + caracteres + "' found at line " + numberLine.ToString() + " col " +
                                                                        (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString());
                                                                        var model = new TokensViewModel();
                                                                        model.Token = tokenAnterior;
                                                                        model.Cadena = caracteres;
                                                                        model.Comentario = "Token reconocido";
                                                                        model.Linea = numberLine.ToString();
                                                                        model.Columnas = (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString();
                                                                        tokensList.Add(model);
                                                                        caracteres = "";
                                                                        tokenAnterior = "";
                                                                    }
                                                                    j -= 1;
                                                                    nuevapalabra = true;
                                                                }
                                                                else
                                                                {
                                                                    nuevofin = inicio + j;
                                                                    listaTokens.Add("Error invalid word" + "'" + caracteres + "' found at line " + numberLine.ToString() + " col " +
                                                                    (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString());
                                                                    var model = new TokensViewModel();
                                                                    model.Token = "Error cadena no reconocida";
                                                                    model.Cadena = caracteres;
                                                                    model.Comentario = "Token reconocido";
                                                                    model.Linea = numberLine.ToString();
                                                                    model.Columnas = (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString();
                                                                    tokensList.Add(model);
                                                                    caracteres = "";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                palabrasReservadas.comentarioon = true;
                                                            }
                                                        }
                                                    }
                                                    //Si hay un string sin cierre
                                                    if (concatena != "")
                                                    {
                                                        strings.Add(concatena);
                                                    }
                                                    //Si caracteres queda con algo
                                                    if (caracteres != "")
                                                    {
                                                        var tokenTemporal = PerteneceToken(numberLine, caracteres);
                                                        if (tokenTemporal != "TokenOperadores")
                                                        {
                                                            nuevofin = inicio + caracteres.Length;
                                                            if (tokenTemporal == "error")
                                                            {
                                                                listaTokens.Add("Error invalid word" + "'" + caracteres + "' found at line " + numberLine.ToString() + " col " +
                                                                    (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString());
                                                                var model = new TokensViewModel();
                                                                model.Token = "Error";
                                                                model.Cadena = caracteres;
                                                                model.Comentario = "Cadena no reconocida";
                                                                model.Linea = numberLine.ToString();
                                                                model.Columnas = (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString();
                                                                tokensList.Add(model);
                                                            }
                                                            else
                                                            {
                                                                listaTokens.Add(tokenTemporal + "'" + caracteres + "' found at line " + numberLine.ToString() + " col " +
                                                                        (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString());
                                                                var model = new TokensViewModel();
                                                                model.Token = tokenTemporal;
                                                                model.Cadena = caracteres;
                                                                model.Comentario = "Token reconocido";
                                                                model.Linea = numberLine.ToString();
                                                                model.Columnas = (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString();
                                                                tokensList.Add(model);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            for (int x = 0; x < caracteres.Length; x++)
                                                            {
                                                                if (caracteres[x].ToString() == "(")
                                                                {
                                                                    if(caracteres.Length > 1)
                                                                    {
                                                                        if (caracteres[x + 1].ToString() == ")")
                                                                        {
                                                                            var parentesis = "()";
                                                                            listaTokens.Add(tokenTemporal + "'" + parentesis + "' found at line " + numberLine.ToString() + " col " +
                                                                            (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString());
                                                                            var model = new TokensViewModel();
                                                                            model.Token = tokenTemporal;
                                                                            model.Cadena = parentesis;
                                                                            model.Comentario = "Token reconocido";
                                                                            model.Linea = numberLine.ToString();
                                                                            model.Columnas = (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString();
                                                                            tokensList.Add(model);
                                                                            x++;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        var parentesis = "(";
                                                                        listaTokens.Add(tokenTemporal + "'" + parentesis + "' found at line " + numberLine.ToString() + " col " +
                                                                        (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString());
                                                                        var model = new TokensViewModel();
                                                                        model.Token = tokenTemporal;
                                                                        model.Cadena = parentesis;
                                                                        model.Comentario = "Token reconocido";
                                                                        model.Linea = numberLine.ToString();
                                                                        model.Columnas = (nuevoinicio++).ToString() + "-" + (nuevofin++).ToString();
                                                                        tokensList.Add(model);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    listaTokens.Add(tokenTemporal + "'" + caracteres[x].ToString() + "' found at line " + numberLine.ToString() + " col " +
                                                                        (nuevoinicio++).ToString() + "-" + (nuevofin).ToString());
                                                                    var model = new TokensViewModel();
                                                                    model.Token = tokenTemporal;
                                                                    model.Cadena = caracteres[x].ToString();
                                                                    model.Comentario = "Token reconocido";
                                                                    model.Linea = numberLine.ToString();
                                                                    model.Columnas = (nuevoinicio++).ToString() + "-" + (nuevofin).ToString();
                                                                    tokensList.Add(model);
                                                                }
                                                            }
                                                        }
                                                        caracteres = "";
                                                    }
                                                }
                                                #endregion
                                                #region identificador muy largo
                                                else if (isMatch == "ErrorID")
                                                {
                                                    var primeros31 = palabra.Substring(0, 31);
                                                    listaTokens.Add("Error Id muy largo" + "'" + palabra + "' found at line " + numberLine.ToString() + " col " +
                                                                (inicio++).ToString() + "-" + (fin++).ToString());
                                                    var model = new TokensViewModel();
                                                    model.Token = "Error Id muy largo";
                                                    model.Cadena = palabra;
                                                    model.Comentario = "Token reconocido";
                                                    model.Linea = numberLine.ToString();
                                                    model.Columnas = (inicio++).ToString() + "-" + (fin++).ToString();
                                                    tokensList.Add(model);

                                                    listaTokens.Add("TokenIdentificador" + "'" + primeros31 + "' found at line " + numberLine.ToString() + " col " +
                                                                (inicio++).ToString() + "-" + (fin++).ToString());
                                                    var model2 = new TokensViewModel();
                                                    model2.Token = "TokenIdentificador";
                                                    model2.Cadena = primeros31;
                                                    model2.Comentario = "Token reconocido";
                                                    model2.Linea = numberLine.ToString();
                                                    model2.Columnas = (inicio++).ToString() + "-" + (fin++).ToString();
                                                    tokensList.Add(model2);
                                                }
                                                #endregion
                                                #region Palabra Normal
                                                else
                                                {
                                                    if (!palabrasReservadas.comentarioson && !palabrasReservadas.comentarioon && palabra != "")
                                                    {
                                                        listaTokens.Add(isMatch + "'" + palabra + "' found at line " + numberLine.ToString() + " col " +
                                                                (inicio++).ToString() + "-" + (fin++).ToString());
                                                        var model = new TokensViewModel();
                                                        model.Token = isMatch;
                                                        model.Cadena = palabra;
                                                        model.Comentario = "Token reconocido";
                                                        model.Linea = numberLine.ToString();
                                                        model.Columnas = (inicio++).ToString() + "-" + (fin++).ToString();
                                                        tokensList.Add(model);
                                                    }
                                                }
                                                #endregion
                                                nuevapalabra = true;
                                                palabra = "";
                                            }
                                            #region Token comentarios varias lineas
                                            if (palabra.Length >= 2)
                                            {
                                                if (Regex.IsMatch(palabra.Substring(palabra.Length - 2, 2), palabrasReservadas.comentariosfin))
                                                {
                                                    //Valida si viene cierre de comentario sin que se haya abierto
                                                    if (palabra.Substring(palabra.Length - 2, 2) == "*/" && !palabrasReservadas.comentarioson)
                                                    {
                                                        listaTokens.Add("TokenComentarios " + "'" + string.Join(" ", comments.ToArray()) + "' found at line " + numberLine.ToString());
                                                        var model = new TokensViewModel();
                                                        model.Token = "Error";
                                                        model.Cadena = palabra;
                                                        model.Comentario = "Comentario nunca se abrio";
                                                        model.Linea = palabrasReservadas.lineainiciacoment.ToString() + "-" + numberLine.ToString();
                                                        model.Columnas = "";
                                                        palabra = "   "; comments.Clear();
                                                        tokensList.Add(model); palabrasReservadas.lineainiciacoment = 0;
                                                    }
                                                    //Si se cierra el comentario
                                                    else if (palabra.Substring(palabra.Length - 2, 2) == "*/")
                                                    {
                                                        palabrasReservadas.comentarioson = false;
                                                        var model = new TokensViewModel();
                                                        model.Token = "TokenComentarios";
                                                        model.Cadena = comments.Last();
                                                        model.Comentario = "Token reconocido";
                                                        model.Linea = palabrasReservadas.lineainiciacoment.ToString() + "-" + numberLine.ToString();
                                                        model.Columnas = (inicio + 2).ToString() + "-" + (fin++).ToString();
                                                        tokensList.Add(model); palabrasReservadas.lineainiciacoment = 0;
                                                        comments.Clear();
                                                    }
                                                    //return true;
                                                }
                                            }
                                            #endregion

                                            nuevapalabra = true;
                                            palabra = "";
                                        }
                                        #endregion
                                        #region Token comentarios varias lineas
                                        if (Regex.IsMatch(palabra, palabrasReservadas.comentariosfin))
                                        {
                                            if (palabra.Length >= 2)
                                            {
                                                if (palabra.Substring(palabra.Length - 2, 2) == "*/" && !palabrasReservadas.comentarioson)
                                                {
                                                    listaTokens.Add("TokenComentarios " + "'" + string.Join(" ", comments.ToArray()) + "' found at line " + numberLine.ToString() + " col " +
                                                            (inicio++).ToString() + "-" + (fin++).ToString());
                                                    var model = new TokensViewModel();
                                                    model.Token = "Error";
                                                    model.Cadena = palabra;
                                                    model.Comentario = "Comentario nunca se abrio";
                                                    model.Linea = palabrasReservadas.lineainiciacoment.ToString() + "-" + numberLine.ToString();
                                                    model.Columnas = "";
                                                    palabra = ""; comments.Clear();
                                                    tokensList.Add(model); palabrasReservadas.lineainiciacoment = 0;
                                                }
                                                else if (palabra.Substring(palabra.Length - 2, 2) == "*/")
                                                {
                                                    palabrasReservadas.comentarioson = false;
                                                    var model = new TokensViewModel();
                                                    model.Token = "TokenComentarios";
                                                    model.Cadena = string.Join(" ", comments.ToArray());
                                                    model.Comentario = "Token reconocido";
                                                    model.Linea = palabrasReservadas.lineainiciacoment.ToString() + "-" + numberLine.ToString();
                                                    model.Columnas = (inicio + 2).ToString() + "-" + (fin++).ToString();
                                                    tokensList.Add(model); palabrasReservadas.lineainiciacoment = 0;
                                                    comments.Clear();
                                                }
                                            }
                                        }
                                        #endregion

                                    }
                                }
                            }
                            fin = i;
                        }
                    }
                    #region Si el comentario nunca se cierra
                    if (palabrasReservadas.comentarioson)
                    {
                        listaTokens.Add("Error Nunca se cerro  el comentario " + "'" + string.Join(" ", comments.ToArray()) + "' found at line " + numberLine.ToString());

                        var model = new TokensViewModel();
                        model.Token = "Error";
                        model.Cadena = string.Join(" ", comments.ToArray());
                        model.Comentario = "Comentario nunca se cerro";
                        model.Linea = palabrasReservadas.lineainiciacoment.ToString() + "-" + numberLine.ToString();
                        model.Columnas = "";
                        tokensList.Add(model); palabrasReservadas.lineainiciacoment = 0;
                    }
                    #endregion
                }
            }
            #region Escribir Archivo
            var currentPath = Path.GetFullPath(path);
            currentPath = Directory.GetParent(currentPath).FullName + "_ArchivoSalida.out";

            using (var stream = new FileStream(currentPath, FileMode.Create))
            {

                using (var reader = new StreamWriter(stream))
                {
                    foreach (var item in listaTokens)
                    {
                        reader.WriteLine(item);
                    }
                }
            }
            #endregion
            return tokensList;
        }
        public List<TokensViewModel> ObtenerLista()
        {
            return tokensList;
        }
        public string PerteneceToken(int numberLine, string palabra)
        {

            #region Validar si es reservada
            if (reservadas.Contains(palabra))
            {
                return "TokenReservadas";
            }
            #endregion

            #region Validar operadores
            else if (operadores.Contains(palabra))
            {
                return "TokenOperadores";
            }
            #endregion

            #region Validar si es boolenano
            else if (booleanos.Contains(palabra))
            {
                return "TokenBooleano";
            }
            #endregion

            #region Validar si es identificador
            if (Regex.IsMatch(palabra, identificadores) && !palabra.Contains("(") && !palabra.Contains("[") && !palabra.Contains("{"))
            {
                if (palabra.Length > 2)
                {
                    if (palabra.Substring(0, 2) == palabrasReservadas.comentario)
                    {
                        palabrasReservadas.comentarioon = true;
                        var comentarios = palabra;
                        comments.Add(comentarios);
                        palabrasReservadas.lineainiciacoment = numberLine;
                        return "";
                    }
                    else if (palabra.Substring(0, 2) == palabrasReservadas.comentarios && !palabrasReservadas.comentarioson)
                    {
                        palabrasReservadas.comentarioson = true;
                        palabrasReservadas.lineainiciacoment = numberLine;
                        var comentarios = palabra;
                        comments.Add(comentarios);
                        return "";
                    }
                    else if (Regex.IsMatch(palabra.Substring(palabra.Length - 2), palabrasReservadas.comentariosfin))
                    {
                        palabrasReservadas.comentarioson = false;
                        palabrasReservadas.comentarioon = false;

                        comments.Clear();
                        return "TokenComentarios";
                    }
                }

                if (!palabrasReservadas.comentarioson && !palabrasReservadas.comentarioon && !palabrasReservadas.stringon)
                {
                    var longitud = palabra.Length;
                    if (palabra.Length <= 31)
                    {
                        return "TokenIdentificador";
                    }
                    else
                    {
                        return "ErrorID";
                    }
                }
            }
            #endregion

            #region Validar si es  Double
            else if (Regex.IsMatch(palabra, palabrasReservadas.doubles))
            {
                return "TokenDouble";
            }
            #endregion

            #region Validar si es  Base10
            else if (Regex.IsMatch(palabra, palabrasReservadas.base10))
            {
                return "TokenBase10";
            }
            #endregion

            #region Validar si es Hexagesimal
            else if (Regex.IsMatch(palabra, palabrasReservadas.Hexadecimales))
            {
                return "TokenHexagesimal";
            }
            #endregion

            #region Validar si viene un comentario
            else if (Regex.IsMatch(palabra, palabrasReservadas.comentario))
            {
                palabrasReservadas.comentarioon = true;
                palabrasReservadas.lineainiciacoment = numberLine;
                var encontro = Regex.Match(palabra, palabrasReservadas.comentario);
                var comentarios = palabra;
                comments.Add(comentarios);
                return "";
            }
            #endregion

            #region Validar si viene un comentario de varias lineas
            else if (Regex.IsMatch(palabra, palabrasReservadas.comentarios) && !palabrasReservadas.comentarioson)
            {
                palabrasReservadas.comentarioson = true;
                palabrasReservadas.lineainiciacoment = numberLine;
                var encontro = Regex.Match(palabra, palabrasReservadas.comentario);

                var comentarios = palabra;
                comments.Add(comentarios);
                return "";
            }
            #endregion
            return "error";
        }

    }
}