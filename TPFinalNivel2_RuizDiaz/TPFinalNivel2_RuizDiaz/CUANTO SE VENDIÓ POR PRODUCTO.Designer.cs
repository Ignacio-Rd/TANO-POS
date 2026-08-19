namespace TPFinalNivel2_RuizDiaz
{
    partial class CUANTO_SE_VENDIÓ_POR_PRODUCTO
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.PRODUCTOS = new System.Windows.Forms.Label();
            this.DTMINICIO = new System.Windows.Forms.DateTimePicker();
            this.DGVproductos = new System.Windows.Forms.DataGridView();
            this.BtnBuscar = new System.Windows.Forms.Button();
            this.DtpHoraInicio = new System.Windows.Forms.DateTimePicker();
            this.DtpHoraFin = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCodigoProducto = new System.Windows.Forms.TextBox();
            this.lblProducto = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DGVproductos)).BeginInit();
            this.SuspendLayout();
            // 
            // PRODUCTOS
            // 
            this.PRODUCTOS.AutoSize = true;
            this.PRODUCTOS.Location = new System.Drawing.Point(47, 62);
            this.PRODUCTOS.Name = "PRODUCTOS";
            this.PRODUCTOS.Size = new System.Drawing.Size(75, 13);
            this.PRODUCTOS.TabIndex = 1;
            this.PRODUCTOS.Text = "PRODUCTOS";
            // 
            // DTMINICIO
            // 
            this.DTMINICIO.CustomFormat = "custom";
            this.DTMINICIO.Location = new System.Drawing.Point(489, 12);
            this.DTMINICIO.Name = "DTMINICIO";
            this.DTMINICIO.Size = new System.Drawing.Size(200, 20);
            this.DTMINICIO.TabIndex = 3;
            // 
            // DGVproductos
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Bahnschrift Condensed", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DGVproductos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DGVproductos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVproductos.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DGVproductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVproductos.Location = new System.Drawing.Point(116, 129);
            this.DGVproductos.Name = "DGVproductos";
            this.DGVproductos.Size = new System.Drawing.Size(527, 206);
            this.DGVproductos.TabIndex = 4;
            // 
            // BtnBuscar
            // 
            this.BtnBuscar.Location = new System.Drawing.Point(346, 100);
            this.BtnBuscar.Name = "BtnBuscar";
            this.BtnBuscar.Size = new System.Drawing.Size(75, 23);
            this.BtnBuscar.TabIndex = 5;
            this.BtnBuscar.Text = "BUSCAR";
            this.BtnBuscar.UseVisualStyleBackColor = true;
            this.BtnBuscar.Click += new System.EventHandler(this.BtnBuscar_Click);
            // 
            // DtpHoraInicio
            // 
            this.DtpHoraInicio.CustomFormat = "HH:mm";
            this.DtpHoraInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpHoraInicio.Location = new System.Drawing.Point(489, 38);
            this.DtpHoraInicio.Name = "DtpHoraInicio";
            this.DtpHoraInicio.ShowUpDown = true;
            this.DtpHoraInicio.Size = new System.Drawing.Size(200, 20);
            this.DtpHoraInicio.TabIndex = 14;
            // 
            // DtpHoraFin
            // 
            this.DtpHoraFin.CustomFormat = "HH:mm";
            this.DtpHoraFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpHoraFin.Location = new System.Drawing.Point(489, 64);
            this.DtpHoraFin.Name = "DtpHoraFin";
            this.DtpHoraFin.ShowUpDown = true;
            this.DtpHoraFin.Size = new System.Drawing.Size(200, 20);
            this.DtpHoraFin.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(461, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "DE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(440, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "HASTA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(695, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "HS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(695, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "HS";
            // 
            // txtCodigoProducto
            // 
            this.txtCodigoProducto.Location = new System.Drawing.Point(140, 55);
            this.txtCodigoProducto.Name = "txtCodigoProducto";
            this.txtCodigoProducto.Size = new System.Drawing.Size(100, 20);
            this.txtCodigoProducto.TabIndex = 20;
            this.txtCodigoProducto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCodigoProducto_KeyDown);
            // 
            // lblProducto
            // 
            this.lblProducto.AutoSize = true;
            this.lblProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProducto.Location = new System.Drawing.Point(93, 13);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(0, 20);
            this.lblProducto.TabIndex = 21;
            // 
            // CUANTO_SE_VENDIÓ_POR_PRODUCTO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblProducto);
            this.Controls.Add(this.txtCodigoProducto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DtpHoraFin);
            this.Controls.Add(this.DtpHoraInicio);
            this.Controls.Add(this.BtnBuscar);
            this.Controls.Add(this.DGVproductos);
            this.Controls.Add(this.DTMINICIO);
            this.Controls.Add(this.PRODUCTOS);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CUANTO_SE_VENDIÓ_POR_PRODUCTO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CUANTO_SE_VENDIÓ_POR_PRODUCTO";
            this.Load += new System.EventHandler(this.CUANTO_SE_VENDIÓ_POR_PRODUCTO_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVproductos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PRODUCTOS;
        private System.Windows.Forms.DateTimePicker DTMINICIO;
        private System.Windows.Forms.DataGridView DGVproductos;
        private System.Windows.Forms.Button BtnBuscar;
        private System.Windows.Forms.DateTimePicker DtpHoraInicio;
        private System.Windows.Forms.DateTimePicker DtpHoraFin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCodigoProducto;
        private System.Windows.Forms.Label lblProducto;
    }
}