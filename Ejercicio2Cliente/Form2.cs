using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio1Cliente
{
    public partial class Form2 : Form
    {

        public string Ip { get; set; }
        public int Puerto { get; set; }

        bool validoIp = true;
        bool validoPuerto = true;
        bool flag = false;

        string ipAux;
        string puertoAux;

        public Form2(string ip, int puerto)
        {
            InitializeComponent();

            txtIp.Text = ip;
            ipAux = ip;
            txtPuerto.Text = puerto.ToString();
            puertoAux = puerto.ToString();

        }

        public void sanitizar()
        {
            if (!IPAddress.TryParse(txtIp.Text, out _))
            {
                validoIp = false;
                MessageBox.Show("La IP no es válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Ip = txtIp.Text;
                validoIp = true;
            }


            if (!int.TryParse(txtPuerto.Text, out int puerto) || puerto < 0 || puerto > 65535)
            {
                validoPuerto = false;
                MessageBox.Show("El puerto no es válido. Debe estar entre 0 y 65535.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Puerto = puerto;
                validoPuerto = true;
            }
            flag = true;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            sanitizar();
            if (validoPuerto && validoIp)
            {
                MessageBox.Show("IP/Puerto cambiado exitosamente!", "Añadido", MessageBoxButtons.OK, MessageBoxIcon.Information);
             
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!flag || !validoIp || !validoPuerto)
            {
                Ip = ipAux;

                Puerto = int.Parse(puertoAux);
            }


        }
    }
}
