using System;
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
        List<StreamWriter> usuarios = new List<StreamWriter>();
        List<string> lista = new List<string>();
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
            string usuario;
            string mensaje;
            Socket cliente = (Socket)socket;
            IPEndPoint ieCliente = (IPEndPoint)cliente.RemoteEndPoint;
            Console.WriteLine("Connected with client {0} at port {1}",
            ieCliente.Address, ieCliente.Port);
            using (NetworkStream ns = new NetworkStream(cliente))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {
                usuarios.Add(sw);
                string welcome = "Bienvenido al chatroom! Introduce tu nombre de usuario.";
                sw.WriteLine(welcome);
                sw.Flush();
                usuario = sr.ReadLine();
                if (usuario != null)
                {
                    lista.Add($"{usuario}@{ieCliente.Address}");
                    Console.WriteLine("{0}@{1} se ha conectado al chat!",
                    usuario, ieCliente.Address);

                    sw.WriteLine("{0}@{1} se ha conectado al chat!",
                    usuario, ieCliente.Address);
                    sw.Flush();

                    foreach (var usuarioChat in usuarios)
                    {
                        if (usuarioChat != sw)
                        {
                            usuarioChat.WriteLine("{0}@{1} se ha conectado al chat!", usuario, ieCliente.Address);
                            usuarioChat.Flush();
                        }


                    }
                }
                else
                {
                    cliente.Close();
                }
                while (true)
                {
                    try
                    {
                        mensaje = sr.ReadLine();
                        if (mensaje != null)
                        {
                            if (mensaje=="#exit")
                            {
                                lista.Remove($"{usuario}@{ieCliente.Address}");
                                cliente.Close();

                            } else if (mensaje=="#list")
                            {
                                foreach (string usuarioLista in lista)
                                {
                                    sw.WriteLine(usuarioLista);
                                    sw.Flush();
                                }
                            } else
                            {
                                Console.WriteLine(mensaje);
                                foreach (var usuarioChat in usuarios)
                                {
                                    if (usuarioChat != sw)
                                    {
                                        usuarioChat.WriteLine(mensaje);
                                        usuarioChat.Flush();
                                    }


                                }
                            }
                            
                        } else
                        {
                            lista.Remove($"{usuario}@{ieCliente.Address}");
                            cliente.Close();

                        }
                    }
                    catch (IOException)
                    {

                        lista.Remove($"{usuario}@{ieCliente.Address}");
                        cliente.Close();
                        break;
                    }
                }
                Console.WriteLine("Finished connection with {0}:{1}",
                ieCliente.Address, ieCliente.Port);

                foreach (var usuarioChat in usuarios)
                {
                    if (usuarioChat != sw)
                    {
                        usuarioChat.WriteLine("Finished connection with {0}@{1}",usuario, ieCliente.Address);
                        usuarioChat.Flush();
                    }


                }
                cliente.Close();
                usuarios.Remove(sw);
            } 
            cliente.Close();
        }


    }

}

