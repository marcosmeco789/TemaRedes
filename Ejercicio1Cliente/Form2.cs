using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio1Cliente
{
    public partial class Form2 : Form
    {

        public string Ip { get; set; }
        public int Puerto { get; set; }

        public Form2(string ip, int puerto)
        {
            InitializeComponent();

            txtIp.Text = ip;
            txtPuerto.Text = puerto.ToString();

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Ip = txtIp.Text;
            Puerto = int.Parse(txtPuerto.Text);
        }
    }
}
