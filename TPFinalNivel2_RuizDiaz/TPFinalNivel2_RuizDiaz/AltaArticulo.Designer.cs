namespace TPFinalNivel2_RuizDiaz
{
    partial class AltaArticulo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AltaArticulo));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbxMarcas = new System.Windows.Forms.ComboBox();
            this.cbxCategoria = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.BtnMarcas = new System.Windows.Forms.Button();
            this.BtnCategorias = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.PrecioCostoTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.AliceBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(35, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Codigo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.AliceBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(35, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Nombre";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.AliceBlue;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(35, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Descripcion";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(151, 32);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(100, 20);
            this.txtCodigo.TabIndex = 0;
            this.txtCodigo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCodigo_KeyDown_1);
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(151, 84);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(100, 20);
            this.txtNombre.TabIndex = 1;
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(151, 136);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(100, 20);
            this.txtDescripcion.TabIndex = 2;
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.Color.AliceBlue;
            this.btnAceptar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAceptar.Location = new System.Drawing.Point(191, 427);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 8;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.AliceBlue;
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancelar.Location = new System.Drawing.Point(467, 427);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.AliceBlue;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(35, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 24);
            this.label6.TabIndex = 10;
            this.label6.Text = "Marcas";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.AliceBlue;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(35, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 24);
            this.label7.TabIndex = 11;
            this.label7.Text = "categoria";
            // 
            // cbxMarcas
            // 
            this.cbxMarcas.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxMarcas.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxMarcas.FormattingEnabled = true;
            this.cbxMarcas.Location = new System.Drawing.Point(151, 184);
            this.cbxMarcas.Name = "cbxMarcas";
            this.cbxMarcas.Size = new System.Drawing.Size(100, 21);
            this.cbxMarcas.TabIndex = 3;
            this.cbxMarcas.Validating += new System.ComponentModel.CancelEventHandler(this.cbxMarcas_Validating);
            // 
            // cbxCategoria
            // 
            this.cbxCategoria.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxCategoria.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxCategoria.FormattingEnabled = true;
            this.cbxCategoria.Location = new System.Drawing.Point(151, 231);
            this.cbxCategoria.Name = "cbxCategoria";
            this.cbxCategoria.Size = new System.Drawing.Size(100, 21);
            this.cbxCategoria.TabIndex = 4;
            this.cbxCategoria.Validating += new System.ComponentModel.CancelEventHandler(this.cbxCategoria_Validating);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.AliceBlue;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(27, 326);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(118, 24);
            this.label9.TabIndex = 15;
            this.label9.Text = "Precio Venta";
            // 
            // txtPrecio
            // 
            this.txtPrecio.Location = new System.Drawing.Point(151, 331);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.Size = new System.Drawing.Size(100, 20);
            this.txtPrecio.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.AliceBlue;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(35, 373);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 24);
            this.label4.TabIndex = 20;
            this.label4.Text = "Stock";
            // 
            // txtStock
            // 
            this.txtStock.Location = new System.Drawing.Point(151, 378);
            this.txtStock.Name = "txtStock";
            this.txtStock.Size = new System.Drawing.Size(100, 20);
            this.txtStock.TabIndex = 7;
            // 
            // BtnMarcas
            // 
            this.BtnMarcas.BackColor = System.Drawing.Color.PowderBlue;
            this.BtnMarcas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnMarcas.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnMarcas.Font = new System.Drawing.Font("Bebas Neue", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMarcas.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnMarcas.Location = new System.Drawing.Point(280, 172);
            this.BtnMarcas.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnMarcas.Name = "BtnMarcas";
            this.BtnMarcas.Size = new System.Drawing.Size(188, 39);
            this.BtnMarcas.TabIndex = 21;
            this.BtnMarcas.Text = "Marcas";
            this.BtnMarcas.UseVisualStyleBackColor = false;
            this.BtnMarcas.Click += new System.EventHandler(this.BtnMarcas_Click);
            // 
            // BtnCategorias
            // 
            this.BtnCategorias.BackColor = System.Drawing.Color.PowderBlue;
            this.BtnCategorias.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCategorias.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnCategorias.Font = new System.Drawing.Font("Bebas Neue", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCategorias.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnCategorias.Location = new System.Drawing.Point(280, 219);
            this.BtnCategorias.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnCategorias.Name = "BtnCategorias";
            this.BtnCategorias.Size = new System.Drawing.Size(188, 39);
            this.BtnCategorias.TabIndex = 22;
            this.BtnCategorias.Text = "rubro";
            this.BtnCategorias.UseVisualStyleBackColor = false;
            this.BtnCategorias.Click += new System.EventHandler(this.BtnCategorias_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.AliceBlue;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(28, 276);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 24);
            this.label5.TabIndex = 23;
            this.label5.Text = "Precio Costo";
            // 
            // PrecioCostoTxt
            // 
            this.PrecioCostoTxt.Location = new System.Drawing.Point(151, 280);
            this.PrecioCostoTxt.Name = "PrecioCostoTxt";
            this.PrecioCostoTxt.Size = new System.Drawing.Size(100, 20);
            this.PrecioCostoTxt.TabIndex = 5;
            // 
            // AltaArticulo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.ClientSize = new System.Drawing.Size(724, 479);
            this.Controls.Add(this.PrecioCostoTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BtnCategorias);
            this.Controls.Add(this.BtnMarcas);
            this.Controls.Add(this.txtStock);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPrecio);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbxCategoria);
            this.Controls.Add(this.cbxMarcas);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AltaArticulo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AltaArticulo";
            this.Load += new System.EventHandler(this.AltaArticulo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbxMarcas;
        private System.Windows.Forms.ComboBox cbxCategoria;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.Button BtnMarcas;
        private System.Windows.Forms.Button BtnCategorias;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox PrecioCostoTxt;
    }
}