namespace TPFinalNivel2_RuizDiaz
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pbxArticulos = new System.Windows.Forms.PictureBox();
            this.BtnModificar = new System.Windows.Forms.Button();
            this.BtnEliminar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFiltro = new System.Windows.Forms.TextBox();
            this.btnVerdetalles = new System.Windows.Forms.Button();
            this.BtnAgregar = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BtnAumentarMarca = new System.Windows.Forms.Button();
            this.BtnPrecioRubro = new System.Windows.Forms.Button();
            this.dgvArticulos = new System.Windows.Forms.DataGridView();
            this.cbxBuscarMarca = new System.Windows.Forms.ComboBox();
            this.cbxBuscarRubro = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnBuscarRubro = new System.Windows.Forms.Button();
            this.BtnBuscarMarca = new System.Windows.Forms.Button();
            this.BtnAumentarProducto = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbxArticulos)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArticulos)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxArticulos
            // 
            this.pbxArticulos.Location = new System.Drawing.Point(1068, 539);
            this.pbxArticulos.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.pbxArticulos.Name = "pbxArticulos";
            this.pbxArticulos.Size = new System.Drawing.Size(264, 164);
            this.pbxArticulos.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxArticulos.TabIndex = 1;
            this.pbxArticulos.TabStop = false;
            this.pbxArticulos.Visible = false;
            // 
            // BtnModificar
            // 
            this.BtnModificar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnModificar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnModificar.Font = new System.Drawing.Font("Bebas Neue", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnModificar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnModificar.Location = new System.Drawing.Point(183, 38);
            this.BtnModificar.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnModificar.Name = "BtnModificar";
            this.BtnModificar.Size = new System.Drawing.Size(163, 39);
            this.BtnModificar.TabIndex = 3;
            this.BtnModificar.Text = "Modificar";
            this.BtnModificar.UseVisualStyleBackColor = true;
            this.BtnModificar.Click += new System.EventHandler(this.BtnModificar_Click);
            // 
            // BtnEliminar
            // 
            this.BtnEliminar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnEliminar.Font = new System.Drawing.Font("Bebas Neue", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnEliminar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnEliminar.Location = new System.Drawing.Point(352, 38);
            this.BtnEliminar.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnEliminar.Name = "BtnEliminar";
            this.BtnEliminar.Size = new System.Drawing.Size(184, 39);
            this.BtnEliminar.TabIndex = 4;
            this.BtnEliminar.Text = "Eliminar";
            this.BtnEliminar.UseVisualStyleBackColor = true;
            this.BtnEliminar.Click += new System.EventHandler(this.BtnEliminar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bebas Neue", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(41, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Filtro:";
            // 
            // txtFiltro
            // 
            this.txtFiltro.Location = new System.Drawing.Point(103, 166);
            this.txtFiltro.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.txtFiltro.Name = "txtFiltro";
            this.txtFiltro.Size = new System.Drawing.Size(287, 30);
            this.txtFiltro.TabIndex = 6;
            this.txtFiltro.TextChanged += new System.EventHandler(this.txtFiltro_TextChanged);
            // 
            // btnVerdetalles
            // 
            this.btnVerdetalles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVerdetalles.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.btnVerdetalles.FlatAppearance.BorderSize = 30;
            this.btnVerdetalles.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnVerdetalles.Font = new System.Drawing.Font("Bebas Neue", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerdetalles.Location = new System.Drawing.Point(1169, 539);
            this.btnVerdetalles.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnVerdetalles.Name = "btnVerdetalles";
            this.btnVerdetalles.Size = new System.Drawing.Size(163, 39);
            this.btnVerdetalles.TabIndex = 16;
            this.btnVerdetalles.Text = "Ver detalles";
            this.btnVerdetalles.UseVisualStyleBackColor = true;
            this.btnVerdetalles.Click += new System.EventHandler(this.btnVerdetalles_Click);
            // 
            // BtnAgregar
            // 
            this.BtnAgregar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnAgregar.Font = new System.Drawing.Font("Bebas Neue", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAgregar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnAgregar.Location = new System.Drawing.Point(11, 38);
            this.BtnAgregar.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnAgregar.Name = "BtnAgregar";
            this.BtnAgregar.Size = new System.Drawing.Size(166, 39);
            this.BtnAgregar.TabIndex = 2;
            this.BtnAgregar.Text = "Agregar";
            this.BtnAgregar.UseVisualStyleBackColor = true;
            this.BtnAgregar.Click += new System.EventHandler(this.BtnAgregar_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Beige;
            this.groupBox2.Controls.Add(this.BtnAumentarProducto);
            this.groupBox2.Controls.Add(this.BtnAumentarMarca);
            this.groupBox2.Controls.Add(this.BtnPrecioRubro);
            this.groupBox2.Controls.Add(this.BtnAgregar);
            this.groupBox2.Controls.Add(this.BtnEliminar);
            this.groupBox2.Controls.Add(this.BtnModificar);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Font = new System.Drawing.Font("Bebas Neue", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox2.Location = new System.Drawing.Point(1, 539);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.groupBox2.Size = new System.Drawing.Size(1162, 102);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ACCIONES";
            // 
            // BtnAumentarMarca
            // 
            this.BtnAumentarMarca.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAumentarMarca.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnAumentarMarca.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAumentarMarca.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnAumentarMarca.Location = new System.Drawing.Point(751, 38);
            this.BtnAumentarMarca.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnAumentarMarca.Name = "BtnAumentarMarca";
            this.BtnAumentarMarca.Size = new System.Drawing.Size(184, 39);
            this.BtnAumentarMarca.TabIndex = 6;
            this.BtnAumentarMarca.Text = "Aumentar por marca";
            this.BtnAumentarMarca.UseVisualStyleBackColor = true;
            this.BtnAumentarMarca.Click += new System.EventHandler(this.BtnAumentarMarca_Click);
            // 
            // BtnPrecioRubro
            // 
            this.BtnPrecioRubro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPrecioRubro.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnPrecioRubro.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPrecioRubro.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnPrecioRubro.Location = new System.Drawing.Point(561, 38);
            this.BtnPrecioRubro.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnPrecioRubro.Name = "BtnPrecioRubro";
            this.BtnPrecioRubro.Size = new System.Drawing.Size(184, 39);
            this.BtnPrecioRubro.TabIndex = 5;
            this.BtnPrecioRubro.Text = "Aumentar por rubro";
            this.BtnPrecioRubro.UseVisualStyleBackColor = true;
            this.BtnPrecioRubro.Click += new System.EventHandler(this.BtnPrecioRubro_Click);
            // 
            // dgvArticulos
            // 
            this.dgvArticulos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvArticulos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArticulos.Location = new System.Drawing.Point(35, 206);
            this.dgvArticulos.Name = "dgvArticulos";
            this.dgvArticulos.RowTemplate.Height = 40;
            this.dgvArticulos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvArticulos.Size = new System.Drawing.Size(1297, 323);
            this.dgvArticulos.TabIndex = 5;
            this.dgvArticulos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArticulos_CellContentClick);
            // 
            // cbxBuscarMarca
            // 
            this.cbxBuscarMarca.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxBuscarMarca.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxBuscarMarca.FormattingEnabled = true;
            this.cbxBuscarMarca.Location = new System.Drawing.Point(714, 65);
            this.cbxBuscarMarca.Name = "cbxBuscarMarca";
            this.cbxBuscarMarca.Size = new System.Drawing.Size(176, 31);
            this.cbxBuscarMarca.TabIndex = 19;
            // 
            // cbxBuscarRubro
            // 
            this.cbxBuscarRubro.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxBuscarRubro.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxBuscarRubro.FormattingEnabled = true;
            this.cbxBuscarRubro.Location = new System.Drawing.Point(714, 153);
            this.cbxBuscarRubro.Name = "cbxBuscarRubro";
            this.cbxBuscarRubro.Size = new System.Drawing.Size(176, 31);
            this.cbxBuscarRubro.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(582, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 23);
            this.label3.TabIndex = 22;
            this.label3.Text = "MARCA";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(583, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 23);
            this.label4.TabIndex = 23;
            this.label4.Text = "RUBRO";
            // 
            // BtnBuscarRubro
            // 
            this.BtnBuscarRubro.Location = new System.Drawing.Point(976, 153);
            this.BtnBuscarRubro.Name = "BtnBuscarRubro";
            this.BtnBuscarRubro.Size = new System.Drawing.Size(126, 31);
            this.BtnBuscarRubro.TabIndex = 24;
            this.BtnBuscarRubro.Text = "BUSCAR";
            this.BtnBuscarRubro.UseVisualStyleBackColor = true;
            this.BtnBuscarRubro.Click += new System.EventHandler(this.BtnBuscarRubro_Click);
            // 
            // BtnBuscarMarca
            // 
            this.BtnBuscarMarca.Location = new System.Drawing.Point(976, 67);
            this.BtnBuscarMarca.Name = "BtnBuscarMarca";
            this.BtnBuscarMarca.Size = new System.Drawing.Size(126, 31);
            this.BtnBuscarMarca.TabIndex = 25;
            this.BtnBuscarMarca.Text = "BUSCAR";
            this.BtnBuscarMarca.UseVisualStyleBackColor = true;
            this.BtnBuscarMarca.Click += new System.EventHandler(this.BtnBuscarMarca_Click);
            // 
            // BtnAumentarProducto
            // 
            this.BtnAumentarProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnAumentarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnAumentarProducto.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAumentarProducto.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnAumentarProducto.Location = new System.Drawing.Point(941, 38);
            this.BtnAumentarProducto.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnAumentarProducto.Name = "BtnAumentarProducto";
            this.BtnAumentarProducto.Size = new System.Drawing.Size(184, 39);
            this.BtnAumentarProducto.TabIndex = 7;
            this.BtnAumentarProducto.Text = "Aumentar por Producto";
            this.BtnAumentarProducto.UseVisualStyleBackColor = true;
            this.BtnAumentarProducto.Click += new System.EventHandler(this.BtnAumentarProducto_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.BtnBuscarMarca);
            this.Controls.Add(this.BtnBuscarRubro);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbxBuscarRubro);
            this.Controls.Add(this.cbxBuscarMarca);
            this.Controls.Add(this.dgvArticulos);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnVerdetalles);
            this.Controls.Add(this.txtFiltro);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbxArticulos);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Font = new System.Drawing.Font("Bebas Neue", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Articulos";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxArticulos)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvArticulos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pbxArticulos;
        private System.Windows.Forms.Button BtnModificar;
        private System.Windows.Forms.Button BtnEliminar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFiltro;
        private System.Windows.Forms.Button btnVerdetalles;
        private System.Windows.Forms.Button BtnAgregar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvArticulos;
        private System.Windows.Forms.Button BtnPrecioRubro;
        private System.Windows.Forms.Button BtnAumentarMarca;
        private System.Windows.Forms.ComboBox cbxBuscarMarca;
        private System.Windows.Forms.ComboBox cbxBuscarRubro;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnBuscarRubro;
        private System.Windows.Forms.Button BtnBuscarMarca;
        private System.Windows.Forms.Button BtnAumentarProducto;
    }
}

