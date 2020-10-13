namespace ProyectCompis2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.directorypath = new System.Windows.Forms.Label();
            this.BrowseFile = new System.Windows.Forms.Button();
            this.Analizar = new System.Windows.Forms.Button();
            this.AnalizadorSintatico = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(17, 103);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(936, 399);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DataSourceChanged += new System.EventHandler(this.dataGridView1_DataSourceChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(675, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Jose Orellana , Lisbeth Diaz";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(55, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "File Path:";
            // 
            // directorypath
            // 
            this.directorypath.AutoSize = true;
            this.directorypath.Location = new System.Drawing.Point(137, 22);
            this.directorypath.Name = "directorypath";
            this.directorypath.Size = new System.Drawing.Size(83, 13);
            this.directorypath.TabIndex = 3;
            this.directorypath.Text = "directory path....";
            // 
            // BrowseFile
            // 
            this.BrowseFile.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BrowseFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseFile.Location = new System.Drawing.Point(59, 40);
            this.BrowseFile.Name = "BrowseFile";
            this.BrowseFile.Size = new System.Drawing.Size(149, 46);
            this.BrowseFile.TabIndex = 4;
            this.BrowseFile.Text = "Browse File";
            this.BrowseFile.UseVisualStyleBackColor = false;
            this.BrowseFile.Click += new System.EventHandler(this.BrowseFile_Click);
            // 
            // Analizar
            // 
            this.Analizar.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Analizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Analizar.Location = new System.Drawing.Point(250, 40);
            this.Analizar.Name = "Analizar";
            this.Analizar.Size = new System.Drawing.Size(149, 46);
            this.Analizar.TabIndex = 5;
            this.Analizar.Text = "Analizador Lexico";
            this.Analizar.UseVisualStyleBackColor = false;
            this.Analizar.Click += new System.EventHandler(this.Analizar_Click);
            // 
            // AnalizadorSintatico
            // 
            this.AnalizadorSintatico.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.AnalizadorSintatico.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnalizadorSintatico.Location = new System.Drawing.Point(434, 40);
            this.AnalizadorSintatico.Name = "AnalizadorSintatico";
            this.AnalizadorSintatico.Size = new System.Drawing.Size(197, 46);
            this.AnalizadorSintatico.TabIndex = 6;
            this.AnalizadorSintatico.Text = "Analizador Sintatico";
            this.AnalizadorSintatico.UseVisualStyleBackColor = false;
            this.AnalizadorSintatico.Click += new System.EventHandler(this.AnalizadorSintatico_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(965, 514);
            this.Controls.Add(this.AnalizadorSintatico);
            this.Controls.Add(this.Analizar);
            this.Controls.Add(this.BrowseFile);
            this.Controls.Add(this.directorypath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Proyecto Compiladores";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label directorypath;
        private System.Windows.Forms.Button BrowseFile;
        private System.Windows.Forms.Button Analizar;
        private System.Windows.Forms.Button AnalizadorSintatico;
    }
}

