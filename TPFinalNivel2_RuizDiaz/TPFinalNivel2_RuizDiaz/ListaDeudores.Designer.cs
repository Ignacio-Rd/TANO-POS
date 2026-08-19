namespace TPFinalNivel2_RuizDiaz
{
    partial class ListaDeudores
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
            this.DgvDeudores = new System.Windows.Forms.DataGridView();
            this.cboListaDeudores = new System.Windows.Forms.ComboBox();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.DeudaHistorica = new System.Windows.Forms.Label();
            this.lblDeuda = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DgvDeudores)).BeginInit();
            this.SuspendLayout();
            // 
            // DgvDeudores
            // 
            this.DgvDeudores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvDeudores.Location = new System.Drawing.Point(46, 175);
            this.DgvDeudores.Name = "DgvDeudores";
            this.DgvDeudores.Size = new System.Drawing.Size(445, 238);
            this.DgvDeudores.TabIndex = 0;
            this.DgvDeudores.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvDeudores_CellDoubleClick);
            // 
            // cboListaDeudores
            // 
            this.cboListaDeudores.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboListaDeudores.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboListaDeudores.FormattingEnabled = true;
            this.cboListaDeudores.Location = new System.Drawing.Point(121, 36);
            this.cboListaDeudores.Name = "cboListaDeudores";
            this.cboListaDeudores.Size = new System.Drawing.Size(121, 21);
            this.cboListaDeudores.TabIndex = 1;
            this.cboListaDeudores.SelectedIndexChanged += new System.EventHandler(this.cboListaDeudores_SelectedIndexChanged);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(321, 36);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(75, 23);
            this.btnLimpiar.TabIndex = 3;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(169, 442);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(163, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Cancelar o bajar deuda";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // DeudaHistorica
            // 
            this.DeudaHistorica.AutoSize = true;
            this.DeudaHistorica.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeudaHistorica.Location = new System.Drawing.Point(126, 74);
            this.DeudaHistorica.Name = "DeudaHistorica";
            this.DeudaHistorica.Size = new System.Drawing.Size(0, 20);
            this.DeudaHistorica.TabIndex = 30;
            // 
            // lblDeuda
            // 
            this.lblDeuda.AutoSize = true;
            this.lblDeuda.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeuda.Location = new System.Drawing.Point(126, 100);
            this.lblDeuda.Name = "lblDeuda";
            this.lblDeuda.Size = new System.Drawing.Size(0, 20);
            this.lblDeuda.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(404, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "(DOBLE CLICK SOBRE ALGÚN CLIENTE PARA VER SU HISTORIAL DE DEUDA)";
            // 
            // ListaDeudores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 505);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DeudaHistorica);
            this.Controls.Add(this.lblDeuda);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.cboListaDeudores);
            this.Controls.Add(this.DgvDeudores);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ListaDeudores";
            this.Text = "ListaDeudores";
            this.Load += new System.EventHandler(this.ListaDeudores_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListaDeudores_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.DgvDeudores)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DgvDeudores;
        private System.Windows.Forms.ComboBox cboListaDeudores;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label DeudaHistorica;
        private System.Windows.Forms.Label lblDeuda;
        private System.Windows.Forms.Label label1;
    }
}