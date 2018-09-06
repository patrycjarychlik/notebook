using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Zadania
{
    class DataFromFile : Generator { 
        string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string parameter = "numbers.txt";

        public List<double> GetData() {
            List<double> numbers = new List<double>();

            using (System.IO.StreamReader inputFile = File.OpenText(myDocuments + @"\" + parameter)) {
                while (!inputFile.EndOfStream) {
                    numbers.Add(double.Parse(inputFile.ReadLine()));
                }
                return numbers;
            }

        }

        public void PassParameter(object obj) {
            if (obj.GetType() == typeof(string)) {
                parameter = (string)obj;
            }
        }
    }
}
