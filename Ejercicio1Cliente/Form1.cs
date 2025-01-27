﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio1Cliente
{
    public partial class Form1 : Form// Comprobaciones ip y puerto y abreeviar coido igual en los botones
    {
        static string IP_SERVER = "127.0.0.1";
        static int PUERTO = 31416;
        public Form1()
        {
            InitializeComponent();
        }

        string boton = "";
        public void conexion()
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
                // Leemos mensaje de bienvenida ya que es lo primero que envía el servidor
                msg = sr.ReadLine();

                switch (boton)
                {
                    case "time":
                        sw.WriteLine("time");
                        sw.Flush();
                        msg = sr.ReadLine();
                        MessageBox.Show("Son las: " + msg, "Comando Time", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "date":
                        sw.WriteLine("date");
                        sw.Flush();
                        msg = sr.ReadLine();
                        MessageBox.Show("Hoy es: " + msg, "Comando Date", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "all":
                        sw.WriteLine("all");
                        sw.Flush();
                        msg = sr.ReadLine();
                        MessageBox.Show(msg, "Comando All", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "close":
                        sw.WriteLine("close");
                        sw.Flush();
                        sr.ReadLine();

                        sw.WriteLine(textBox1.Text);
                        sw.Flush();
                        msg = sr.ReadLine();
                        if (msg == "Cerrando")
                        {
                            MessageBox.Show("Servidor cerrado con exito!", "Cerrado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                        {
                            MessageBox.Show("Contraseña incorrecta!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        break;

                    default:
                        break;
                }






            }


        }

        private void btnTime_Click(object sender, EventArgs e)
        {
            boton = "time";
            conexion();
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            boton = "date";
            conexion();
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            boton = "all";
            conexion();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            boton = "close";
            conexion();
        }

        private void cambiarIPPuertoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2( IP_SERVER,  PUERTO);
            form2.ShowDialog();

             IP_SERVER = form2.Ip;
             PUERTO = form2.Puerto;
        }
    }
}