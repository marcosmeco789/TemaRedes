﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Ejercicio3
{
    internal class Server
    {
        Socket s;
        bool activo = true;
        public void initServer()
        {
            bool openPort = false;
            int port = 31416;

            while (!openPort)
            {
                try
                {
                    s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint ie = new IPEndPoint(IPAddress.Any, port);
                    s.Bind(ie);
                    s.Listen(10);
                    Console.WriteLine("Server waiting at port {0}", ie.Port);
                    openPort = true;

                }
                catch (SocketException e)
                {
                    openPort = false;
                    port++;

                }
            }

            while (activo)
            {
                try
                {
                    Socket cliente = s.Accept();
                    Thread hilo = new Thread(hiloCliente);
                    hilo.Start(cliente);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("SocketException: {0}", ex.Message);

                }
            }
        }

        public void hiloCliente(object socket)
        {
            string mensaje;
            Socket cliente = (Socket)socket;
            IPEndPoint ieCliente = (IPEndPoint)cliente.RemoteEndPoint;
            Console.WriteLine("Connected with client {0} at port {1}",
            ieCliente.Address, ieCliente.Port);
            using (NetworkStream ns = new NetworkStream(cliente))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {
                string welcome = "Wellcome to this great Server";
                sw.WriteLine(welcome);
                sw.Flush();
                while (true) //Esto normalmente dependerá de una bandera
                {
                    try
                    {
                        mensaje = sr.ReadLine();
                        sw.WriteLine(mensaje);
                        sw.Flush();
                        //El mensaje es null al cerrar
                        if (mensaje != null)
                        {
                            Console.WriteLine("{0} says: {1}",
                            ieCliente.Address, mensaje);
                        }
                    }
                    catch (IOException)
                    {
                        //Salta al acceder al socket
                        //y no estar permitido
                        break;
                    }
                }
                Console.WriteLine("Finished connection with {0}:{1}",
                ieCliente.Address, ieCliente.Port);
            }
            cliente.Close();
        }


    }

}

