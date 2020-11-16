using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectCompis2
{
    public partial class AnalizadorSemanticofrm : Form
    {
        public static DataTable error = new DataTable();
        public AnalizadorSemanticofrm()
        {
            InitializeComponent();
        }
        public void MostrarErrores(List<string> errores)
        {
            if (errores.Count > 0)
            {
                error.Columns.Add("Syntax error", typeof(String));
                error.Columns["Syntax error"].ReadOnly = true;

                foreach (var item in errores)
                {
                    DataRow aa = error.NewRow();
                    aa["Syntax error"] = item.ToString();
                    error.Rows.Add(aa);
                }
                dataGridErrores.DataSource = error;
                dataGridErrores.Refresh();
                dataGridErrores.Update();
                this.ShowDialog();
            }
            else
            {
                MessageBox.Show("Archivo exitoso!");
            }
        }

        private void dataGridErrores_DataMemberChanged(object sender, EventArgs e)
        {
            dataGridErrores.AutoResizeColumns();
        }
    }
}
