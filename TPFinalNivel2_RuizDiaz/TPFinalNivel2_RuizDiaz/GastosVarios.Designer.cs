namespace TPFinalNivel2_RuizDiaz
{
    partial class GastosVarios
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
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.cbxCategoria = new System.Windows.Forms.ComboBox();
            this.txtMonto = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxMetodoPago = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnPagar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtObservaciones = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvHistorial
            // 
            this.dgvHistorial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistorial.Location = new System.Drawing.Point(60, 284);
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.Size = new System.Drawing.Size(537, 321);
            this.dgvHistorial.TabIndex = 0;
            // 
            // cbxCategoria
            // 
            this.cbxCategoria.FormattingEnabled = true;
            this.cbxCategoria.Location = new System.Drawing.Point(241, 36);
            this.cbxCategoria.Name = "cbxCategoria";
            this.cbxCategoria.Size = new System.Drawing.Size(121, 21);
            this.cbxCategoria.TabIndex = 1;
            // 
            // txtMonto
            // 
            this.txtMonto.Location = new System.Drawing.Point(241, 95);
            this.txtMonto.Name = "txtMonto";
            this.txtMonto.Size = new System.Drawing.Size(121, 20);
            this.txtMonto.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(134, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 16);
            this.label6.TabIndex = 29;
            this.label6.Text = "CATEGORÍA:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(166, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 16);
            this.label1.TabIndex = 30;
            this.label1.Text = "MONTO:";
            // 
            // cbxMetodoPago
            // 
            this.cbxMetodoPago.FormattingEnabled = true;
            this.cbxMetodoPago.Location = new System.Drawing.Point(241, 154);
            this.cbxMetodoPago.Name = "cbxMetodoPago";
            this.cbxMetodoPago.Size = new System.Drawing.Size(121, 21);
            this.cbxMetodoPago.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(85, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 16);
            this.label2.TabIndex = 32;
            this.label2.Text = "METODO DE PAGO:";
            // 
            // BtnPagar
            // 
            this.BtnPagar.Location = new System.Drawing.Point(258, 246);
            this.BtnPagar.Name = "BtnPagar";
            this.BtnPagar.Size = new System.Drawing.Size(87, 23);
            this.BtnPagar.TabIndex = 33;
            this.BtnPagar.Text = "PAGAR";
            this.BtnPagar.UseVisualStyleBackColor = true;
            this.BtnPagar.Click += new System.EventHandler(this.BtnPagar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(93, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 16);
            this.label3.TabIndex = 35;
            this.label3.Text = "OBSERVACIONES:";
            // 
            // TxtObservaciones
            // 
            this.TxtObservaciones.Location = new System.Drawing.Point(241, 209);
            this.TxtObservaciones.Name = "TxtObservaciones";
            this.TxtObservaciones.Size = new System.Drawing.Size(121, 20);
            this.TxtObservaciones.TabIndex = 34;
            // 
            // GastosVarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 648);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtObservaciones);
            this.Controls.Add(this.BtnPagar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxMetodoPago);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtMonto);
            this.Controls.Add(this.cbxCategoria);
            this.Controls.Add(this.dgvHistorial);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GastosVarios";
            this.Text = "GastosVarios";
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.ComboBox cbxCategoria;
        private System.Windows.Forms.TextBox txtMonto;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxMetodoPago;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnPagar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtObservaciones;
    }
}