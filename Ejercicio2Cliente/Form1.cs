using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Ejercicio1Cliente;

namespace Ejercicio2Cliente
{
    public partial class Form1 : Form
    {
        static string IP_SERVER = "127.0.0.1";
        static int PUERTO = 31416;

        public Form1()
        {
            InitializeComponent();
        }

        public void conexion(object sender)
        {
            string msg;
            Console.WriteLine(IP_SERVER);
            // Indicamos servidor al que nos queremos conectar y puerto
            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(IP_SERVER), PUERTO);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // El cliente inicia la conexión haciendo petición con Connect
                server.Connect(ie);
            }
            catch (SocketException e)
            {
                MessageBox.Show("Conexion no establecida", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            IPEndPoint ieServer = (IPEndPoint)server.RemoteEndPoint;

            using (NetworkStream ns = new NetworkStream(server))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {

            }
        }

        private void cambiarIPPuertoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(IP_SERVER, PUERTO);
            form2.ShowDialog();

            IP_SERVER = form2.Ip;
            PUERTO = form2.Puerto;
        }

    }
}
