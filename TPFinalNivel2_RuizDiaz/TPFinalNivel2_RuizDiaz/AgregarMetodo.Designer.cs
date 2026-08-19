namespace TPFinalNivel2_RuizDiaz
{
    partial class BtnAgregar
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
            this.txtPorcentaje = new System.Windows.Forms.TextBox();
            this.txtMetodoDePago = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAgregarMetodo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPorcentaje
            // 
            this.txtPorcentaje.Location = new System.Drawing.Point(204, 131);
            this.txtPorcentaje.Name = "txtPorcentaje";
            this.txtPorcentaje.Size = new System.Drawing.Size(100, 20);
            this.txtPorcentaje.TabIndex = 5;
            // 
            // txtMetodoDePago
            // 
            this.txtMetodoDePago.Location = new System.Drawing.Point(204, 79);
            this.txtMetodoDePago.Name = "txtMetodoDePago";
            this.txtMetodoDePago.Size = new System.Drawing.Size(100, 20);
            this.txtMetodoDePago.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.AliceBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(86, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Porcentaje";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.AliceBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(68, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "MetodoPago";
            // 
            // btnAgregarMetodo
            // 
            this.btnAgregarMetodo.Location = new System.Drawing.Point(153, 224);
            this.btnAgregarMetodo.Name = "btnAgregarMetodo";
            this.btnAgregarMetodo.Size = new System.Drawing.Size(75, 23);
            this.btnAgregarMetodo.TabIndex = 9;
            this.btnAgregarMetodo.Text = "AGREGAR";
            this.btnAgregarMetodo.UseVisualStyleBackColor = true;
            this.btnAgregarMetodo.Click += new System.EventHandler(this.btnAgregarMetodo_Click);
            // 
            // BtnAgregar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 291);
            this.Controls.Add(this.btnAgregarMetodo);
            this.Controls.Add(this.txtPorcentaje);
            this.Controls.Add(this.txtMetodoDePago);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "BtnAgregar";
            this.Text = "AgregarMetodo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtPorcentaje;
        private System.Windows.Forms.TextBox txtMetodoDePago;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAgregarMetodo;
    }
}