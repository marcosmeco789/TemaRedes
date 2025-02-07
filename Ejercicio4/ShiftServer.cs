using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ejercicio4
{
    internal class ShiftServer
    {
        Socket s;
        bool activo = true;
        string[] users;
        List<string> waitQueue = new List<string>();
        List<string> userWaitNames = new List<string>();

        public void ReadNames(string nombreArchivoRead)
        {
            try
            {
                using (StreamReader sr = new StreamReader(Environment.GetEnvironmentVariable("userprofile") + "\\" + nombreArchivoRead))
                {
                    string texto = sr.ReadLine();
                    users = texto.Split(';');

                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is FileNotFoundException || ex is DirectoryNotFoundException || ex is IOException)
            {
                Console.WriteLine("Error en la lectura del archivo!");
            }

        }

        public int ReadPin()
        {
            string contraseñaString = "";
            int contraseña;
            try
            {
                using (BinaryReader br = new BinaryReader(new FileStream(Environment.GetEnvironmentVariable("userprofile") + "\\pin.bin", FileMode.Open)))
                {
                    contraseñaString = br.ReadString();
                }

                if (contraseñaString.Length != 4)
                {
                    return -1;
                }
                else
                {
                    contraseña = int.Parse(contraseñaString);
                    return contraseña;
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is NotSupportedException || ex is ArgumentNullException || ex is SecurityException ||
            ex is FileNotFoundException || ex is IOException || ex is DirectoryNotFoundException || ex is PathTooLongException || ex is ArgumentOutOfRangeException)
            {
                return -1;

            }

        }

        public void init()
        {
            bool openPort = false;
            int port = 31416;
            string linea;

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


                    try
                    {
                        using (StreamReader srLista = new StreamReader(Environment.GetEnvironmentVariable("userprofile") + "\\listaEspera.txt"))
                        {

                            while ((linea = srLista.ReadLine()) != null)
                            {
                                if (!waitQueue.Contains(linea))
                                {
                                    waitQueue.Add(linea);
                                }
                            }
                        }

                        foreach (string contenidoWaitQueue in waitQueue)
                        {
                            string[] usuarioSplit;
                            usuarioSplit = contenidoWaitQueue.Split(' ');
                            userWaitNames.Add(usuarioSplit[0]);
                        }
                    }
                    catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is FileNotFoundException ||
                           ex is DirectoryNotFoundException || ex is IOException)
                    {

                    }

                }
                catch (SocketException e)
                {
                    openPort = false;
                    if (port == 31416)
                    {
                        port = 1024;
                    }
                    else
                    {
                        port++;
                    }

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

        public void admin(StreamWriter sw, StreamReader sr, Socket cliente, Socket s)
        {
            string mensaje;
            string[] partes;
            int pin;
            bool pararBucle = false;
            bool parseExitoso;
            sw.WriteLine("Bienvenido a la consola de administrador. Los comandos disponibles son 'del pos', 'chpin pin', 'exit' y 'shutdown'");
            sw.Flush();

            while (!pararBucle)
            {
                mensaje = sr.ReadLine();
                if (!string.IsNullOrEmpty(mensaje))
                {
                    partes = mensaje.Split(' ');

                    switch (partes[0])
                    {
                        case "del":
                            if (partes.Length > 1)
                            {
                                try
                                {
                                    waitQueue.RemoveAt(int.Parse(partes[1]));
                                    userWaitNames.RemoveAt(int.Parse(partes[1]));
                                    sw.WriteLine("OK");
                                    sw.Flush();
                                }
                                catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                                {
                                    sw.WriteLine("Error al eliminar");
                                    sw.Flush();
                                }

                            }
                            break;

                        case "chpin":
                            if (partes.Length > 1)
                            {
                                parseExitoso = int.TryParse(partes[1], out pin);
                                if (partes[1].Length >= 4 && parseExitoso)
                                {
                                    try
                                    {
                                        using (BinaryWriter bw = new BinaryWriter(new FileStream(Environment.GetEnvironmentVariable("userprofile") + "\\pin.bin", FileMode.Create)))
                                        {
                                            bw.Write(pin.ToString());
                                        }
                                        sw.WriteLine("Pin establecido correctamente");
                                        sw.Flush();
                                    }

                                    catch (Exception ex) when (ex is ArgumentException || ex is NotSupportedException || ex is ArgumentNullException || ex is SecurityException ||
                                    ex is FileNotFoundException || ex is IOException || ex is DirectoryNotFoundException || ex is PathTooLongException || ex is ArgumentOutOfRangeException)
                                    {
                                        sw.WriteLine("Error al establecer el pin");
                                        sw.Flush();
                                    }


                                } else
                                {
                                    sw.WriteLine("Error al establecer el pin");
                                    sw.Flush();
                                }
                            }


                            break;

                        case "exit":
                            pararBucle = true;
                            cliente.Close();
                            break;

                        case "shutdown":
                            try
                            {
                                pararBucle = true;
                                activo = false;

                                using (StreamWriter swLista = new StreamWriter(Environment.GetEnvironmentVariable("userprofile") + "\\listaEspera.txt", false))
                                {
                                    foreach (string usuarioEnEspera in waitQueue)
                                    {
                                        swLista.WriteLine(usuarioEnEspera);
                                        swLista.Flush();
                                    }
                                }

                                cliente.Close();
                                s.Close();
                            }
                            catch (Exception ex) when (ex is UnauthorizedAccessException || ex is ArgumentException || ex is ArgumentNullException ||
                                   ex is DirectoryNotFoundException || ex is IOException || ex is PathTooLongException || ex is SecurityException)
                            {


                            }

                            break;

                        default:
                            break;
                    }

                }


            }
        }

        public void hiloCliente(object socket)
        {
            string mensaje;
            string usuario;
            int contraseña;



            Socket cliente = (Socket)socket;
            IPEndPoint ieCliente = (IPEndPoint)cliente.RemoteEndPoint;
            Console.WriteLine("Connected with client {0} at port {1}",
            ieCliente.Address, ieCliente.Port);



            using (NetworkStream ns = new NetworkStream(cliente))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {
                try
                {
                    string welcome = "Bienvenido al servidor de turnos! Introduce tu nombre de usuario.";
                    sw.WriteLine(welcome);
                    sw.Flush();
                    usuario = sr.ReadLine();
                    if (usuario != null)
                    {
                        ReadNames("usuarios.txt");
                        if (usuario == "admin")
                        {
                            sw.WriteLine("Introduce la contraseña de admin");
                            sw.Flush();
                            mensaje = sr.ReadLine();

                            int.TryParse(mensaje, out contraseña);
                            if (ReadPin() == -1 && contraseña == 1234)
                            {
                                admin(sw, sr, cliente, s);
                            }
                            else if ((ReadPin() == -1 && contraseña != 1234))
                            {
                                cliente.Close();
                            }
                            else if (ReadPin() == contraseña)
                            {
                                admin(sw, sr, cliente, s);
                            }
                            else
                            {
                                cliente.Close();
                            }
                        }
                        else if (users.Contains(usuario))
                        {
                            sw.WriteLine("En la lista! Los comandos disponibles son list y add\n");
                            sw.Flush();
                            mensaje = sr.ReadLine();

                            if (mensaje == "list")
                            {
                                foreach (string usuarioEnLista in waitQueue)
                                {
                                    sw.WriteLine(usuarioEnLista);
                                    sw.Flush();
                                }
                            }
                            else if (mensaje == "add")
                            {
                                if (!userWaitNames.Contains(usuario))
                                {
                                    userWaitNames.Add(usuario);
                                    waitQueue.Add(usuario + " - " + DateTime.Now.ToString());
                                    sw.WriteLine("OK");
                                    sw.Flush();
                                }
                                else
                                {
                                    sw.WriteLine("Ya estas en la lista!");
                                    sw.Flush();
                                }
                            }
                            else
                            {
                                cliente.Close();
                            }
                        }
                        else
                        {
                            sw.WriteLine("Usuario desconocido");
                            sw.Flush();
                        }
                    }
                }
                catch (IOException)
                {
                    // Manejo de excepciones
                }
                cliente.Close();
                Console.WriteLine("Se ha desconectado: " + ieCliente.Address, ieCliente.Port);
            }
        }
    }
}
