namespace TPFinalNivel2_RuizDiaz
{
    partial class VerInformeEnPantalla
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
            this.ppcInforme = new System.Windows.Forms.PrintPreviewControl();
            this.btnSiguiente = new System.Windows.Forms.Button();
            this.btnAnterior = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ppcInforme
            // 
            this.ppcInforme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppcInforme.Location = new System.Drawing.Point(0, 0);
            this.ppcInforme.Name = "ppcInforme";
            this.ppcInforme.Size = new System.Drawing.Size(1370, 603);
            this.ppcInforme.TabIndex = 0;
            // 
            // btnSiguiente
            // 
            this.btnSiguiente.Location = new System.Drawing.Point(12, 12);
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(75, 23);
            this.btnSiguiente.TabIndex = 1;
            this.btnSiguiente.Text = "siguiente pagina";
            this.btnSiguiente.UseVisualStyleBackColor = true;
            this.btnSiguiente.Click += new System.EventHandler(this.btnSiguiente_Click);
            // 
            // btnAnterior
            // 
            this.btnAnterior.Location = new System.Drawing.Point(12, 41);
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new System.Drawing.Size(75, 23);
            this.btnAnterior.TabIndex = 2;
            this.btnAnterior.Text = "anterior";
            this.btnAnterior.UseVisualStyleBackColor = true;
            this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);
            // 
            // VerInformeEnPantalla
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 603);
            this.Controls.Add(this.btnAnterior);
            this.Controls.Add(this.btnSiguiente);
            this.Controls.Add(this.ppcInforme);
            this.Name = "VerInformeEnPantalla";
            this.Text = "VerInformeEnPantalla";
            this.Shown += new System.EventHandler(this.VerInformeEnPantalla_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PrintPreviewControl ppcInforme;
        private System.Windows.Forms.Button btnSiguiente;
        private System.Windows.Forms.Button btnAnterior;
    }
}