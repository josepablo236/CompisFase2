using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectCompis2.Models
{
    public class PalabrasReservadas
    {
        //string, int, double, float
        public List<string> tiposVariables = new List<string>();
        // void, 
        public List<string> metodos = new List<string>();
        //(Llaves o parejas) {},[], /**/
        public List<string> parejasreservadas = new List<string>();
        //Void, int, double, bool, string, class, const, interface, null, this, for, while, foreach, if, else, return, break, New, New Array, Console, WriteLine
        public List<string> palabrasreservadas = new List<string>();

        public string comentario = "//";
        public string comentarios = "[/*]";
        public string llavesstrings = "[\"]";
        public string comentariosfin = "[*/]";
        public int lineainiciacoment = 0;
        public int iniciaComentario = 0;
        public int inicioString = 0;
        public bool comentarioon = false;
        public bool stringon = false;
        public bool comentarioson = false;
        public string Hexadecimales = "^([0][xX])([0-9]|[abcdefABCDEF])*$";
        public string base10 = "^[0-9]+$";

        public string doubles = "^[0-9]+[.][0-9]*([eE]([+-])?)?[0-9]*$";
        //public string doubles = "[0-9]+[.](([e|E][+][0-9]+)|[0-9])*$";
        public List<string> constantes = new List<string>();

    }
}
