namespace TPFinalNivel2_RuizDiaz
{
    partial class ENTRADA_DE_MERCADERIA
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
            this.CodigoProdcutotxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Lbl_Producto = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Cantidad_Stock_Ingreso = new System.Windows.Forms.TextBox();
            this.Btn_Ingresar_Stock = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.PrecioCosto_txt = new System.Windows.Forms.TextBox();
            this.BtnSoloCantidad = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_Precio_Venta = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.AumentarPorPorcentajeOCifra = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.CboPrecioTeclado = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblProveedor = new System.Windows.Forms.Label();
            this.btnFinalizar = new System.Windows.Forms.Button();
            this.dgvListaCompra = new System.Windows.Forms.DataGridView();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtPagoAProveedor = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblDeuda = new System.Windows.Forms.Label();
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaCompra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "CÓDIGO PRODUCTO:";
            // 
            // CodigoProdcutotxt
            // 
            this.CodigoProdcutotxt.Location = new System.Drawing.Point(189, 32);
            this.CodigoProdcutotxt.Name = "CodigoProdcutotxt";
            this.CodigoProdcutotxt.Size = new System.Drawing.Size(250, 20);
            this.CodigoProdcutotxt.TabIndex = 0;
            this.CodigoProdcutotxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CodigoProdcutotxt_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "PRODUCTO:";
            // 
            // Lbl_Producto
            // 
            this.Lbl_Producto.AutoSize = true;
            this.Lbl_Producto.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Producto.Location = new System.Drawing.Point(189, 67);
            this.Lbl_Producto.Name = "Lbl_Producto";
            this.Lbl_Producto.Size = new System.Drawing.Size(0, 20);
            this.Lbl_Producto.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "CANTIDAD QUE INGRESA:";
            // 
            // Cantidad_Stock_Ingreso
            // 
            this.Cantidad_Stock_Ingreso.Location = new System.Drawing.Point(192, 137);
            this.Cantidad_Stock_Ingreso.Name = "Cantidad_Stock_Ingreso";
            this.Cantidad_Stock_Ingreso.Size = new System.Drawing.Size(100, 20);
            this.Cantidad_Stock_Ingreso.TabIndex = 2;
            this.Cantidad_Stock_Ingreso.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Cantidad_Stock_Ingreso_KeyDown);
            // 
            // Btn_Ingresar_Stock
            // 
            this.Btn_Ingresar_Stock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.Btn_Ingresar_Stock.Location = new System.Drawing.Point(144, 637);
            this.Btn_Ingresar_Stock.Name = "Btn_Ingresar_Stock";
            this.Btn_Ingresar_Stock.Size = new System.Drawing.Size(142, 23);
            this.Btn_Ingresar_Stock.TabIndex = 5;
            this.Btn_Ingresar_Stock.Text = "AÑADIR A LISTA";
            this.Btn_Ingresar_Stock.UseVisualStyleBackColor = false;
            this.Btn_Ingresar_Stock.Visible = false;
            this.Btn_Ingresar_Stock.Click += new System.EventHandler(this.Btn_Ingresar_Stock_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 541);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "PRECIO DE COSTO:";
            this.label4.Visible = false;
            // 
            // PrecioCosto_txt
            // 
            this.PrecioCosto_txt.Location = new System.Drawing.Point(209, 533);
            this.PrecioCosto_txt.Name = "PrecioCosto_txt";
            this.PrecioCosto_txt.Size = new System.Drawing.Size(100, 20);
            this.PrecioCosto_txt.TabIndex = 3;
            this.PrecioCosto_txt.Visible = false;
            this.PrecioCosto_txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PrecioCosto_txt_KeyDown);
            // 
            // BtnSoloCantidad
            // 
            this.BtnSoloCantidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.BtnSoloCantidad.Location = new System.Drawing.Point(334, 637);
            this.BtnSoloCantidad.Name = "BtnSoloCantidad";
            this.BtnSoloCantidad.Size = new System.Drawing.Size(158, 23);
            this.BtnSoloCantidad.TabIndex = 9;
            this.BtnSoloCantidad.Text = "CANCELAR INGRESO";
            this.BtnSoloCantidad.UseVisualStyleBackColor = false;
            this.BtnSoloCantidad.Visible = false;
            this.BtnSoloCantidad.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 597);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "PRECIO DE VENTA";
            this.label5.Visible = false;
            // 
            // txt_Precio_Venta
            // 
            this.txt_Precio_Venta.Location = new System.Drawing.Point(209, 590);
            this.txt_Precio_Venta.Name = "txt_Precio_Venta";
            this.txt_Precio_Venta.Size = new System.Drawing.Size(100, 20);
            this.txt_Precio_Venta.TabIndex = 4;
            this.txt_Precio_Venta.Visible = false;
            this.txt_Precio_Venta.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Precio_Venta_KeyDown);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button1.Location = new System.Drawing.Point(62, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(192, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "SOLO AÑADIR A LISTA";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // AumentarPorPorcentajeOCifra
            // 
            this.AumentarPorPorcentajeOCifra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.AumentarPorPorcentajeOCifra.Location = new System.Drawing.Point(29, 21);
            this.AumentarPorPorcentajeOCifra.Name = "AumentarPorPorcentajeOCifra";
            this.AumentarPorPorcentajeOCifra.Size = new System.Drawing.Size(262, 61);
            this.AumentarPorPorcentajeOCifra.TabIndex = 13;
            this.AumentarPorPorcentajeOCifra.Text = "AUMENTAR PRECIO POR PORCENTAJE O DINERO EN CASO DE AUMENTO Y AÑADIR A LISTA";
            this.AumentarPorPorcentajeOCifra.UseVisualStyleBackColor = false;
            this.AumentarPorPorcentajeOCifra.Click += new System.EventHandler(this.AumentarPorPorcentajeOCifra_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Cantidad_Stock_Ingreso);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Lbl_Producto);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.CodigoProdcutotxt);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(16, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(561, 180);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DATOS DEL PRODUCTO";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.AumentarPorPorcentajeOCifra);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(46, 390);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(517, 88);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ACTUALIZACIÓN DE PRECIO";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button3.Location = new System.Drawing.Point(328, 40);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(158, 23);
            this.button3.TabIndex = 20;
            this.button3.Text = "CANCELAR INGRESO";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Location = new System.Drawing.Point(46, 243);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(517, 83);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SOLO INGRESAR CANTIDAD DE STOCK";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button2.Location = new System.Drawing.Point(328, 33);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(158, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "CANCELAR INGRESO";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // CboPrecioTeclado
            // 
            this.CboPrecioTeclado.AutoSize = true;
            this.CboPrecioTeclado.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboPrecioTeclado.Location = new System.Drawing.Point(47, 495);
            this.CboPrecioTeclado.Name = "CboPrecioTeclado";
            this.CboPrecioTeclado.Size = new System.Drawing.Size(352, 20);
            this.CboPrecioTeclado.TabIndex = 18;
            this.CboPrecioTeclado.Text = "INGRESAR PRECIOS NUEVOS POR TECLADO";
            this.CboPrecioTeclado.UseVisualStyleBackColor = true;
            this.CboPrecioTeclado.Visible = false;
            this.CboPrecioTeclado.CheckedChanged += new System.EventHandler(this.CboPrecioTeclado_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(46, 348);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(330, 20);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "ACTUALIZAR PRECIO E INGRESAR STOCK";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(163, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 15);
            this.label6.TabIndex = 20;
            this.label6.Text = "PROVEEDOR:";
            // 
            // lblProveedor
            // 
            this.lblProveedor.AutoSize = true;
            this.lblProveedor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProveedor.Location = new System.Drawing.Point(261, 20);
            this.lblProveedor.Name = "lblProveedor";
            this.lblProveedor.Size = new System.Drawing.Size(0, 15);
            this.lblProveedor.TabIndex = 21;
            // 
            // btnFinalizar
            // 
            this.btnFinalizar.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFinalizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinalizar.ForeColor = System.Drawing.Color.White;
            this.btnFinalizar.Location = new System.Drawing.Point(786, 590);
            this.btnFinalizar.Name = "btnFinalizar";
            this.btnFinalizar.Size = new System.Drawing.Size(317, 53);
            this.btnFinalizar.TabIndex = 22;
            this.btnFinalizar.Text = "FINALIZAR INGRESO";
            this.btnFinalizar.UseVisualStyleBackColor = false;
            this.btnFinalizar.Click += new System.EventHandler(this.btnFinalizar_Click);
            // 
            // dgvListaCompra
            // 
            this.dgvListaCompra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListaCompra.Location = new System.Drawing.Point(662, 48);
            this.dgvListaCompra.Name = "dgvListaCompra";
            this.dgvListaCompra.Size = new System.Drawing.Size(543, 348);
            this.dgvListaCompra.TabIndex = 23;
            this.dgvListaCompra.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvListaCompra_CellValidating);
            this.dgvListaCompra.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListaCompra_CellValueChanged);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(866, 411);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(164, 23);
            this.btnEliminar.TabIndex = 24;
            this.btnEliminar.Text = "Eliminar Producto de Lista";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(856, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 15);
            this.label7.TabIndex = 25;
            this.label7.Text = "LISTA DE COMPRAS:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(786, 526);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "PAGO AL PROVEEDOR:";
            // 
            // TxtPagoAProveedor
            // 
            this.TxtPagoAProveedor.Location = new System.Drawing.Point(919, 523);
            this.TxtPagoAProveedor.Name = "TxtPagoAProveedor";
            this.TxtPagoAProveedor.Size = new System.Drawing.Size(128, 20);
            this.TxtPagoAProveedor.TabIndex = 27;
            this.TxtPagoAProveedor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtPagoAProveedor_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(783, 457);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "TOTAL COMPRA:";
            // 
            // lblDeuda
            // 
            this.lblDeuda.AutoSize = true;
            this.lblDeuda.Location = new System.Drawing.Point(916, 457);
            this.lblDeuda.Name = "lblDeuda";
            this.lblDeuda.Size = new System.Drawing.Size(0, 13);
            this.lblDeuda.TabIndex = 29;
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // ENTRADA_DE_MERCADERIA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1230, 704);
            this.Controls.Add(this.lblDeuda);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.TxtPagoAProveedor);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.dgvListaCompra);
            this.Controls.Add(this.btnFinalizar);
            this.Controls.Add(this.lblProveedor);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.CboPrecioTeclado);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txt_Precio_Venta);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BtnSoloCantidad);
            this.Controls.Add(this.PrecioCosto_txt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Btn_Ingresar_Stock);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "ENTRADA_DE_MERCADERIA";
            this.Text = "ENTRADA_DE_MERCADERIA";
            this.Load += new System.EventHandler(this.ENTRADA_DE_MERCADERIA_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaCompra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CodigoProdcutotxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Lbl_Producto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Cantidad_Stock_Ingreso;
        private System.Windows.Forms.Button Btn_Ingresar_Stock;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PrecioCosto_txt;
        private System.Windows.Forms.Button BtnSoloCantidad;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_Precio_Venta;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button AumentarPorPorcentajeOCifra;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CboPrecioTeclado;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label lblProveedor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnFinalizar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.DataGridView dgvListaCompra;
        private System.Windows.Forms.TextBox TxtPagoAProveedor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblDeuda;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ErrorProvider errorProvider2;
    }
}