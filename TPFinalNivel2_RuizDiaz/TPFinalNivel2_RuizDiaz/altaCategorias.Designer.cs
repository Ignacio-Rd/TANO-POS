namespace TPFinalNivel2_RuizDiaz
{
    partial class altaCategorias
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(altaCategorias));
            this.dgvCategoria = new System.Windows.Forms.DataGridView();
            this.btnAgregarCategorias = new System.Windows.Forms.Button();
            this.txtAgregarCategoria = new System.Windows.Forms.TextBox();
            this.btnEliminar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategoria)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCategoria
            // 
            this.dgvCategoria.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCategoria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategoria.Location = new System.Drawing.Point(25, 24);
            this.dgvCategoria.Name = "dgvCategoria";
            this.dgvCategoria.Size = new System.Drawing.Size(334, 240);
            this.dgvCategoria.TabIndex = 0;
            // 
            // btnAgregarCategorias
            // 
            this.btnAgregarCategorias.Location = new System.Drawing.Point(148, 372);
            this.btnAgregarCategorias.Name = "btnAgregarCategorias";
            this.btnAgregarCategorias.Size = new System.Drawing.Size(75, 23);
            this.btnAgregarCategorias.TabIndex = 3;
            this.btnAgregarCategorias.Text = "Agregar";
            this.btnAgregarCategorias.UseVisualStyleBackColor = true;
            this.btnAgregarCategorias.Click += new System.EventHandler(this.btnAgregarCategorias_Click);
            // 
            // txtAgregarCategoria
            // 
            this.txtAgregarCategoria.Location = new System.Drawing.Point(135, 323);
            this.txtAgregarCategoria.Name = "txtAgregarCategoria";
            this.txtAgregarCategoria.Size = new System.Drawing.Size(100, 20);
            this.txtAgregarCategoria.TabIndex = 4;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(148, 415);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 23);
            this.btnEliminar.TabIndex = 5;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // altaCategorias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 450);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.txtAgregarCategoria);
            this.Controls.Add(this.btnAgregarCategorias);
            this.Controls.Add(this.dgvCategoria);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "altaCategorias";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "altaCategorias";
            this.Load += new System.EventHandler(this.altaCategorias_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategoria)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCategoria;
        private System.Windows.Forms.Button btnAgregarCategorias;
        private System.Windows.Forms.TextBox txtAgregarCategoria;
        private System.Windows.Forms.Button btnEliminar;
    }
}