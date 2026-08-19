namespace TPFinalNivel2_RuizDiaz
{
    partial class DetalleDeCompra
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
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.DgvDetalle = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvDetalle)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvHistorial
            // 
            this.dgvHistorial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHistorial.BackgroundColor = System.Drawing.SystemColors.ActiveBorder;
            this.dgvHistorial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistorial.Location = new System.Drawing.Point(48, 45);
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.Size = new System.Drawing.Size(423, 274);
            this.dgvHistorial.TabIndex = 0;
            this.dgvHistorial.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvHistorial_CellFormatting);
            this.dgvHistorial.SelectionChanged += new System.EventHandler(this.dgvHistorial_SelectionChanged);
            // 
            // DgvDetalle
            // 
            this.DgvDetalle.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvDetalle.BackgroundColor = System.Drawing.SystemColors.ActiveBorder;
            this.DgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvDetalle.Location = new System.Drawing.Point(517, 45);
            this.DgvDetalle.Name = "DgvDetalle";
            this.DgvDetalle.Size = new System.Drawing.Size(423, 274);
            this.DgvDetalle.TabIndex = 3;
            // 
            // DetalleDeCompra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 395);
            this.Controls.Add(this.DgvDetalle);
            this.Controls.Add(this.dgvHistorial);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DetalleDeCompra";
            this.Text = "FrmHistorialDelDeudor";
            this.Load += new System.EventHandler(this.FrmHistorialDelDeudor_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvDetalle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.DataGridView DgvDetalle;
    }
}