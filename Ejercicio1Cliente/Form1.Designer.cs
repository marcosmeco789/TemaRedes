namespace Ejercicio1Cliente
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
            this.btnTime = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.cambiarIPPuertoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cambiarIPPuertoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTime
            // 
            this.btnTime.Location = new System.Drawing.Point(90, 66);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(75, 23);
            this.btnTime.TabIndex = 0;
            this.btnTime.Tag = "time";
            this.btnTime.Text = "Time";
            this.btnTime.UseVisualStyleBackColor = true;
            this.btnTime.Click += new System.EventHandler(this.btnTime_Click);
            // 
            // btnDate
            // 
            this.btnDate.Location = new System.Drawing.Point(263, 66);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(75, 23);
            this.btnDate.TabIndex = 1;
            this.btnDate.Tag = "date";
            this.btnDate.Text = "Date";
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(90, 138);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(75, 23);
            this.btnAll.TabIndex = 2;
            this.btnAll.Tag = "all";
            this.btnAll.Text = "All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(263, 138);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Tag = "close";
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(470, 68);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 4;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(404, 71);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Password:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cambiarIPPuertoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(628, 24);
            this.menuStrip1.TabIndex = 6;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 242);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btnDate);
            this.Controls.Add(this.btnTime);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Cliente";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTime;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cambiarIPPuertoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cambiarIPPuertoToolStripMenuItem1;
    }
}

