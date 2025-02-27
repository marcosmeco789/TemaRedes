using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio5
{
    internal class Leer : BinaryReader
    {
        public Leer(Stream str) : base(str)
        {
        }

        public Record ReadRecord()
        {
            try
            {
                string nombre = ReadString();
                int segundos = ReadInt32();
                return new Record(nombre, segundos);
            }
            catch
            {
                BaseStream.Close();
                return null;
            }
        }
    }
}
