namespace TPFinalNivel2_RuizDiaz
{
    partial class FechaInformeDeVentas
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
            this.DtmInforme = new System.Windows.Forms.DateTimePicker();
            this.DtmInformeFecha1 = new System.Windows.Forms.DateTimePicker();
            this.DtmFechaInforme2 = new System.Windows.Forms.DateTimePicker();
            this.DtmInformeDesde = new System.Windows.Forms.DateTimePicker();
            this.DtmInformeHasta = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rbtnRangoFecha = new System.Windows.Forms.RadioButton();
            this.rbtnFechaEspecifica = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.GpbRangoFecha = new System.Windows.Forms.GroupBox();
            this.GbFechaEspecifica = new System.Windows.Forms.GroupBox();
            this.BtnPedirInforme = new System.Windows.Forms.Button();
            this.BtnVerEnPantalla = new System.Windows.Forms.Button();
            this.GpbRangoFecha.SuspendLayout();
            this.GbFechaEspecifica.SuspendLayout();
            this.SuspendLayout();
            // 
            // DtmInforme
            // 
            this.DtmInforme.Location = new System.Drawing.Point(219, 23);
            this.DtmInforme.Name = "DtmInforme";
            this.DtmInforme.Size = new System.Drawing.Size(200, 20);
            this.DtmInforme.TabIndex = 1;
            // 
            // DtmInformeFecha1
            // 
            this.DtmInformeFecha1.Location = new System.Drawing.Point(79, 29);
            this.DtmInformeFecha1.Name = "DtmInformeFecha1";
            this.DtmInformeFecha1.Size = new System.Drawing.Size(200, 20);
            this.DtmInformeFecha1.TabIndex = 4;
            // 
            // DtmFechaInforme2
            // 
            this.DtmFechaInforme2.Location = new System.Drawing.Point(409, 29);
            this.DtmFechaInforme2.Name = "DtmFechaInforme2";
            this.DtmFechaInforme2.Size = new System.Drawing.Size(200, 20);
            this.DtmFechaInforme2.TabIndex = 5;
            // 
            // DtmInformeDesde
            // 
            this.DtmInformeDesde.CustomFormat = "HH:mm";
            this.DtmInformeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtmInformeDesde.Location = new System.Drawing.Point(101, 89);
            this.DtmInformeDesde.Name = "DtmInformeDesde";
            this.DtmInformeDesde.Size = new System.Drawing.Size(200, 20);
            this.DtmInformeDesde.TabIndex = 6;
            // 
            // DtmInformeHasta
            // 
            this.DtmInformeHasta.CustomFormat = "HH:mm";
            this.DtmInformeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtmInformeHasta.Location = new System.Drawing.Point(396, 88);
            this.DtmInformeHasta.Name = "DtmInformeHasta";
            this.DtmInformeHasta.Size = new System.Drawing.Size(200, 20);
            this.DtmInformeHasta.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "DESDE:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(344, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "HASTA:";
            // 
            // rbtnRangoFecha
            // 
            this.rbtnRangoFecha.AutoSize = true;
            this.rbtnRangoFecha.Location = new System.Drawing.Point(333, 218);
            this.rbtnRangoFecha.Name = "rbtnRangoFecha";
            this.rbtnRangoFecha.Size = new System.Drawing.Size(171, 17);
            this.rbtnRangoFecha.TabIndex = 10;
            this.rbtnRangoFecha.TabStop = true;
            this.rbtnRangoFecha.Text = "ELEGIR UN RANGO DE DÍAS";
            this.rbtnRangoFecha.UseVisualStyleBackColor = true;
            this.rbtnRangoFecha.CheckedChanged += new System.EventHandler(this.rbtnRangoFecha_CheckedChanged);
            // 
            // rbtnFechaEspecifica
            // 
            this.rbtnFechaEspecifica.AutoSize = true;
            this.rbtnFechaEspecifica.Location = new System.Drawing.Point(286, 27);
            this.rbtnFechaEspecifica.Name = "rbtnFechaEspecifica";
            this.rbtnFechaEspecifica.Size = new System.Drawing.Size(192, 17);
            this.rbtnFechaEspecifica.TabIndex = 11;
            this.rbtnFechaEspecifica.TabStop = true;
            this.rbtnFechaEspecifica.Text = "ELEGIR UNA FECHA ESPECÍFICA";
            this.rbtnFechaEspecifica.UseVisualStyleBackColor = true;
            this.rbtnFechaEspecifica.CheckedChanged += new System.EventHandler(this.rbtnFechaEspecifica_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "DESDE:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(340, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "HASTA:";
            // 
            // GpbRangoFecha
            // 
            this.GpbRangoFecha.Controls.Add(this.label4);
            this.GpbRangoFecha.Controls.Add(this.label3);
            this.GpbRangoFecha.Controls.Add(this.DtmFechaInforme2);
            this.GpbRangoFecha.Controls.Add(this.DtmInformeFecha1);
            this.GpbRangoFecha.Location = new System.Drawing.Point(63, 241);
            this.GpbRangoFecha.Name = "GpbRangoFecha";
            this.GpbRangoFecha.Size = new System.Drawing.Size(669, 70);
            this.GpbRangoFecha.TabIndex = 14;
            this.GpbRangoFecha.TabStop = false;
            this.GpbRangoFecha.Text = "RANGO DE FECHA";
            // 
            // GbFechaEspecifica
            // 
            this.GbFechaEspecifica.Controls.Add(this.label2);
            this.GbFechaEspecifica.Controls.Add(this.label1);
            this.GbFechaEspecifica.Controls.Add(this.DtmInformeHasta);
            this.GbFechaEspecifica.Controls.Add(this.DtmInformeDesde);
            this.GbFechaEspecifica.Controls.Add(this.DtmInforme);
            this.GbFechaEspecifica.Location = new System.Drawing.Point(59, 50);
            this.GbFechaEspecifica.Name = "GbFechaEspecifica";
            this.GbFechaEspecifica.Size = new System.Drawing.Size(672, 141);
            this.GbFechaEspecifica.TabIndex = 15;
            this.GbFechaEspecifica.TabStop = false;
            this.GbFechaEspecifica.Text = "FECHA ESPECÍFICA";
            // 
            // BtnPedirInforme
            // 
            this.BtnPedirInforme.Location = new System.Drawing.Point(455, 337);
            this.BtnPedirInforme.Name = "BtnPedirInforme";
            this.BtnPedirInforme.Size = new System.Drawing.Size(146, 42);
            this.BtnPedirInforme.TabIndex = 16;
            this.BtnPedirInforme.Text = "IMPRIMIR INFORME";
            this.BtnPedirInforme.UseVisualStyleBackColor = true;
            this.BtnPedirInforme.Click += new System.EventHandler(this.BtnPedirInforme_Click);
            // 
            // BtnVerEnPantalla
            // 
            this.BtnVerEnPantalla.Location = new System.Drawing.Point(214, 337);
            this.BtnVerEnPantalla.Name = "BtnVerEnPantalla";
            this.BtnVerEnPantalla.Size = new System.Drawing.Size(146, 42);
            this.BtnVerEnPantalla.TabIndex = 17;
            this.BtnVerEnPantalla.Text = "VER EN PANTALLA INFORME";
            this.BtnVerEnPantalla.UseVisualStyleBackColor = true;
            this.BtnVerEnPantalla.Click += new System.EventHandler(this.BtnVerEnPantalla_Click);
            // 
            // FechaInformeDeVentas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 391);
            this.Controls.Add(this.BtnVerEnPantalla);
            this.Controls.Add(this.BtnPedirInforme);
            this.Controls.Add(this.GbFechaEspecifica);
            this.Controls.Add(this.GpbRangoFecha);
            this.Controls.Add(this.rbtnFechaEspecifica);
            this.Controls.Add(this.rbtnRangoFecha);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FechaInformeDeVentas";
            this.Text = "FechaInformeDeVentas";
            this.Load += new System.EventHandler(this.FechaInformeDeVentas_Load);
            this.GpbRangoFecha.ResumeLayout(false);
            this.GpbRangoFecha.PerformLayout();
            this.GbFechaEspecifica.ResumeLayout(false);
            this.GbFechaEspecifica.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker DtmInforme;
        private System.Windows.Forms.DateTimePicker DtmInformeFecha1;
        private System.Windows.Forms.DateTimePicker DtmFechaInforme2;
        private System.Windows.Forms.DateTimePicker DtmInformeDesde;
        private System.Windows.Forms.DateTimePicker DtmInformeHasta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbtnRangoFecha;
        private System.Windows.Forms.RadioButton rbtnFechaEspecifica;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox GpbRangoFecha;
        private System.Windows.Forms.GroupBox GbFechaEspecifica;
        private System.Windows.Forms.Button BtnPedirInforme;
        private System.Windows.Forms.Button BtnVerEnPantalla;
    }
}