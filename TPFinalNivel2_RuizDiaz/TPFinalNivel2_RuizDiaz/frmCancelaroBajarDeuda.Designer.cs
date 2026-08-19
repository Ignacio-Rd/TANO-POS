namespace TPFinalNivel2_RuizDiaz
{
    partial class frmCancelaroBajarDeuda
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
            this.lblDeuda = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNombreDeudor = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMontoPago = new System.Windows.Forms.TextBox();
            this.btnCobrar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboMetodoPago = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblDeuda
            // 
            this.lblDeuda.AutoSize = true;
            this.lblDeuda.Location = new System.Drawing.Point(378, 50);
            this.lblDeuda.Name = "lblDeuda";
            this.lblDeuda.Size = new System.Drawing.Size(0, 13);
            this.lblDeuda.TabIndex = 0;
            this.lblDeuda.Click += new System.EventHandler(this.lblDeuda_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(255, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "DEBE:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblNombreDeudor
            // 
            this.lblNombreDeudor.AutoSize = true;
            this.lblNombreDeudor.Location = new System.Drawing.Point(170, 50);
            this.lblNombreDeudor.Name = "lblNombreDeudor";
            this.lblNombreDeudor.Size = new System.Drawing.Size(0, 13);
            this.lblNombreDeudor.TabIndex = 2;
            this.lblNombreDeudor.Click += new System.EventHandler(this.lblNombreDeudor_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "CANCELA:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // txtMontoPago
            // 
            this.txtMontoPago.Location = new System.Drawing.Point(91, 114);
            this.txtMontoPago.Name = "txtMontoPago";
            this.txtMontoPago.Size = new System.Drawing.Size(100, 20);
            this.txtMontoPago.TabIndex = 4;
            this.txtMontoPago.TextChanged += new System.EventHandler(this.txtMontoPago_TextChanged);
            // 
            // btnCobrar
            // 
            this.btnCobrar.Location = new System.Drawing.Point(238, 198);
            this.btnCobrar.Name = "btnCobrar";
            this.btnCobrar.Size = new System.Drawing.Size(75, 23);
            this.btnCobrar.TabIndex = 5;
            this.btnCobrar.Text = "ACEPTAR";
            this.btnCobrar.UseVisualStyleBackColor = true;
            this.btnCobrar.Click += new System.EventHandler(this.BtnAceptar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(206, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "$";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(292, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "METODO DE PAGO:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // cboMetodoPago
            // 
            this.cboMetodoPago.FormattingEnabled = true;
            this.cboMetodoPago.Location = new System.Drawing.Point(406, 113);
            this.cboMetodoPago.Name = "cboMetodoPago";
            this.cboMetodoPago.Size = new System.Drawing.Size(121, 21);
            this.cboMetodoPago.TabIndex = 8;
            this.cboMetodoPago.SelectedIndexChanged += new System.EventHandler(this.cboMetodoPago_SelectedIndexChanged);
            // 
            // frmCancelaroBajarDeuda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 259);
            this.Controls.Add(this.cboMetodoPago);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCobrar);
            this.Controls.Add(this.txtMontoPago);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblNombreDeudor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDeuda);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmCancelaroBajarDeuda";
            this.Text = "frmCancelaroBajarDeuda";
            this.Load += new System.EventHandler(this.frmCancelaroBajarDeuda_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDeuda;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNombreDeudor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMontoPago;
        private System.Windows.Forms.Button btnCobrar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboMetodoPago;
    }
}