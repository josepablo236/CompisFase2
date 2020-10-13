using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectCompis2.Models;
namespace ProyectCompis2
{
    public partial class Form1 : Form
    {
        public string FilePath = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void BrowseFile_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) 
            {
                directorypath.Text = openFileDialog1.FileName;
                FilePath = openFileDialog1.FileName;
            }
        }

        private void Analizar_Click(object sender, EventArgs e)
        {
            Analizar analizador = new Analizar();
            List<TokensViewModel> listafinal = analizador.ReadText(FilePath);
            LlenarGrid(listafinal);
        }
        public void LlenarGrid(List<TokensViewModel> tokensViews)
        {
            DataTable tokens = new DataTable();
            tokens.Columns.Add("Token", typeof(String));
            tokens.Columns["Token"].ReadOnly = true;
            tokens.Columns.Add("Cadena", typeof(String));
            tokens.Columns["Cadena"].ReadOnly = true;
            tokens.Columns.Add("Linea", typeof(String));
            tokens.Columns["Linea"].ReadOnly = true;
            tokens.Columns.Add("Columnas", typeof(String));
            tokens.Columns["Columnas"].ReadOnly = true;

            foreach (var item in tokensViews)
            { 
                if(item.Token.Substring(5, item.Token.Length-5) != "Comentario")
                {
                    DataRow aa = tokens.NewRow();
                    aa["Token"] = item.Token;
                    aa["Cadena"] = item.Cadena;
                    aa["Linea"] = item.Linea;
                    aa["Columnas"] = item.Columnas;
                    tokens.Rows.Add(aa);
                }
            }
            dataGridView1.DataSource = tokens;
            dataGridView1.Refresh();
            dataGridView1.Update();

        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();
        }

        private void AnalizadorSintatico_Click(object sender, EventArgs e)
        {
            Analizar analizador = new Analizar();
            var aSintactico = new AnálisisSintactico();
            List<string[]> lista = new List<string[]>();
            var modeloTokens = analizador.ObtenerLista();
            lista = ObtenerTokens(modeloTokens);
            aSintactico.LeerTokens(lista);
        }

        public List<string[]> ObtenerTokens(List<TokensViewModel> tokensList)
        {
            List<string[]> tokens = new List<string[]>();
            foreach(var item in tokensList)
            {
                string[] temporal = new string[2];
                temporal[0] = item.Token.Substring(5, item.Token.Length -5);
                temporal[1] = item.Cadena;
                if((temporal[0] != "Error") && (temporal[0] != "Comentario"))
                {
                    tokens.Add(temporal);
                }
            }
            return tokens;
        }
    }
}
