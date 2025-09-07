namespace SimuladorGrafos
{
    partial class Form1
    {
        /// Variable del diseñador necesaria.
        private System.ComponentModel.IContainer components = null;

        /// Limpiar los recursos que se estén usando.
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

        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        private void InitializeComponent()
        {
            this.panelGrafo = new System.Windows.Forms.Panel();
            this.btnBFS = new System.Windows.Forms.Button();
            this.btnDFS = new System.Windows.Forms.Button();
            this.txtResultados = new System.Windows.Forms.TextBox();
            this.lblNodoInicio = new System.Windows.Forms.Label();
            this.numericNodoInicio = new System.Windows.Forms.NumericUpDown();
            this.panelControles = new System.Windows.Forms.Panel();
            this.panelGrafo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericNodoInicio)).BeginInit();
            this.SuspendLayout();
            // 
            // panelGrafo
            // 
            this.panelGrafo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGrafo.Controls.Add(this.btnBFS);
            this.panelGrafo.Controls.Add(this.btnDFS);
            this.panelGrafo.Controls.Add(this.txtResultados);
            this.panelGrafo.Controls.Add(this.lblNodoInicio);
            this.panelGrafo.Controls.Add(this.numericNodoInicio);
            this.panelGrafo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelGrafo.Location = new System.Drawing.Point(0, 0);
            this.panelGrafo.Name = "panelGrafo";
            this.panelGrafo.Size = new System.Drawing.Size(569, 662);
            this.panelGrafo.TabIndex = 0;
            // 
            // btnBFS
            // 
            this.btnBFS.BackColor = System.Drawing.Color.LimeGreen;
            this.btnBFS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBFS.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnBFS.Location = new System.Drawing.Point(69, 112);
            this.btnBFS.Name = "btnBFS";
            this.btnBFS.Size = new System.Drawing.Size(204, 45);
            this.btnBFS.TabIndex = 2;
            this.btnBFS.Text = "Ejecutar BFS";
            this.btnBFS.UseVisualStyleBackColor = false;
            this.btnBFS.Click += new System.EventHandler(this.btnBFS_Click);
            // 
            // btnDFS
            // 
            this.btnDFS.BackColor = System.Drawing.Color.Crimson;
            this.btnDFS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDFS.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnDFS.Location = new System.Drawing.Point(292, 112);
            this.btnDFS.Name = "btnDFS";
            this.btnDFS.Size = new System.Drawing.Size(204, 45);
            this.btnDFS.TabIndex = 3;
            this.btnDFS.Text = "Ejecutar DFS";
            this.btnDFS.UseVisualStyleBackColor = false;
            this.btnDFS.Click += new System.EventHandler(this.btnDFS_Click);
            // 
            // txtResultados
            // 
            this.txtResultados.Dock = System.Windows.Forms.DockStyle.None; // Cambiar de Bottom a None
            this.txtResultados.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResultados.Location = new System.Drawing.Point(20, 170);
            this.txtResultados.Multiline = true;
            this.txtResultados.Name = "txtResultados";
            this.txtResultados.ReadOnly = true;
            this.txtResultados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultados.Size = new System.Drawing.Size(567, 500);
            this.txtResultados.TabIndex = 2;
            // 
            // lblNodoInicio
            // 
            this.lblNodoInicio.AutoSize = true;
            this.lblNodoInicio.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblNodoInicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNodoInicio.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblNodoInicio.Location = new System.Drawing.Point(149, 48);
            this.lblNodoInicio.Name = "lblNodoInicio";
            this.lblNodoInicio.Size = new System.Drawing.Size(114, 20);
            this.lblNodoInicio.TabIndex = 1;
            this.lblNodoInicio.Text = "Nodo inicial:";
            // 
            // numericNodoInicio
            // 
            this.numericNodoInicio.Location = new System.Drawing.Point(292, 48);
            this.numericNodoInicio.Maximum = new decimal(new int[] {
            13,
            0,
            0,
            0});
            this.numericNodoInicio.Name = "numericNodoInicio";
            this.numericNodoInicio.Size = new System.Drawing.Size(163, 22);
            this.numericNodoInicio.TabIndex = 0;
            // 
            // panelControles
            // 
            this.panelControles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelControles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControles.Location = new System.Drawing.Point(569, 0);
            this.panelControles.Name = "panelControles";
            this.panelControles.Size = new System.Drawing.Size(453, 662);
            this.panelControles.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 662);
            this.Controls.Add(this.panelControles);
            this.Controls.Add(this.panelGrafo);
            this.Name = "Form1";
            this.Text = "Simulador de Grafos - BFS y DFS";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelGrafo.ResumeLayout(false);
            this.panelGrafo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericNodoInicio)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGrafo;
        private System.Windows.Forms.NumericUpDown numericNodoInicio;
        private System.Windows.Forms.Panel panelControles;
        private System.Windows.Forms.Button btnDFS;
        private System.Windows.Forms.Button btnBFS;
        private System.Windows.Forms.Label lblNodoInicio;
        private System.Windows.Forms.TextBox txtResultados;
    }
}