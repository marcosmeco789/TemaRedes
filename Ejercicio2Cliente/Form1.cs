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
            cargarConfiguracion();
        }

        public void cargarConfiguracion()
        {
            using (StreamReader sr = new StreamReader(Environment.GetEnvironmentVariable("userprofile") + "\\configuracionCliente.txt"))
            {
                textBox1.Text = sr.ReadLine();
                IP_SERVER = sr.ReadLine();
                PUERTO = int.Parse(sr.ReadLine());
            }
        }

        public void guardarConfiguracion()
        {
            using (StreamWriter sw = new StreamWriter(Environment.GetEnvironmentVariable("userprofile") + "\\configuracionCliente.txt"))
            {
                sw.WriteLine(textBox1.Text);
                sw.Flush();

                sw.WriteLine(IP_SERVER);
                sw.Flush();

                sw.WriteLine(PUERTO);
                sw.Flush();

            }
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
                sr.ReadLine();

                if (((Button)sender).Tag.ToString() == "add")
                {
                    sw.WriteLine(textBox1.Text);
                    sw.Flush();
                    msg = sr.ReadLine();

                    if (msg != "Usuario desconocido")
                    {
                        sr.ReadLine();
                        sw.WriteLine("add");
                        sw.Flush();
                        msg = sr.ReadLine();
                        MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    listBox1.Items.Clear();
                    sw.WriteLine(textBox1.Text);
                    sw.Flush();
                    msg = sr.ReadLine();

                    if (msg != "Usuario desconocido")
                    {
                        sr.ReadLine();
                        sw.WriteLine("list");
                        sw.Flush();
                        while (msg!=null)
                        {
                            msg = sr.ReadLine();

                            if (msg != null)
                            {
                                listBox1.Items.Add(msg);
                            }
                            
                        }


                    }
                    else
                    {
                        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cambiarIPPuertoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(IP_SERVER, PUERTO);
            form2.ShowDialog();

            IP_SERVER = form2.Ip;
            PUERTO = form2.Puerto;
        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            conexion(sender);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            guardarConfiguracion();
            
        }
    }
}
