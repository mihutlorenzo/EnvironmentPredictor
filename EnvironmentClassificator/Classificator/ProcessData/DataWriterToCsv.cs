using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classificator.ProcessData
{
    public class DataWriterToCsv
    {
        //static readonly string _trainDataPath = Path.Combine(System.Environment.CurrentDirectory, "Data", "environment-sensors-train.csv");
        //static readonly string _testDataPath = Path.Combine(System.Environment.CurrentDirectory, "Data", "environment-sensors-test.csv");

        public static readonly string _trainDataPath = @"E:\Repositories\Github\EnvironmentPredictor\EnvironmentClassificator\EnvironmentClassificator\Data\environment-sensors-train.csv";
        static readonly string _testDataPath = @"E:\Repositories\Github\EnvironmentPredictor\EnvironmentClassificator\EnvironmentClassificator\Data\environment-sensors-test.csv";

        static readonly string firstLine = "Luminosity,Humidity,Temperature,NoiseLevel,EnvironmentState";

        public static void WriteTrainDataToCsv(IList<string> serialDataForTrain)
        {
            using (FileStream fileStream = new FileStream(_trainDataPath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    foreach (string data in serialDataForTrain)
                    {
                        writer.WriteLine(data);
                    }
                }
            }
            
        }


        public static void WriteDataUsedForPredictionToCsv(IList<string> dataUsedForPrediction)
        {
            using (FileStream fileStream = new FileStream(_testDataPath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    foreach (string data in dataUsedForPrediction)
                    {
                        writer.WriteLine(data);
                    }
                }
            }
        }

        public static void RemoveRowsFromCsv(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine(firstLine);
                }
            }
        }
    }

}
