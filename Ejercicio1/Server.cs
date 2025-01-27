using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Ejercicio1// Cumplior protocolo en close (close pass), puerto ocpupado. revisar archivo
{
    internal class Server
    {
        Socket s;
        bool activo = true;
        public void initServer()
        {
            try
            {
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ie = new IPEndPoint(IPAddress.Any, 31416);
                s.Bind(ie);
                s.Listen(10);
                Console.WriteLine("Server waiting at port {0}", ie.Port);

            }
            catch (SocketException e)
            {
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ie = new IPEndPoint(IPAddress.Any, 270);
                s.Bind(ie);
                s.Listen(10);
                Console.WriteLine("Server waiting at port {0}", ie.Port);

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

                    switch (mensaje)
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
                            sw.WriteLine("Para cerrar el servidor necesitas introducir la contraseña!");
                            sw.Flush();
                            string intento = sr.ReadLine();

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
                            


                            break;

                        default:
                            sw.WriteLine("Comando no encontrado, pulsa enter para cerrar!");
                            sw.Flush();
                            cliente.Close();
                            break;
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
