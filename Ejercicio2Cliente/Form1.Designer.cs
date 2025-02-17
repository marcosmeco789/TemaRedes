namespace Ejercicio2Cliente
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.cambiarIPPuertoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cambiarIPPuertoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnAñadir = new System.Windows.Forms.Button();
            this.btnList = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cambiarIPPuertoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(584, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // cambiarIPPuertoToolStripMenuItem
            // 
            this.cambiarIPPuertoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cambiarIPPuertoToolStripMenuItem1});
            this.cambiarIPPuertoToolStripMenuItem.Name = "cambiarIPPuertoToolStripMenuItem";
            this.cambiarIPPuertoToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.cambiarIPPuertoToolStripMenuItem.Text = "Editar";
            // 
            // cambiarIPPuertoToolStripMenuItem1
            // 
            this.cambiarIPPuertoToolStripMenuItem1.Name = "cambiarIPPuertoToolStripMenuItem1";
            this.cambiarIPPuertoToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.cambiarIPPuertoToolStripMenuItem1.Text = "Cambiar IP/Puerto";
            this.cambiarIPPuertoToolStripMenuItem1.Click += new System.EventHandler(this.cambiarIPPuertoToolStripMenuItem1_Click);
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(38, 63);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 8;
            this.lblUsuario.Text = "Usuario:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(104, 60);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 9;
            // 
            // btnAñadir
            // 
            this.btnAñadir.Location = new System.Drawing.Point(32, 114);
            this.btnAñadir.Name = "btnAñadir";
            this.btnAñadir.Size = new System.Drawing.Size(75, 31);
            this.btnAñadir.TabIndex = 10;
            this.btnAñadir.Tag = "add";
            this.btnAñadir.Text = "ADD";
            this.btnAñadir.UseVisualStyleBackColor = true;
            this.btnAñadir.Click += new System.EventHandler(this.btnAñadir_Click);
            // 
            // btnList
            // 
            this.btnList.Location = new System.Drawing.Point(137, 114);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(75, 31);
            this.btnList.TabIndex = 11;
            this.btnList.Tag = "list";
            this.btnList.Text = "LIST";
            this.btnList.UseVisualStyleBackColor = true;
            this.btnList.Click += new System.EventHandler(this.btnAñadir_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(282, 60);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(243, 173);
            this.listBox1.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 284);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnList);
            this.Controls.Add(this.btnAñadir);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cambiarIPPuertoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cambiarIPPuertoToolStripMenuItem1;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnAñadir;
        private System.Windows.Forms.Button btnList;
        private System.Windows.Forms.ListBox listBox1;
    }
}

