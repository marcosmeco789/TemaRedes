using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Ejercicio1 
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
            Console.WriteLine("Connected with client {0} at port {1}", ieCliente.Address, ieCliente.Port);

            using (NetworkStream ns = new NetworkStream(cliente))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {
                string welcome = "Bienvenido! Los comandos permitidos son time, date, all y close";
                sw.WriteLine(welcome);
                sw.Flush();



                try
                {
                    mensaje = sr.ReadLine();
                    if (!string.IsNullOrEmpty(mensaje))
                    {
                        string[] partes = mensaje.Split(' ');
                        switch (partes[0])
                        {
                            case "time":
                                sw.WriteLine(DateTime.Now.ToString("HH:mm") + "\n");
                                sw.WriteLine("Pulsa enter para cerrar!");
                                sw.Flush();
                                cliente.Close();
                                sr.ReadLine();
                                break;

                            case "date":
                                sw.WriteLine(DateTime.Now.ToString("d") + "\n");
                                sw.WriteLine("Pulsa enter para cerrar!");
                                sw.Flush();
                                cliente.Close();
                                sr.ReadLine();
                                break;

                            case "all":
                                sw.WriteLine(DateTime.Now + "\n");
                                sw.WriteLine("Pulsa enter para cerrar!");
                                sw.Flush();
                                cliente.Close();
                                sr.ReadLine();
                                break;

                            case "close":
                                if (partes.Length > 1)
                                {
                                    string intento = partes[1];

                                    try
                                    {
                                        using (StreamReader srp = new StreamReader(Environment.GetEnvironmentVariable("programdata") + "\\serverPassword.txt"))
                                        {
                                            string password = srp.ReadLine();

                                            if (password == intento)
                                            {
                                                sw.WriteLine("Cerrando");
                                                sw.Flush();
                                                activo = false;
                                                s.Close();
                                            }
                                            else
                                            {
                                                sw.WriteLine("Contraseña incorrecta!\n");
                                                sw.Flush();
                                                cliente.Close();
                                                sr.ReadLine();
                                            }
                                        }
                                    }
                                    catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is FileNotFoundException || ex is DirectoryNotFoundException)
                                    {
                                        sw.WriteLine("Error en el archivo");
                                        sw.Flush();
                                        cliente.Close();
                                    }
                                }
                                else
                                {
                                    sw.WriteLine("Comando incorrecto, falta la contraseña!");
                                    sw.Flush();
                                    cliente.Close();
                                }
                                break;

                            default:
                                sw.WriteLine("Comando no encontrado, pulsa enter para cerrar!");
                                sw.Flush();
                                cliente.Close();
                                break;
                        }
                    }
                    else
                    {
                        sw.WriteLine("Mensaje vacío, conexión cerrada.");
                        sw.Flush();
                        cliente.Close();
                    }

                   
                }
                catch (IOException e)
                {
                    Console.WriteLine("Se ha desconectado: " + ieCliente.Address, ieCliente.Port);
                }
            }
        }

    }
}
