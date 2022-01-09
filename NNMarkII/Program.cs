 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNMarkII
{
    class Program
    {
        static Driver driver = new Driver();
        static List<double> dataList = new List<double>();
 
        //---------------------------------------------------------------------
        static void Main(string[] args)
        {

            string path = Environment.CurrentDirectory + @"\test\";
            driver.BrowseForFiles(path, "*.txt");
            int maxIteration = 6000;
            //driver.ReadWeights();
            //////driver.displayCharcterset();
            //for (int i = 0; i < maxIteration; i++)
            //{
            //    if (i % 100 == 0)
            //    {
            //        Console.WriteLine(i + " out of " + maxIteration);
            //        Console.WriteLine(DateTime.Now);
            //        driver.WriteWeights();
            //    }
            //    driver.StartTrainingProcess();
            //}
            //driver.WriteWeights();
            driver.ReadWeights();

            driver.StartAutoTestingProcess();
            Console.WriteLine("Process Completed...");
            Console.ReadKey();
            
            //WeightsToArray.GenerateClassFile(0, new float[] { 0, 2, 5, 4, 8, 7, 9, 8, 7, 5, 4, 6, 1, 3, 2, 1, 0 });
            
           
        }
    }
}
