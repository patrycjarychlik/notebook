using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadania {
    class Writer : StreamWriter {
        public Writer(string path) : base(path) {
        }


        public static Writer operator +(Writer stream, string x) {
            stream.WriteLine(x);
            return stream;
        }
    }
}
