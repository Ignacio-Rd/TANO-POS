namespace TPFinalNivel2_RuizDiaz
{
    partial class FIADO
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
            this.LblDeudor = new System.Windows.Forms.Label();
            this.BtnFiar = new System.Windows.Forms.Button();
            this.lblDeuda = new System.Windows.Forms.Label();
            this.btnNuevoDeudor = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSumaDeuda = new System.Windows.Forms.TextBox();
            this.cboNombreDeudor = new System.Windows.Forms.ComboBox();
            this.lblEstadoDeuda = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LblDeudor
            // 
            this.LblDeudor.AutoSize = true;
            this.LblDeudor.Location = new System.Drawing.Point(114, 87);
            this.LblDeudor.Name = "LblDeudor";
            this.LblDeudor.Size = new System.Drawing.Size(104, 13);
            this.LblDeudor.TabIndex = 0;
            this.LblDeudor.Text = "NOMBRE DEUDOR";
            // 
            // BtnFiar
            // 
            this.BtnFiar.Location = new System.Drawing.Point(117, 304);
            this.BtnFiar.Name = "BtnFiar";
            this.BtnFiar.Size = new System.Drawing.Size(75, 23);
            this.BtnFiar.TabIndex = 2;
            this.BtnFiar.Text = "FIAR";
            this.BtnFiar.UseVisualStyleBackColor = true;
            this.BtnFiar.Click += new System.EventHandler(this.BtnFiar_Click);
            // 
            // lblDeuda
            // 
            this.lblDeuda.AutoSize = true;
            this.lblDeuda.Location = new System.Drawing.Point(114, 125);
            this.lblDeuda.Name = "lblDeuda";
            this.lblDeuda.Size = new System.Drawing.Size(45, 13);
            this.lblDeuda.TabIndex = 3;
            this.lblDeuda.Text = "DEUDA";
            // 
            // btnNuevoDeudor
            // 
            this.btnNuevoDeudor.Location = new System.Drawing.Point(285, 304);
            this.btnNuevoDeudor.Name = "btnNuevoDeudor";
            this.btnNuevoDeudor.Size = new System.Drawing.Size(180, 23);
            this.btnNuevoDeudor.TabIndex = 4;
            this.btnNuevoDeudor.Text = "AGREGAR NUEVO DEUDOR";
            this.btnNuevoDeudor.UseVisualStyleBackColor = true;
            this.btnNuevoDeudor.Click += new System.EventHandler(this.btnNuevoDeudor_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(114, 208);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "SE LE SUMA";
            // 
            // txtSumaDeuda
            // 
            this.txtSumaDeuda.Location = new System.Drawing.Point(252, 201);
            this.txtSumaDeuda.Name = "txtSumaDeuda";
            this.txtSumaDeuda.Size = new System.Drawing.Size(100, 20);
            this.txtSumaDeuda.TabIndex = 6;
            // 
            // cboNombreDeudor
            // 
            this.cboNombreDeudor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboNombreDeudor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNombreDeudor.FormattingEnabled = true;
            this.cboNombreDeudor.Location = new System.Drawing.Point(252, 79);
            this.cboNombreDeudor.Name = "cboNombreDeudor";
            this.cboNombreDeudor.Size = new System.Drawing.Size(121, 21);
            this.cboNombreDeudor.TabIndex = 7;
            this.cboNombreDeudor.SelectedIndexChanged += new System.EventHandler(this.cboNombreDeudor_SelectedIndexChanged);
            // 
            // lblEstadoDeuda
            // 
            this.lblEstadoDeuda.AutoSize = true;
            this.lblEstadoDeuda.Location = new System.Drawing.Point(114, 166);
            this.lblEstadoDeuda.Name = "lblEstadoDeuda";
            this.lblEstadoDeuda.Size = new System.Drawing.Size(126, 13);
            this.lblEstadoDeuda.TabIndex = 8;
            this.lblEstadoDeuda.Text = "ESTADO DE LA DEUDA";
            // 
            // FIADO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 368);
            this.Controls.Add(this.lblEstadoDeuda);
            this.Controls.Add(this.cboNombreDeudor);
            this.Controls.Add(this.txtSumaDeuda);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnNuevoDeudor);
            this.Controls.Add(this.lblDeuda);
            this.Controls.Add(this.BtnFiar);
            this.Controls.Add(this.LblDeudor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FIADO";
            this.Text = "FIADO";
            this.Load += new System.EventHandler(this.FIADO_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FIADO_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblDeudor;
        private System.Windows.Forms.Button BtnFiar;
        private System.Windows.Forms.Label lblDeuda;
        private System.Windows.Forms.Button btnNuevoDeudor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSumaDeuda;
        private System.Windows.Forms.ComboBox cboNombreDeudor;
        private System.Windows.Forms.Label lblEstadoDeuda;
    }
}