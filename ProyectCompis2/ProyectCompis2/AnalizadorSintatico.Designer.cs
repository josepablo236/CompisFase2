namespace ProyectCompis2
{
    partial class AnalizadorSintatico
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
            this.dataGridErrores = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridErrores)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridErrores
            // 
            this.dataGridErrores.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridErrores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridErrores.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridErrores.Location = new System.Drawing.Point(25, 13);
            this.dataGridErrores.Name = "dataGridErrores";
            this.dataGridErrores.Size = new System.Drawing.Size(428, 306);
            this.dataGridErrores.TabIndex = 0;
            this.dataGridErrores.DataMemberChanged += new System.EventHandler(this.dataGridErrores_DataMemberChanged);
            // 
            // AnalizadorSintatico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 342);
            this.Controls.Add(this.dataGridErrores);
            this.Name = "AnalizadorSintatico";
            this.Text = "AnalizadorSintatico";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridErrores)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridErrores;
    }
}