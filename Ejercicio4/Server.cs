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
    internal class Server
    {
        Socket s;
        bool activo = true;
        string[] users;
        List<string> waitQueue = new List<string>();

        public void ReadNames(string nombreArchivoRead)
        {
            try
            {
                using (StreamReader sr = new StreamReader(Environment.GetEnvironmentVariable("userprofile") +"\\" +nombreArchivoRead))
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

        public int ReadPin(string nombreArchivoPin)
        {
            string contraseñaString = "";
            int contraseña;
            try
            {
                using (BinaryReader br = new BinaryReader(new FileStream(Environment.GetEnvironmentVariable("userprofile") + "\\" + nombreArchivoPin, FileMode.Open)))
                {

                    contraseñaString = br.ReadString();

                }

                if (contraseñaString.Length != 4)
                {
                    return -1;
                }else
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

            }
        }
    }
}
