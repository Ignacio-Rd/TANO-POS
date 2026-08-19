namespace TPFinalNivel2_RuizDiaz
{
    partial class ConsultarCompraProveedores
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FiltroRango = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.DtpHoraFin = new System.Windows.Forms.DateTimePicker();
            this.DtpHoraInicio = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DtpFiltro = new System.Windows.Forms.DateTimePicker();
            this.BtnFiltrarFecha = new System.Windows.Forms.Button();
            this.DgvVentas = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDeuda = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cboProveedor = new System.Windows.Forms.ComboBox();
            this.rdbFiltrarNombreFechHora = new System.Windows.Forms.RadioButton();
            this.btnFiltrarPorNombre = new System.Windows.Forms.Button();
            this.btnBajarDeuda = new System.Windows.Forms.Button();
            this.dgvPagos = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnLimpiarGrilla = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvVentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagos)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.FiltroRango);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.DtpHoraFin);
            this.groupBox2.Controls.Add(this.DtpHoraInicio);
            this.groupBox2.Location = new System.Drawing.Point(116, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(694, 117);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RANGO HORARIO";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(661, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "HS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(316, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "HS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "DESDE";
            // 
            // FiltroRango
            // 
            this.FiltroRango.Location = new System.Drawing.Point(254, 69);
            this.FiltroRango.Name = "FiltroRango";
            this.FiltroRango.Size = new System.Drawing.Size(149, 23);
            this.FiltroRango.TabIndex = 15;
            this.FiltroRango.Text = "FILTRAR RANGO";
            this.FiltroRango.UseVisualStyleBackColor = true;
            this.FiltroRango.Click += new System.EventHandler(this.FiltroRango_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(365, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "HASTA";
            // 
            // DtpHoraFin
            // 
            this.DtpHoraFin.CustomFormat = "HH:mm";
            this.DtpHoraFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpHoraFin.Location = new System.Drawing.Point(422, 28);
            this.DtpHoraFin.Name = "DtpHoraFin";
            this.DtpHoraFin.ShowUpDown = true;
            this.DtpHoraFin.Size = new System.Drawing.Size(233, 20);
            this.DtpHoraFin.TabIndex = 14;
            // 
            // DtpHoraInicio
            // 
            this.DtpHoraInicio.CustomFormat = "HH:mm";
            this.DtpHoraInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpHoraInicio.Location = new System.Drawing.Point(76, 28);
            this.DtpHoraInicio.Name = "DtpHoraInicio";
            this.DtpHoraInicio.ShowUpDown = true;
            this.DtpHoraInicio.Size = new System.Drawing.Size(233, 20);
            this.DtpHoraInicio.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Controls.Add(this.DtpFiltro);
            this.groupBox1.Controls.Add(this.BtnFiltrarFecha);
            this.groupBox1.Location = new System.Drawing.Point(116, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(694, 90);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FECHA";
            // 
            // DtpFiltro
            // 
            this.DtpFiltro.Location = new System.Drawing.Point(16, 46);
            this.DtpFiltro.Name = "DtpFiltro";
            this.DtpFiltro.Size = new System.Drawing.Size(233, 20);
            this.DtpFiltro.TabIndex = 2;
            // 
            // BtnFiltrarFecha
            // 
            this.BtnFiltrarFecha.Location = new System.Drawing.Point(287, 43);
            this.BtnFiltrarFecha.Name = "BtnFiltrarFecha";
            this.BtnFiltrarFecha.Size = new System.Drawing.Size(87, 23);
            this.BtnFiltrarFecha.TabIndex = 3;
            this.BtnFiltrarFecha.Text = "FILTRAR";
            this.BtnFiltrarFecha.UseVisualStyleBackColor = true;
            this.BtnFiltrarFecha.Click += new System.EventHandler(this.BtnFiltrarFecha_Click);
            // 
            // DgvVentas
            // 
            this.DgvVentas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvVentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvVentas.Location = new System.Drawing.Point(116, 470);
            this.DgvVentas.Name = "DgvVentas";
            this.DgvVentas.Size = new System.Drawing.Size(662, 214);
            this.DgvVentas.TabIndex = 24;
            this.DgvVentas.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvVentas_CellContentDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 454);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(517, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "(SI QUIERE VER EL DETALLE DE LA COMPRA HAGA DOBLE CLICK SOBRE LA COMPRA QUE DESEE" +
    ")";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(44, 359);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 16);
            this.label6.TabIndex = 26;
            this.label6.Text = "DEUDA DE:";
            // 
            // lblDeuda
            // 
            this.lblDeuda.AutoSize = true;
            this.lblDeuda.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeuda.Location = new System.Drawing.Point(140, 359);
            this.lblDeuda.Name = "lblDeuda";
            this.lblDeuda.Size = new System.Drawing.Size(0, 16);
            this.lblDeuda.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(71, 256);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(303, 16);
            this.label7.TabIndex = 28;
            this.label7.Text = "BUSCAR POR NOMBRE DE PROVEEDOR:";
            // 
            // cboProveedor
            // 
            this.cboProveedor.FormattingEnabled = true;
            this.cboProveedor.Location = new System.Drawing.Point(409, 255);
            this.cboProveedor.Name = "cboProveedor";
            this.cboProveedor.Size = new System.Drawing.Size(121, 21);
            this.cboProveedor.TabIndex = 29;
            // 
            // rdbFiltrarNombreFechHora
            // 
            this.rdbFiltrarNombreFechHora.AutoSize = true;
            this.rdbFiltrarNombreFechHora.Location = new System.Drawing.Point(577, 256);
            this.rdbFiltrarNombreFechHora.Name = "rdbFiltrarNombreFechHora";
            this.rdbFiltrarNombreFechHora.Size = new System.Drawing.Size(212, 17);
            this.rdbFiltrarNombreFechHora.TabIndex = 30;
            this.rdbFiltrarNombreFechHora.TabStop = true;
            this.rdbFiltrarNombreFechHora.Text = "BUSCAR CON EL RANGO DE FECHA ";
            this.rdbFiltrarNombreFechHora.UseVisualStyleBackColor = true;
            this.rdbFiltrarNombreFechHora.Click += new System.EventHandler(this.rdbFiltrarNombreFechHora_Click);
            // 
            // btnFiltrarPorNombre
            // 
            this.btnFiltrarPorNombre.Location = new System.Drawing.Point(326, 309);
            this.btnFiltrarPorNombre.Name = "btnFiltrarPorNombre";
            this.btnFiltrarPorNombre.Size = new System.Drawing.Size(258, 23);
            this.btnFiltrarPorNombre.TabIndex = 31;
            this.btnFiltrarPorNombre.Text = "FILTRAR POR NOMBRE DE PROVEEDOR";
            this.btnFiltrarPorNombre.UseVisualStyleBackColor = true;
            this.btnFiltrarPorNombre.Click += new System.EventHandler(this.btnFiltrarPorNombre_Click);
            // 
            // btnBajarDeuda
            // 
            this.btnBajarDeuda.Location = new System.Drawing.Point(152, 390);
            this.btnBajarDeuda.Name = "btnBajarDeuda";
            this.btnBajarDeuda.Size = new System.Drawing.Size(139, 23);
            this.btnBajarDeuda.TabIndex = 32;
            this.btnBajarDeuda.Text = "Cancelar o bajar deuda";
            this.btnBajarDeuda.UseVisualStyleBackColor = true;
            this.btnBajarDeuda.Visible = false;
            this.btnBajarDeuda.Click += new System.EventHandler(this.btnBajarDeuda_Click);
            // 
            // dgvPagos
            // 
            this.dgvPagos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPagos.Location = new System.Drawing.Point(917, 460);
            this.dgvPagos.Name = "dgvPagos";
            this.dgvPagos.Size = new System.Drawing.Size(656, 214);
            this.dgvPagos.TabIndex = 33;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(148, 429);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 16);
            this.label8.TabIndex = 34;
            this.label8.Text = "COMPRAS:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(1033, 419);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 16);
            this.label9.TabIndex = 35;
            this.label9.Text = "PAGOS:";
            // 
            // btnLimpiarGrilla
            // 
            this.btnLimpiarGrilla.Location = new System.Drawing.Point(538, 390);
            this.btnLimpiarGrilla.Name = "btnLimpiarGrilla";
            this.btnLimpiarGrilla.Size = new System.Drawing.Size(119, 23);
            this.btnLimpiarGrilla.TabIndex = 36;
            this.btnLimpiarGrilla.Text = "Limpiar grillas";
            this.btnLimpiarGrilla.UseVisualStyleBackColor = true;
            this.btnLimpiarGrilla.Click += new System.EventHandler(this.btnLimpiarGrilla_Click);
            // 
            // ConsultarCompraProveedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 701);
            this.Controls.Add(this.btnLimpiarGrilla);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dgvPagos);
            this.Controls.Add(this.btnBajarDeuda);
            this.Controls.Add(this.btnFiltrarPorNombre);
            this.Controls.Add(this.rdbFiltrarNombreFechHora);
            this.Controls.Add(this.cboProveedor);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblDeuda);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DgvVentas);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConsultarCompraProveedores";
            this.Text = "ConsultarCompraProveedores";
            this.Load += new System.EventHandler(this.ConsultarCompraProveedores_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvVentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button FiltroRango;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker DtpHoraFin;
        private System.Windows.Forms.DateTimePicker DtpHoraInicio;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker DtpFiltro;
        private System.Windows.Forms.Button BtnFiltrarFecha;
        private System.Windows.Forms.DataGridView DgvVentas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblDeuda;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboProveedor;
        private System.Windows.Forms.RadioButton rdbFiltrarNombreFechHora;
        private System.Windows.Forms.Button btnFiltrarPorNombre;
        private System.Windows.Forms.Button btnBajarDeuda;
        private System.Windows.Forms.DataGridView dgvPagos;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnLimpiarGrilla;
    }
}