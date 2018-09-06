using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadania {
    public class Application {
        public static int Main(string[] args) {

            //CHOOSE YOUR GENERATOR
            //Generator generator = new RandomGenerator();
            //generator.PassParameter(5);
            Generator generator = new DataFromFile();
            generator.PassParameter("input.txt");

            Analizator analizator = new Analizator();
            analizator.SetGenerator(generator);

            Console.WriteLine("average:" + analizator.CalculateAverage());
            Console.WriteLine("deviation:" + analizator.CalculateDeviation());

            //saveToFile:
            analizator.SaveDataToFile("numbers.txt");
            //save sorted:
            analizator.SortToNewFile("sortedNumbers.txt");

            Console.Read();

            return 0;
        }


    }
}
