using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadania {
    class Analizator {
        private string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private Generator generator;
        List<double> numbers;

        public void SetGenerator(Generator generator) {
            this.generator = generator;
            numbers = generator.GetData();
        }

        public double CalculateAverage() {
            return numbers.Average();
        }

        public double CalculateDeviation() {
            double ret = 0;
            if (numbers.Count() > 0) {
                double avg = numbers.Average();
                double sum = numbers.Sum(d => Math.Pow(d - avg, 2));
                ret = Math.Sqrt((sum) / (numbers.Count() - 1));
            }
            return ret;
        }

        public void SortToNewFile(String outputFileName) {
            numbers.Sort();
            using (StreamWriter outputFile = new StreamWriter(myDocuments + @"\" + outputFileName)) {
                foreach (double number in numbers)
                    outputFile.WriteLine(number);
            }

        }

        public void SaveDataToFile(String fileName) {
            Writer outputFile = new Writer(myDocuments + @"\" + fileName);
            foreach (double number in numbers) {
                outputFile += number.ToString();
            }
            outputFile.Close();
        }

    }
}
