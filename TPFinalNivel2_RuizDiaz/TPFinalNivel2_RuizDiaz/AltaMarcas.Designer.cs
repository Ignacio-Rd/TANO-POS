namespace TPFinalNivel2_RuizDiaz
{
    partial class AltaMarcas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AltaMarcas));
            this.dgvMarcas = new System.Windows.Forms.DataGridView();
            this.btnAgregarMarca = new System.Windows.Forms.Button();
            this.txtAgregarMarca = new System.Windows.Forms.TextBox();
            this.btnEliminar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMarcas)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMarcas
            // 
            this.dgvMarcas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMarcas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMarcas.Location = new System.Drawing.Point(12, 69);
            this.dgvMarcas.Name = "dgvMarcas";
            this.dgvMarcas.Size = new System.Drawing.Size(385, 237);
            this.dgvMarcas.TabIndex = 0;
            // 
            // btnAgregarMarca
            // 
            this.btnAgregarMarca.Location = new System.Drawing.Point(143, 375);
            this.btnAgregarMarca.Name = "btnAgregarMarca";
            this.btnAgregarMarca.Size = new System.Drawing.Size(75, 23);
            this.btnAgregarMarca.TabIndex = 1;
            this.btnAgregarMarca.Text = "Agregar";
            this.btnAgregarMarca.UseVisualStyleBackColor = true;
            this.btnAgregarMarca.Click += new System.EventHandler(this.btnAgregarMarca_Click);
            // 
            // txtAgregarMarca
            // 
            this.txtAgregarMarca.Location = new System.Drawing.Point(117, 339);
            this.txtAgregarMarca.Name = "txtAgregarMarca";
            this.txtAgregarMarca.Size = new System.Drawing.Size(130, 20);
            this.txtAgregarMarca.TabIndex = 2;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(143, 415);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 23);
            this.btnEliminar.TabIndex = 3;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // AltaMarcas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 450);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.txtAgregarMarca);
            this.Controls.Add(this.btnAgregarMarca);
            this.Controls.Add(this.dgvMarcas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AltaMarcas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AltaMarcas";
            this.Load += new System.EventHandler(this.AltaMarcas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMarcas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMarcas;
        private System.Windows.Forms.Button btnAgregarMarca;
        private System.Windows.Forms.TextBox txtAgregarMarca;
        private System.Windows.Forms.Button btnEliminar;
    }
}