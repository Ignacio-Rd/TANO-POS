namespace TPFinalNivel2_RuizDiaz
{
    partial class TOTAL_A_COBRAR
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
            this.LblTotal = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnImprimir = new System.Windows.Forms.Button();
            this.Imprimir = new System.Drawing.Printing.PrintDocument();
            this.BtnFiar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMetodo = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bebas Neue", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(111, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 57);
            this.label1.TabIndex = 0;
            this.label1.Text = "total a cobrar:";
            // 
            // LblTotal
            // 
            this.LblTotal.AutoSize = true;
            this.LblTotal.Font = new System.Drawing.Font("Bebas Neue", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTotal.ForeColor = System.Drawing.Color.Red;
            this.LblTotal.Location = new System.Drawing.Point(386, 110);
            this.LblTotal.Name = "LblTotal";
            this.LblTotal.Size = new System.Drawing.Size(0, 57);
            this.LblTotal.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(214, 377);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(195, 44);
            this.button1.TabIndex = 2;
            this.button1.Text = "FINALIZAR";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnImprimir
            // 
            this.BtnImprimir.Location = new System.Drawing.Point(445, 377);
            this.BtnImprimir.Name = "BtnImprimir";
            this.BtnImprimir.Size = new System.Drawing.Size(195, 44);
            this.BtnImprimir.TabIndex = 3;
            this.BtnImprimir.Text = "TICKET";
            this.BtnImprimir.UseVisualStyleBackColor = true;
            this.BtnImprimir.Click += new System.EventHandler(this.BtnImprimir_Click);
            // 
            // BtnFiar
            // 
            this.BtnFiar.Location = new System.Drawing.Point(688, 377);
            this.BtnFiar.Name = "BtnFiar";
            this.BtnFiar.Size = new System.Drawing.Size(195, 44);
            this.BtnFiar.TabIndex = 5;
            this.BtnFiar.Text = "FIAR";
            this.BtnFiar.UseVisualStyleBackColor = true;
            this.BtnFiar.Click += new System.EventHandler(this.btnAbrirFiado_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(123, 240);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "METODO DE PAGO:";
            // 
            // lblMetodo
            // 
            this.lblMetodo.AutoSize = true;
            this.lblMetodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetodo.Location = new System.Drawing.Point(303, 243);
            this.lblMetodo.Name = "lblMetodo";
            this.lblMetodo.Size = new System.Drawing.Size(0, 16);
            this.lblMetodo.TabIndex = 7;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // TOTAL_A_COBRAR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 492);
            this.Controls.Add(this.lblMetodo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnFiar);
            this.Controls.Add(this.BtnImprimir);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LblTotal);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "TOTAL_A_COBRAR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TOTAL_A_COBRAR";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TOTAL_A_COBRAR_FormClosing);
            this.Load += new System.EventHandler(this.TOTAL_A_COBRAR_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TOTAL_A_COBRAR_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LblTotal;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtnImprimir;
        private System.Drawing.Printing.PrintDocument Imprimir;
        private System.Windows.Forms.Button BtnFiar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMetodo;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}