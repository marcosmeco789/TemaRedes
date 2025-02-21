using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio5
{
    internal class Escribir : BinaryWriter
    {
        public Escribir(Stream str) : base(str)
        {
        }

        public void Write(Record r)
        {
            base.Write(r.Nombre);
            base.Write(r.Segundos);
        }
    }
}
