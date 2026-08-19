namespace TPFinalNivel2_RuizDiaz
{
    partial class VentaTotalDia
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
            this.DtmTotalDia = new System.Windows.Forms.DateTimePicker();
            this.BtnTotalDia = new System.Windows.Forms.Button();
            this.Total = new System.Windows.Forms.GroupBox();
            this.LblTotalDia = new System.Windows.Forms.Label();
            this.visible_rango = new System.Windows.Forms.RadioButton();
            this.HoraInicio = new System.Windows.Forms.DateTimePicker();
            this.HoraFin = new System.Windows.Forms.DateTimePicker();
            this.FiltrarRango = new System.Windows.Forms.Button();
            this.Total.SuspendLayout();
            this.SuspendLayout();
            // 
            // DtmTotalDia
            // 
            this.DtmTotalDia.Location = new System.Drawing.Point(227, 44);
            this.DtmTotalDia.Name = "DtmTotalDia";
            this.DtmTotalDia.Size = new System.Drawing.Size(200, 20);
            this.DtmTotalDia.TabIndex = 0;
            // 
            // BtnTotalDia
            // 
            this.BtnTotalDia.Location = new System.Drawing.Point(257, 88);
            this.BtnTotalDia.Name = "BtnTotalDia";
            this.BtnTotalDia.Size = new System.Drawing.Size(141, 23);
            this.BtnTotalDia.TabIndex = 1;
            this.BtnTotalDia.Text = "Calcular Total";
            this.BtnTotalDia.UseVisualStyleBackColor = true;
            this.BtnTotalDia.Click += new System.EventHandler(this.BtnTotalDia_Click);
            // 
            // Total
            // 
            this.Total.Controls.Add(this.LblTotalDia);
            this.Total.Location = new System.Drawing.Point(124, 306);
            this.Total.Name = "Total";
            this.Total.Size = new System.Drawing.Size(389, 100);
            this.Total.TabIndex = 2;
            this.Total.TabStop = false;
            this.Total.Text = "TOTAL";
            // 
            // LblTotalDia
            // 
            this.LblTotalDia.AutoSize = true;
            this.LblTotalDia.Location = new System.Drawing.Point(164, 44);
            this.LblTotalDia.Name = "LblTotalDia";
            this.LblTotalDia.Size = new System.Drawing.Size(0, 13);
            this.LblTotalDia.TabIndex = 0;
            // 
            // visible_rango
            // 
            this.visible_rango.AutoSize = true;
            this.visible_rango.Location = new System.Drawing.Point(257, 137);
            this.visible_rango.Name = "visible_rango";
            this.visible_rango.Size = new System.Drawing.Size(143, 17);
            this.visible_rango.TabIndex = 3;
            this.visible_rango.TabStop = true;
            this.visible_rango.Text = "CON RANGO HORARIO";
            this.visible_rango.UseVisualStyleBackColor = true;
            this.visible_rango.CheckedChanged += new System.EventHandler(this.visible_rango_CheckedChanged);
            // 
            // HoraInicio
            // 
            this.HoraInicio.CustomFormat = "HH";
            this.HoraInicio.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.HoraInicio.Location = new System.Drawing.Point(227, 160);
            this.HoraInicio.Name = "HoraInicio";
            this.HoraInicio.ShowUpDown = true;
            this.HoraInicio.Size = new System.Drawing.Size(200, 20);
            this.HoraInicio.TabIndex = 4;
            this.HoraInicio.Visible = false;
            // 
            // HoraFin
            // 
            this.HoraFin.CustomFormat = "HH";
            this.HoraFin.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.HoraFin.Location = new System.Drawing.Point(227, 196);
            this.HoraFin.Name = "HoraFin";
            this.HoraFin.ShowUpDown = true;
            this.HoraFin.Size = new System.Drawing.Size(200, 20);
            this.HoraFin.TabIndex = 5;
            this.HoraFin.Visible = false;
            // 
            // FiltrarRango
            // 
            this.FiltrarRango.Location = new System.Drawing.Point(257, 238);
            this.FiltrarRango.Name = "FiltrarRango";
            this.FiltrarRango.Size = new System.Drawing.Size(141, 23);
            this.FiltrarRango.TabIndex = 6;
            this.FiltrarRango.Text = "FILTRAR RANGO";
            this.FiltrarRango.UseVisualStyleBackColor = true;
            this.FiltrarRango.Visible = false;
            this.FiltrarRango.Click += new System.EventHandler(this.FiltrarRango_Click);
            // 
            // VentaTotalDia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 450);
            this.Controls.Add(this.FiltrarRango);
            this.Controls.Add(this.HoraFin);
            this.Controls.Add(this.HoraInicio);
            this.Controls.Add(this.visible_rango);
            this.Controls.Add(this.Total);
            this.Controls.Add(this.BtnTotalDia);
            this.Controls.Add(this.DtmTotalDia);
            this.Name = "VentaTotalDia";
            this.Text = "VentaTotalDia";
            this.Load += new System.EventHandler(this.VentaTotalDia_Load);
            this.Total.ResumeLayout(false);
            this.Total.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker DtmTotalDia;
        private System.Windows.Forms.Button BtnTotalDia;
        private System.Windows.Forms.GroupBox Total;
        private System.Windows.Forms.Label LblTotalDia;
        private System.Windows.Forms.RadioButton visible_rango;
        private System.Windows.Forms.DateTimePicker HoraInicio;
        private System.Windows.Forms.DateTimePicker HoraFin;
        private System.Windows.Forms.Button FiltrarRango;
    }
}