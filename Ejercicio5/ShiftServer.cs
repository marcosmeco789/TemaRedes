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
        List<Record> records = new List<Record>();
        Random random = new Random();
        int minSegundos;



        public void leerPalabras(Socket cliente)
        {
            string filePath = Environment.GetEnvironmentVariable("homepath") + "\\palabras.txt";

            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }

                using (StreamReader sr = new StreamReader(filePath))
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


        public void leerRecords(StreamWriter sw)
        {
            string filePath = Environment.GetEnvironmentVariable("homepath") + "\\records.bin";

            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    Leer r = new Leer(fs);
                    Record obtenerRecord;
                    while ((obtenerRecord = r.ReadRecord()) != null)
                    {
                        records.Add(obtenerRecord);
                    }
                }
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException || ex is NotSupportedException || ex is FileNotFoundException ||
                   ex is SecurityException || ex is DirectoryNotFoundException || ex is UnauthorizedAccessException || ex is PathTooLongException || ex is ArgumentOutOfRangeException)
            {
                sw.WriteLine("Error al leer los records");
                sw.Flush();
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
            try
            {
                if (File.Exists(Environment.GetEnvironmentVariable("homepath") + "\\records.bin"))
                {
                    using (FileStream fs = new FileStream(Environment.GetEnvironmentVariable("homepath") + "\\records.bin", FileMode.Open, FileAccess.Read))
                    {
                        Leer l = new Leer(fs);
                        Record record;
                        while ((record = l.ReadRecord()) != null)
                        {
                            sw.WriteLine(record.ToString());
                        }
                        sw.Flush();
                    }
                }
                else
                {
                    sw.WriteLine("No hay records disponibles.");
                    sw.Flush();
                }
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException || ex is NotSupportedException || ex is FileNotFoundException ||
                   ex is SecurityException || ex is DirectoryNotFoundException || ex is UnauthorizedAccessException || ex is PathTooLongException || ex is ArgumentOutOfRangeException)
            {
                sw.WriteLine("Error al leer los records");
                sw.Flush();
            }
        }

        public void sendRecord(StreamWriter sw, string nombre, int segundos)
        {
            Record r = new Record(nombre, segundos);

            try
            {
                List<Record> recordsExistentes = new List<Record>();
                if (File.Exists(Environment.GetEnvironmentVariable("homepath") + "\\records.bin"))
                {
                    using (FileStream fs = new FileStream(Environment.GetEnvironmentVariable("homepath") + "\\records.bin", FileMode.Open, FileAccess.Read))
                    {
                        Leer l = new Leer(fs);
                        Record recordExistente;
                        while ((recordExistente = l.ReadRecord()) != null)
                        {
                            recordsExistentes.Add(recordExistente);
                        }
                    }
                }

                bool recordGuardado = false;

                if (recordsExistentes.Count < 3)
                {
                    recordsExistentes.Add(r);
                    recordGuardado = true;
                }
                else
                {
                    Record maxRecord = obtenerMaximo(recordsExistentes);
                    if (r.Segundos < maxRecord.Segundos)
                    {
                        recordsExistentes.Remove(maxRecord);
                        recordsExistentes.Add(r);
                        recordGuardado = true;
                    }
                }

                if (recordGuardado)
                {
                    using (FileStream fs = new FileStream(Environment.GetEnvironmentVariable("homepath") + "\\records.bin", FileMode.Create, FileAccess.Write))
                    {
                        Escribir e = new Escribir(fs);
                        foreach (var record in recordsExistentes)
                        {
                            e.Write(record);
                        }
                    }
                    sw.WriteLine("ACCEPT");
                    sw.Flush();
                }
                else
                {
                    sw.WriteLine("REJECT");
                    sw.Flush();
                }

            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException || ex is NotSupportedException || ex is FileNotFoundException ||
                   ex is SecurityException || ex is DirectoryNotFoundException || ex is UnauthorizedAccessException || ex is PathTooLongException || ex is ArgumentOutOfRangeException)
            {
                sw.WriteLine("Error en el archivo");
                sw.Flush();
            }
        }

        public Record obtenerMaximo(List<Record> records)
        {
            int maxSegundos = records[0].Segundos;
            Record maxRecord = records[0];
            foreach (Record item in records)
            {
                if (item.Segundos > maxSegundos)
                {
                    maxSegundos = item.Segundos;
                    maxRecord = item;
                }
            }
            return maxRecord;
        }


        public void closeServer(StreamWriter sw, string clave, Socket s)
        {
            string contraseña = "1234";
            if (clave == contraseña)
            {
                activo = false;
                sw.WriteLine("Servidor cerrado");
                sw.Flush();
                s.Close();
            }
            else
            {
                sw.WriteLine("Contraseña incorrecta");
                sw.Flush();
            }

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
                    leerRecords(sw);
                    sw.WriteLine("Comandos disponibles: getword; sendword [palabra]; getrecords ; sendrecord [nombre] [tiempo] y closeserver [contraseña]");
                    sw.Flush();
                    mensaje = sr.ReadLine();



                    if (mensaje != null)
                    {
                        string[] mensajeSplit = mensaje.Split(' ');

                        switch (mensajeSplit[0])
                        {
                            case "getword":
                                if (mensajeSplit.Length == 1)
                                {
                                    getWord(sw);
                                }
                                break;

                            case "sendword":
                                if (mensajeSplit.Length == 2)
                                {
                                    sendWord(sw, mensajeSplit[1]);
                                }
                                break;

                            case "getrecords":
                                if (mensajeSplit.Length == 1)
                                {
                                    getRecords(sw);
                                }
                                break;

                            case "sendrecord":
                                if (mensajeSplit.Length == 3)
                                {
                                    if (mensajeSplit.Length == 3)
                                    {
                                        if (int.TryParse(mensajeSplit[2], out int segundos))
                                        {
                                            sendRecord(sw, mensajeSplit[1], segundos);
                                        }
                                    }
                                }
                                break;

                            case "closeserver":
                                if (mensajeSplit.Length == 2)
                                {
                                    closeServer(sw, mensajeSplit[1], s);
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

