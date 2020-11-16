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
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(23, 127);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(1248, 491);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DataSourceChanged += new System.EventHandler(this.dataGridView1_DataSourceChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(900, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 17);
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
            this.label2.Location = new System.Drawing.Point(73, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "File Path:";
            // 
            // directorypath
            // 
            this.directorypath.AutoSize = true;
            this.directorypath.Location = new System.Drawing.Point(183, 27);
            this.directorypath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.directorypath.Name = "directorypath";
            this.directorypath.Size = new System.Drawing.Size(111, 17);
            this.directorypath.TabIndex = 3;
            this.directorypath.Text = "directory path....";
            // 
            // BrowseFile
            // 
            this.BrowseFile.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BrowseFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseFile.Location = new System.Drawing.Point(79, 49);
            this.BrowseFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BrowseFile.Name = "BrowseFile";
            this.BrowseFile.Size = new System.Drawing.Size(199, 57);
            this.BrowseFile.TabIndex = 4;
            this.BrowseFile.Text = "Browse File";
            this.BrowseFile.UseVisualStyleBackColor = false;
            this.BrowseFile.Click += new System.EventHandler(this.BrowseFile_Click);
            // 
            // Analizar
            // 
            this.Analizar.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Analizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Analizar.Location = new System.Drawing.Point(333, 49);
            this.Analizar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Analizar.Name = "Analizar";
            this.Analizar.Size = new System.Drawing.Size(199, 57);
            this.Analizar.TabIndex = 5;
            this.Analizar.Text = "Analizador Lexico";
            this.Analizar.UseVisualStyleBackColor = false;
            this.Analizar.Click += new System.EventHandler(this.Analizar_Click);
            // 
            // AnalizadorSintatico
            // 
            this.AnalizadorSintatico.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.AnalizadorSintatico.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnalizadorSintatico.Location = new System.Drawing.Point(579, 49);
            this.AnalizadorSintatico.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AnalizadorSintatico.Name = "AnalizadorSintatico";
            this.AnalizadorSintatico.Size = new System.Drawing.Size(263, 57);
            this.AnalizadorSintatico.TabIndex = 6;
            this.AnalizadorSintatico.Text = "Analizador Sintatico";
            this.AnalizadorSintatico.UseVisualStyleBackColor = false;
            this.AnalizadorSintatico.Click += new System.EventHandler(this.AnalizadorSintatico_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(875, 49);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(211, 57);
            this.button1.TabIndex = 7;
            this.button1.Text = "Analizador Semantico";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1287, 633);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.AnalizadorSintatico);
            this.Controls.Add(this.Analizar);
            this.Controls.Add(this.BrowseFile);
            this.Controls.Add(this.directorypath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        private System.Windows.Forms.Button button1;
    }
}

