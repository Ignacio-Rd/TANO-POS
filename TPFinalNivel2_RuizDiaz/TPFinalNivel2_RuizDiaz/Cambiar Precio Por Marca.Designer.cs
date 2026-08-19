namespace TPFinalNivel2_RuizDiaz
{
    partial class Cambiar_Precio_Por_Marca
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.CbxMarca = new System.Windows.Forms.ComboBox();
            this.BtnAceptar = new System.Windows.Forms.Button();
            this.RbtnMontoFijo = new System.Windows.Forms.RadioButton();
            this.RbtnPorcentajeCostoVenta = new System.Windows.Forms.RadioButton();
            this.RbtnPorcentajeSoloVenta = new System.Windows.Forms.RadioButton();
            this.RbtnSumarSoloAVenta = new System.Windows.Forms.RadioButton();
            this.RbtnPorcentajeSoloACosto = new System.Windows.Forms.RadioButton();
            this.RbtnSumaSoloACosto = new System.Windows.Forms.RadioButton();
            this.TxtPorcentajeyocifra = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblIngrese = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.txtCodigoBarras = new System.Windows.Forms.TextBox();
            this.lblProducto = new System.Windows.Forms.Label();
            this.lblStockEntrante = new System.Windows.Forms.Label();
            this.lblStock = new System.Windows.Forms.Label();
            this.lblPorcenOCifra = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(323, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Marca";
            // 
            // CbxMarca
            // 
            this.CbxMarca.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CbxMarca.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CbxMarca.FormattingEnabled = true;
            this.CbxMarca.Location = new System.Drawing.Point(377, 43);
            this.CbxMarca.Name = "CbxMarca";
            this.CbxMarca.Size = new System.Drawing.Size(121, 21);
            this.CbxMarca.TabIndex = 2;
            // 
            // BtnAceptar
            // 
            this.BtnAceptar.Location = new System.Drawing.Point(377, 622);
            this.BtnAceptar.Name = "BtnAceptar";
            this.BtnAceptar.Size = new System.Drawing.Size(252, 23);
            this.BtnAceptar.TabIndex = 4;
            this.BtnAceptar.Text = "APLICAR CAMBIOS";
            this.BtnAceptar.UseVisualStyleBackColor = true;
            this.BtnAceptar.Click += new System.EventHandler(this.BtnAceptar_Click);
            // 
            // RbtnMontoFijo
            // 
            this.RbtnMontoFijo.AutoSize = true;
            this.RbtnMontoFijo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtnMontoFijo.ForeColor = System.Drawing.Color.Lime;
            this.RbtnMontoFijo.Location = new System.Drawing.Point(109, 97);
            this.RbtnMontoFijo.Name = "RbtnMontoFijo";
            this.RbtnMontoFijo.Size = new System.Drawing.Size(364, 17);
            this.RbtnMontoFijo.TabIndex = 6;
            this.RbtnMontoFijo.Tag = "CIFRA_COSTOVENTA";
            this.RbtnMontoFijo.Text = "AUMENTAR POR DINERO AL VALOR DE COSTO Y VENTA";
            this.RbtnMontoFijo.UseVisualStyleBackColor = true;
            this.RbtnMontoFijo.CheckedChanged += new System.EventHandler(this.RbtnMontoFijo_CheckedChanged);
            // 
            // RbtnPorcentajeCostoVenta
            // 
            this.RbtnPorcentajeCostoVenta.AutoSize = true;
            this.RbtnPorcentajeCostoVenta.Checked = true;
            this.RbtnPorcentajeCostoVenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtnPorcentajeCostoVenta.ForeColor = System.Drawing.Color.Lime;
            this.RbtnPorcentajeCostoVenta.Location = new System.Drawing.Point(109, 54);
            this.RbtnPorcentajeCostoVenta.Name = "RbtnPorcentajeCostoVenta";
            this.RbtnPorcentajeCostoVenta.Size = new System.Drawing.Size(397, 17);
            this.RbtnPorcentajeCostoVenta.TabIndex = 9;
            this.RbtnPorcentajeCostoVenta.TabStop = true;
            this.RbtnPorcentajeCostoVenta.Tag = "PORC_COSTOVENTA";
            this.RbtnPorcentajeCostoVenta.Text = "AUMENTAR POR PORCENTAJE AL VALOR DE COSTO Y VENTA";
            this.RbtnPorcentajeCostoVenta.UseVisualStyleBackColor = true;
            this.RbtnPorcentajeCostoVenta.CheckedChanged += new System.EventHandler(this.RbtnPorcentajeCostoVenta_CheckedChanged);
            // 
            // RbtnPorcentajeSoloVenta
            // 
            this.RbtnPorcentajeSoloVenta.AutoSize = true;
            this.RbtnPorcentajeSoloVenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtnPorcentajeSoloVenta.ForeColor = System.Drawing.Color.Red;
            this.RbtnPorcentajeSoloVenta.Location = new System.Drawing.Point(109, 173);
            this.RbtnPorcentajeSoloVenta.Name = "RbtnPorcentajeSoloVenta";
            this.RbtnPorcentajeSoloVenta.Size = new System.Drawing.Size(376, 17);
            this.RbtnPorcentajeSoloVenta.TabIndex = 10;
            this.RbtnPorcentajeSoloVenta.Tag = "PORC_VENTA";
            this.RbtnPorcentajeSoloVenta.Text = "AUMENTAR POR PORCENTAJE SOLO AL VALOR DE VENTA";
            this.RbtnPorcentajeSoloVenta.UseVisualStyleBackColor = true;
            this.RbtnPorcentajeSoloVenta.CheckedChanged += new System.EventHandler(this.RbtnPorcentajeSoloVenta_CheckedChanged);
            // 
            // RbtnSumarSoloAVenta
            // 
            this.RbtnSumarSoloAVenta.AutoSize = true;
            this.RbtnSumarSoloAVenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtnSumarSoloAVenta.ForeColor = System.Drawing.Color.Red;
            this.RbtnSumarSoloAVenta.Location = new System.Drawing.Point(109, 211);
            this.RbtnSumarSoloAVenta.Name = "RbtnSumarSoloAVenta";
            this.RbtnSumarSoloAVenta.Size = new System.Drawing.Size(343, 17);
            this.RbtnSumarSoloAVenta.TabIndex = 11;
            this.RbtnSumarSoloAVenta.Tag = "CIFRA_VENTA";
            this.RbtnSumarSoloAVenta.Text = "AUMENTAR POR DINERO SOLO AL VALOR DE VENTA";
            this.RbtnSumarSoloAVenta.UseVisualStyleBackColor = true;
            this.RbtnSumarSoloAVenta.CheckedChanged += new System.EventHandler(this.RbtnSumarSoloAVenta_CheckedChanged);
            // 
            // RbtnPorcentajeSoloACosto
            // 
            this.RbtnPorcentajeSoloACosto.AutoSize = true;
            this.RbtnPorcentajeSoloACosto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtnPorcentajeSoloACosto.ForeColor = System.Drawing.Color.Gold;
            this.RbtnPorcentajeSoloACosto.Location = new System.Drawing.Point(109, 298);
            this.RbtnPorcentajeSoloACosto.Name = "RbtnPorcentajeSoloACosto";
            this.RbtnPorcentajeSoloACosto.Size = new System.Drawing.Size(377, 17);
            this.RbtnPorcentajeSoloACosto.TabIndex = 12;
            this.RbtnPorcentajeSoloACosto.Tag = "PORC_COSTO";
            this.RbtnPorcentajeSoloACosto.Text = "AUMENTAR POR PORCENTAJE SOLO AL VALOR DE COSTO";
            this.RbtnPorcentajeSoloACosto.UseVisualStyleBackColor = true;
            this.RbtnPorcentajeSoloACosto.CheckedChanged += new System.EventHandler(this.RbtnPorcentajeSoloACosto_CheckedChanged);
            // 
            // RbtnSumaSoloACosto
            // 
            this.RbtnSumaSoloACosto.AutoSize = true;
            this.RbtnSumaSoloACosto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtnSumaSoloACosto.ForeColor = System.Drawing.Color.Gold;
            this.RbtnSumaSoloACosto.Location = new System.Drawing.Point(109, 341);
            this.RbtnSumaSoloACosto.Name = "RbtnSumaSoloACosto";
            this.RbtnSumaSoloACosto.Size = new System.Drawing.Size(344, 17);
            this.RbtnSumaSoloACosto.TabIndex = 13;
            this.RbtnSumaSoloACosto.Tag = "CIFRA_COSTO";
            this.RbtnSumaSoloACosto.Text = "AUMENTAR POR DINERO SOLO AL VALOR DE COSTO";
            this.RbtnSumaSoloACosto.UseVisualStyleBackColor = true;
            this.RbtnSumaSoloACosto.CheckedChanged += new System.EventHandler(this.RbtnSumaSoloACosto_CheckedChanged);
            // 
            // TxtPorcentajeyocifra
            // 
            this.TxtPorcentajeyocifra.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtPorcentajeyocifra.Location = new System.Drawing.Point(571, 533);
            this.TxtPorcentajeyocifra.Name = "TxtPorcentajeyocifra";
            this.TxtPorcentajeyocifra.Size = new System.Drawing.Size(121, 22);
            this.TxtPorcentajeyocifra.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox1.Controls.Add(this.RbtnPorcentajeCostoVenta);
            this.groupBox1.Controls.Add(this.RbtnSumaSoloACosto);
            this.groupBox1.Controls.Add(this.RbtnMontoFijo);
            this.groupBox1.Controls.Add(this.RbtnPorcentajeSoloACosto);
            this.groupBox1.Controls.Add(this.RbtnPorcentajeSoloVenta);
            this.groupBox1.Controls.Add(this.RbtnSumarSoloAVenta);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Info;
            this.groupBox1.Location = new System.Drawing.Point(202, 111);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(593, 393);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TIPO DE AJUSTE";
            // 
            // lblIngrese
            // 
            this.lblIngrese.AutoSize = true;
            this.lblIngrese.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIngrese.Location = new System.Drawing.Point(78, 539);
            this.lblIngrese.Name = "lblIngrese";
            this.lblIngrese.Size = new System.Drawing.Size(482, 16);
            this.lblIngrese.TabIndex = 15;
            this.lblIngrese.Text = "INGRESE PORCENTAJE A SUMAR AL VALOR DE COSTO Y VENTA:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(255, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Código Del Producto";
            this.label3.Visible = false;
            // 
            // txtCodigoBarras
            // 
            this.txtCodigoBarras.Location = new System.Drawing.Point(377, 73);
            this.txtCodigoBarras.Name = "txtCodigoBarras";
            this.txtCodigoBarras.Size = new System.Drawing.Size(164, 20);
            this.txtCodigoBarras.TabIndex = 17;
            this.txtCodigoBarras.Visible = false;
            this.txtCodigoBarras.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCodigoBarras_KeyDown);
            // 
            // lblProducto
            // 
            this.lblProducto.AutoSize = true;
            this.lblProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProducto.Location = new System.Drawing.Point(560, 76);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(0, 20);
            this.lblProducto.TabIndex = 18;
            // 
            // lblStockEntrante
            // 
            this.lblStockEntrante.AutoSize = true;
            this.lblStockEntrante.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStockEntrante.Location = new System.Drawing.Point(338, 581);
            this.lblStockEntrante.Name = "lblStockEntrante";
            this.lblStockEntrante.Size = new System.Drawing.Size(175, 16);
            this.lblStockEntrante.TabIndex = 19;
            this.lblStockEntrante.Text = "STOCK QUE ENTRA DE";
            this.lblStockEntrante.Visible = false;
            // 
            // lblStock
            // 
            this.lblStock.AutoSize = true;
            this.lblStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStock.Location = new System.Drawing.Point(519, 581);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(0, 16);
            this.lblStock.TabIndex = 20;
            this.lblStock.Visible = false;
            // 
            // lblPorcenOCifra
            // 
            this.lblPorcenOCifra.AutoSize = true;
            this.lblPorcenOCifra.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPorcenOCifra.Location = new System.Drawing.Point(698, 536);
            this.lblPorcenOCifra.Name = "lblPorcenOCifra";
            this.lblPorcenOCifra.Size = new System.Drawing.Size(0, 16);
            this.lblPorcenOCifra.TabIndex = 21;
            // 
            // Cambiar_Precio_Por_Marca
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 669);
            this.Controls.Add(this.lblPorcenOCifra);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.lblStockEntrante);
            this.Controls.Add(this.lblProducto);
            this.Controls.Add(this.txtCodigoBarras);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblIngrese);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnAceptar);
            this.Controls.Add(this.TxtPorcentajeyocifra);
            this.Controls.Add(this.CbxMarca);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Cambiar_Precio_Por_Marca";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cambiar_Precio_Por_Marca";
            this.Load += new System.EventHandler(this.Cambiar_Precio_Por_Marca_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CbxMarca;
        private System.Windows.Forms.Button BtnAceptar;
        private System.Windows.Forms.RadioButton RbtnMontoFijo;
        private System.Windows.Forms.RadioButton RbtnPorcentajeCostoVenta;
        private System.Windows.Forms.RadioButton RbtnPorcentajeSoloVenta;
        private System.Windows.Forms.RadioButton RbtnSumarSoloAVenta;
        private System.Windows.Forms.RadioButton RbtnPorcentajeSoloACosto;
        private System.Windows.Forms.RadioButton RbtnSumaSoloACosto;
        private System.Windows.Forms.TextBox TxtPorcentajeyocifra;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblIngrese;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox txtCodigoBarras;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.Label lblStockEntrante;
        private System.Windows.Forms.Label lblPorcenOCifra;
    }
}