namespace TPFinalNivel2_RuizDiaz
{
    partial class Configurar_Impresora
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboImpresoras = new System.Windows.Forms.ComboBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(170, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "SELECCIONE IMPRESORA DE TICKETS";
            // 
            // cboImpresoras
            // 
            this.cboImpresoras.FormattingEnabled = true;
            this.cboImpresoras.Location = new System.Drawing.Point(209, 99);
            this.cboImpresoras.Name = "cboImpresoras";
            this.cboImpresoras.Size = new System.Drawing.Size(121, 21);
            this.cboImpresoras.TabIndex = 1;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(188, 146);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(156, 23);
            this.btnGuardar.TabIndex = 2;
            this.btnGuardar.Text = "GUARDAR IMPRESORA";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // Configurar_Impresora
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 219);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.cboImpresoras);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Configurar_Impresora";
            this.Text = "Configurar_Impresora";
            this.Load += new System.EventHandler(this.Configurar_Impresora_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboImpresoras;
        private System.Windows.Forms.Button btnGuardar;
    }
}