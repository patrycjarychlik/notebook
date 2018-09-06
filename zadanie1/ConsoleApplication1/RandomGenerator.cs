using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Zadania
{
    class RandomGenerator : Generator {

        private List<double> numbers = new List<double>();
        private int parameter = 0;


        public List<double> GenerateNumbers() {
            Random rand = new Random();
            for (int i = 0; i < parameter; i++) {
                numbers.Add(rand.NextDouble());
            }
            //CheckNumbersByPrintToScreen();
            return numbers;
        }

        public void CheckNumbersByPrintToScreen() {
            foreach (double number in numbers) {
                Console.WriteLine(number);
            }
        }


        public List<double> GetData() {
            return GenerateNumbers();
        }

        public void PassParameter(object obj) {
            if (obj.GetType() == typeof(int)) {
                parameter = (int)obj;
            }
        }
    }
}
