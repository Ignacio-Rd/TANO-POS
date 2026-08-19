namespace TPFinalNivel2_RuizDiaz
{
    partial class PAGAR_A_PROVEEDOR
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
            this.btnAgregarProveedor = new System.Windows.Forms.Button();
            this.cbxProveedores = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAgregarProveedor
            // 
            this.btnAgregarProveedor.BackColor = System.Drawing.Color.LightGray;
            this.btnAgregarProveedor.FlatAppearance.BorderSize = 0;
            this.btnAgregarProveedor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAgregarProveedor.Font = new System.Drawing.Font("Bebas Neue", 19F);
            this.btnAgregarProveedor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAgregarProveedor.Location = new System.Drawing.Point(209, 270);
            this.btnAgregarProveedor.Name = "btnAgregarProveedor";
            this.btnAgregarProveedor.Size = new System.Drawing.Size(243, 40);
            this.btnAgregarProveedor.TabIndex = 13;
            this.btnAgregarProveedor.Text = "AGREGAR NUEVO PROVEEDOR";
            this.btnAgregarProveedor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgregarProveedor.UseVisualStyleBackColor = false;
            this.btnAgregarProveedor.Click += new System.EventHandler(this.btnAgregarProveedor_Click);
            // 
            // cbxProveedores
            // 
            this.cbxProveedores.FormattingEnabled = true;
            this.cbxProveedores.Location = new System.Drawing.Point(154, 47);
            this.cbxProveedores.Name = "cbxProveedores";
            this.cbxProveedores.Size = new System.Drawing.Size(121, 21);
            this.cbxProveedores.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbxProveedores);
            this.groupBox1.Location = new System.Drawing.Point(177, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 129);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "REGISTRAR ENTRADA DE MERCADERIA";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(108, 100);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Aceptar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "PROVEEDOR:";
            // 
            // PAGAR_A_PROVEEDOR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 390);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnAgregarProveedor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "PAGAR_A_PROVEEDOR";
            this.Text = "PAGAR_A_PROVEEDOR";
            this.Load += new System.EventHandler(this.PAGAR_A_PROVEEDOR_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAgregarProveedor;
        private System.Windows.Forms.ComboBox cbxProveedores;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
    }
}