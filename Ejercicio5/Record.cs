using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio5
{
    internal class Record
    {
        public string Nombre { set; get; }
        public int Segundos { set; get; }

        public Record(string nombre, int segundos)
        {
            Nombre = nombre;
            Segundos = segundos;
        }

        public override string ToString()
        {
            return "Nombre: " + Nombre + " | Segundos: " + Segundos;
        }
    }
}







