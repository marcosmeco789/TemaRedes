using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Ejercicio5
{
    internal class ShiftServer
    {
        static readonly private object l = new object();
        Socket s;
        bool activo = true;
        List<string> palabras = new List<string>();
        Random random = new Random();



        public void leerPalabras(Socket cliente)
        {
            try
            {
                using (StreamReader sr = new StreamReader(Environment.GetEnvironmentVariable("homepath") + "\\palabras.txt"))
                {
                    string cadena = sr.ReadLine();
                    string[] palabrasSplit = cadena.ToUpper().Split(';');
                    foreach (string palabra in palabrasSplit)
                    {
                        palabras.Add(palabra);
                    }
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is FileNotFoundException || ex is DirectoryNotFoundException || ex is IOException)
            {
                cliente.Close();
            }

        }

        public void leerRecords()
        {
            Record r = new Record();

            FileStream fs = new FileStream(Environment.GetEnvironmentVariable("homepath") + "\\records.bin", FileMode.Open);
            using (BinaryReader br = new BinaryReader(fs))
            {

            }
        }

        public void getWord(StreamWriter sw)
        {
            int tamaño = palabras.Count();

            int aleatorio = random.Next(0, tamaño);

            sw.WriteLine(palabras[aleatorio]);
            sw.Flush();

            
        }

        public void sendWord(StreamWriter sw, string palabraAñadir)
        {
            palabras.Add(palabraAñadir);


            try
            {
                using (StreamWriter strw = new StreamWriter(Environment.GetEnvironmentVariable("homepath") + "\\records.bin"))
                {
                    foreach (string palabra in palabras)
                    {
                        strw.WriteLine(palabra);
                        strw.Flush();
                    }
                }
                sw.WriteLine("OK");
                sw.Flush();
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException || ex is ArgumentException || ex is ArgumentNullException || 
                  ex is DirectoryNotFoundException || ex is PathTooLongException || ex is IOException)
            {
                sw.WriteLine("ERROR");
                sw.Flush();

            }

            
        }

        public void getRecords(StreamWriter sw)
        {

        }

        public void init()
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

            string mensaje;
            Socket cliente = (Socket)socket;
            IPEndPoint ieCliente = (IPEndPoint)cliente.RemoteEndPoint;
            Console.WriteLine("Connected with client {0} at port {1}",
            ieCliente.Address, ieCliente.Port);

            leerPalabras(cliente);

            using (NetworkStream ns = new NetworkStream(cliente))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {
                sw.WriteLine("Comandos disponibles: getword; sendword []; getrecords y sendrecord []");
                sw.Flush();
                mensaje = sr.ReadLine();

                string[] mensajeSplit = mensaje.Split(' ');

                if (mensaje != null)
                {
                    switch (mensajeSplit[0])
                    {
                        case "getword":
                            getWord(sw);
                            break;

                        case "sendword":
                            if (mensajeSplit.Length == 1)
                            {
                                sendWord(sw, mensajeSplit[1]);
                            }
                            break;

                        case "getrecords":
                            break;

                        case "sendrecord":
                            if (mensajeSplit.Length == 1)
                            {

                            }
                            break;

                        default:
                            break;
                    }
                }

            }
        }
    }
}


