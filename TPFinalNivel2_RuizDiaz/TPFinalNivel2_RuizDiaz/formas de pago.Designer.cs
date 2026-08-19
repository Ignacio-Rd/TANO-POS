namespace TPFinalNivel2_RuizDiaz
{
    partial class formas_de_pago
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
            this.dgv_metodopago = new System.Windows.Forms.DataGridView();
            this.btn_eliminarmetodopago = new System.Windows.Forms.Button();
            this.btnAgregarMetodopago = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_metodopago)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_metodopago
            // 
            this.dgv_metodopago.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_metodopago.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_metodopago.Location = new System.Drawing.Point(72, 51);
            this.dgv_metodopago.Name = "dgv_metodopago";
            this.dgv_metodopago.Size = new System.Drawing.Size(356, 238);
            this.dgv_metodopago.TabIndex = 0;
            // 
            // btn_eliminarmetodopago
            // 
            this.btn_eliminarmetodopago.Location = new System.Drawing.Point(198, 402);
            this.btn_eliminarmetodopago.Name = "btn_eliminarmetodopago";
            this.btn_eliminarmetodopago.Size = new System.Drawing.Size(75, 23);
            this.btn_eliminarmetodopago.TabIndex = 1;
            this.btn_eliminarmetodopago.Text = "Eliminar";
            this.btn_eliminarmetodopago.UseVisualStyleBackColor = true;
            this.btn_eliminarmetodopago.Click += new System.EventHandler(this.btn_eliminarmetodopago_Click);
            // 
            // btnAgregarMetodopago
            // 
            this.btnAgregarMetodopago.Location = new System.Drawing.Point(118, 326);
            this.btnAgregarMetodopago.Name = "btnAgregarMetodopago";
            this.btnAgregarMetodopago.Size = new System.Drawing.Size(75, 23);
            this.btnAgregarMetodopago.TabIndex = 2;
            this.btnAgregarMetodopago.Text = "Agregar";
            this.btnAgregarMetodopago.UseVisualStyleBackColor = true;
            this.btnAgregarMetodopago.Click += new System.EventHandler(this.btnAgregarMetodopago_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(316, 326);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Modificar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // formas_de_pago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 437);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnAgregarMetodopago);
            this.Controls.Add(this.btn_eliminarmetodopago);
            this.Controls.Add(this.dgv_metodopago);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "formas_de_pago";
            this.Text = "metodo_de_pago";
            this.Load += new System.EventHandler(this.formas_de_pago_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_metodopago)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_metodopago;
        private System.Windows.Forms.Button btn_eliminarmetodopago;
        private System.Windows.Forms.Button btnAgregarMetodopago;
        private System.Windows.Forms.Button button1;
    }
}