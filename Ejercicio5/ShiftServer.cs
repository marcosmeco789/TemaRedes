using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security;

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
                    if (cadena != null)
                    {
                        string[] palabrasSplit = cadena.Split(';');

                        foreach (string palabra in palabrasSplit)
                        {
                            if (!palabras.Contains(palabra))
                            {
                                palabras.Add(palabra);
                            }
                        }
                    }


                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is FileNotFoundException || ex is DirectoryNotFoundException || ex is IOException)
            {
                cliente.Close();
            }

        }

        public void getWord(StreamWriter sw)
        {
            int tamaño = palabras.Count();

            int aleatorio = random.Next(0, tamaño);

            if (palabras.Count > 0)
            {
                sw.WriteLine(palabras[aleatorio].ToUpper());
                sw.Flush();
            }



        }

        public void sendWord(StreamWriter sw, string palabraAñadir)
        {

            palabras.Add(palabraAñadir);

            try
            {
                using (StreamWriter strw = new StreamWriter(Environment.GetEnvironmentVariable("homepath") + "\\palabras.txt", false))
                {
                    string cadena = "";
                    foreach (string palabra in palabras)
                    {
                        cadena = cadena + palabra + ";";
                    }
                    strw.Write(cadena.Trim(';'));
                    strw.Flush();





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


            FileStream fs = new FileStream(Environment.GetEnvironmentVariable("homepath") + "\\records.bin", FileMode.Open);

            Leer r = new Leer(fs);

            sw.WriteLine(r.ReadRecord().ToString());
            sw.Flush();


            r.Close();
            fs.Close();
        }

        public void sendRecord(StreamWriter sw, string recordAñadir)
        {
            string nombre = "test";
            int segundos = 10;
            Record r = new Record(nombre, segundos);
            List<Record> records = new List<Record>();

            try
            {
                FileStream fs = new FileStream(Environment.GetEnvironmentVariable("homepath") + "\\records.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite);

                Escribir e = new Escribir(fs);
                Leer l = new Leer(fs);
                bool continuar = true;
                int minSegundos;

                while (continuar)
                {
                    Record record = l.ReadRecord();
                    if (record == null)
                    {
                        continuar = false;
                    }
                    else
                    {
                        records.Add(l.ReadRecord());
                    }
                }

                minSegundos = records[0].Segundos;
                foreach (Record item in records)
                {
                    if (item.Segundos<minSegundos)
                    {
                        minSegundos = item.Segundos;
                    }
                }

                foreach (Record item in records)
                {
                    if (item.Segundos == minSegundos)
                    {
                        
                    }
                }




                if (records.Count<3)
                {
                    e.Write(r);
                    
                } else
                {
                    if (r.Segundos>minSegundos)
                    {
                        
                    }
                }


                e.Close();





            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException || ex is NotSupportedException || ex is FileNotFoundException ||
                   ex is SecurityException || ex is DirectoryNotFoundException || ex is UnauthorizedAccessException || ex is PathTooLongException || ex is ArgumentOutOfRangeException)
            {
                sw.WriteLine("Error en el archivo");
            }




        }


        // puede ser hardcodeada, da un poco igual
        public void closeServer(StreamWriter sw, string clave)
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


            using (NetworkStream ns = new NetworkStream(cliente))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {
                try
                {
                    leerPalabras(cliente);
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
                                if (mensajeSplit.Length == 2)
                                {
                                    sendWord(sw, mensajeSplit[1]);
                                }
                                break;

                            case "getrecords":
                                getRecords(sw);
                                break;

                            case "sendrecord":
                                if (mensajeSplit.Length == 2)
                                {
                                    sendRecord(sw, mensajeSplit[1]);
                                }
                                break;

                            case "closeserver":
                                if (mensajeSplit.Length == 2)
                                {
                                    closeServer(sw, mensajeSplit[1]);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
                catch (IOException)
                {


                }
                cliente.Close();
                Console.WriteLine("Se ha desconectado: " + ieCliente.Address, ieCliente.Port);


            }
        }
    }
}


