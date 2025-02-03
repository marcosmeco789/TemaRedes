using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Ejercicio3//Locks
{
    internal class Server
    {
        List<StreamWriter> usuarios = new List<StreamWriter>();
        List<string> lista = new List<string>();
        static readonly private object l = new object();
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
            bool bucleActivo = true;
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
                lock (l)
                {
                    usuarios.Add(sw);
                }
                string welcome = "Bienvenido al chatroom! Introduce tu nombre de usuario.";
                sw.WriteLine(welcome);
                sw.Flush();
                usuario = sr.ReadLine();
                if (usuario != null)
                {
                    lock (l)
                    {
                        lista.Add($"{usuario}@{ieCliente.Address}");
                    }
                    Console.WriteLine("{0}@{1} se ha conectado al chat!",
                    usuario, ieCliente.Address);

                    sw.WriteLine("{0}@{1} se ha conectado al chat!",
                    usuario, ieCliente.Address);
                    sw.Flush();

                    bucleUsuarios(sw, string.Format("{0}@{1} se ha conectado al chat!", usuario, ieCliente.Address));

                }
                else
                {
                    cliente.Close();
                }
                while (bucleActivo)
                {
                    try
                    {
                        mensaje = sr.ReadLine();
                        if (mensaje != null)
                        {
                            if (mensaje == "#exit")
                            {
                                lock (l)
                                {
                                    lista.Remove($"{usuario}@{ieCliente.Address}");
                                }
                                bucleActivo = false;

                            }
                            else if (mensaje == "#list")
                            {
                                bucleLista(sw);
                            }
                            else
                            {
                                Console.WriteLine(mensaje);
                                bucleUsuarios(sw, mensaje);
                            }

                        }
                        else
                        {
                            lock (l)
                            {
                                lista.Remove($"{usuario}@{ieCliente.Address}");
                            }
                            bucleActivo = false;

                        }
                    }
                    catch (IOException)
                    {
                        lock (l)
                        {
                            lista.Remove($"{usuario}@{ieCliente.Address}");
                        }
                        bucleActivo = false;
                        break;
                    }

                }
                Console.WriteLine("Finished connection with {0}:{1}",
                ieCliente.Address, ieCliente.Port);

                bucleUsuarios(sw, string.Format("Finished connection with {0}@{1}", usuario, ieCliente.Address));
                cliente.Close();
                lock (l)
                {
                    usuarios.Remove(sw);
                }
            }
            cliente.Close();
        }

        public void bucleUsuarios(StreamWriter sw, string mensaje)
        {
            lock (l)
            {
                foreach (var usuarioChat in usuarios)
                {
                    if (usuarioChat != sw)
                    {
                        usuarioChat.WriteLine(mensaje);
                        usuarioChat.Flush();
                    }


                }
            }

        }

        public void bucleLista(StreamWriter sw)
        {
            lock (l)
            {
                foreach (string usuarioLista in lista)
                {
                    sw.WriteLine(usuarioLista);
                    sw.Flush();
                }
            }
        }


    }

}

