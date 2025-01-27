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
    internal class Program
    {

        static void Main(string[] args)
        {
            Server s = new Server();

            s.initServer();
        }

    }
}
