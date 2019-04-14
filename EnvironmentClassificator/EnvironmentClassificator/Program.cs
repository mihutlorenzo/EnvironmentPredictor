using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using Classificator;
using Classificator.ProcessData;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;


namespace EnvironmentClassificator
{
    class Program
    {
        //static readonly string _trainDataPath = Path.Combine(System.Environment.CurrentDirectory, "Data", "environment-sensors-train.csv");
        //static readonly string _testDataPath = Path.Combine(System.Environment.CurrentDirectory, "Data", "environment-sensors-test.csv");
        static IList<string> possibleEnvironmentValues = new List<string> { "LIGHT", "DARK", "HOT", "COLD", "WET", "DRY" , "NOICE", "QUIET" }; 


        static void Main(string[] args)
        {
            //EnvironmentModel model = new EnvironmentModel();
            //ITransformer classificator = model.LoadModelFromFile();
            ////model.LoadDataFromFile(_trainDataPath);
            ////EstimatorChain<ColumnConcatenatingTransformer> pipeline = model.PreProcessData();
            ////EstimatorChain<KeyToValueMappingTransformer> trainedPipeline = model.BuildAndTrainModel(pipeline);
            //model.PredictOnTestData(_testDataPath, classificator);
            //model.Evaluate(_testDataPath, classificator);

            //SerialInformation arduinoPort = new SerialInformation();
            //// arduinoPort.GetPorts();
            //arduinoPort.ReadFromPort("COM3");
            //Console.ReadLine();
            //arduinoPort.Close();

            // ReadDataFromArduino();

            SerialPortReader arduinoPort = new SerialPortReader(possibleEnvironmentValues[0], "COM3");
            

            int mainOptions = 999;

            do
            {
                Console.WriteLine("Please choose one of the following options : " + Environment.NewLine + "1. Gather data" + 
                    Environment.NewLine + "2. Build model" + Environment.NewLine + "3. Make predictions" 
                    + Environment.NewLine + "4. Delete Training Data" + Environment.NewLine + "5. Exit");

                int.TryParse(Console.ReadLine(),out mainOptions);

                switch (mainOptions)
                {
                    case 1:
                        int environmentValue = 999;
                        //Thread serialPortReadThread = new Thread(new ThreadStart(arduinoPort.ReadFromPort));
                        //serialPortReadThread.Start();
                        arduinoPort.ThreadStartReadingTrainingData();
                        do
                        {
                            Console.WriteLine("1. LIGHT" + Environment.NewLine +
                                              "2. DARK" + Environment.NewLine +
                                              "3. HOT" + Environment.NewLine +
                                              "4. COLD" + Environment.NewLine +
                                              "5. WET" + Environment.NewLine +
                                              "6. DRY" + Environment.NewLine +
                                              "7. NOICE" + Environment.NewLine +
                                              "8. QUIET" + Environment.NewLine +
                                              "9. BACK");
                            int.TryParse(Console.ReadLine(), out int newValue);

                            if (environmentValue != newValue)
                            {
                                environmentValue = newValue;
                                if(environmentValue >= 1 && environmentValue <= 8)
                                {
                                    SerialPortReader.environmentValue = possibleEnvironmentValues[environmentValue - 1];
                                }
                                else if(environmentValue == 9)
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("You chosen a wrong value. Please select one number between 1 and 8");
                                }
                            }

                        } while (environmentValue != 9) ;

                        arduinoPort.Close();
                        //serialPortReadThread.Join();
                        arduinoPort.ThreadStopReadingTrainingData();

                        break;
                    case 2:
                        EnvironmentModel model = new EnvironmentModel();
                        model.LoadDataFromFile(DataWriterToCsv._trainDataPath);
                        EstimatorChain<ColumnConcatenatingTransformer> pipeline = model.PreProcessData();
                        model.BuildAndTrainModel(pipeline);
                        model.SaveModelAsFile();
                        // Code for build model goes here
                        break;
                    case 3:
                        EnvironmentModel createdModel = new EnvironmentModel();
                        createdModel.LoadModelFromFile();
                        arduinoPort.SetUpPredictorModel(createdModel);

                        arduinoPort.ThreadStartReadingTestingData();

                        int readValue = 999;
                        do
                        {
                            Console.WriteLine("Press 5 to exit!");
                            int.TryParse(Console.ReadLine(), out readValue);
                        } while (readValue != 5);
                        
                            arduinoPort.CloseAfterPredict();
                            arduinoPort.ThreadStopReadingTestingData();
                        // Code for predictions goes here
                        break;
                    case 4:
                        DataWriterToCsv.RemoveRowsFromCsv(DataWriterToCsv._trainDataPath);
                        break;
                    default:
                        Console.WriteLine("Invalid option, please choose another one!");
                        break;
                }

            }while (mainOptions != 5) ;

        }

        //public static void ReadDataFromArduino()
        //{
        //    _serialPort = new SerialPort();
        //    _serialPort.PortName = "COM3";//Set your board COM
        //    _serialPort.BaudRate = 9600;
        //    _serialPort.Open();
        //    while (true)
        //    {
        //        string a = _serialPort.ReadExisting();
        //        Console.WriteLine(a);
        //        Thread.Sleep(200);
        //    }
        //}
    }
}
