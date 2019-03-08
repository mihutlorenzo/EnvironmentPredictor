using System;
using System.IO;
using Classificator;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace EnvironmentClassificator
{
    class Program
    {
        static readonly string _trainDataPath = Path.Combine(System.Environment.CurrentDirectory, "Data", "environment-sensors-train.csv");
        static readonly string _testDataPath = Path.Combine(System.Environment.CurrentDirectory, "Data", "environment-sensors-test.csv");


        static void Main(string[] args)
        {
            EnvironmentModel model = new EnvironmentModel();
            ITransformer classificator = model.LoadModelFromFile();
            //model.LoadDataFromFile(_trainDataPath);
            //EstimatorChain<ColumnConcatenatingTransformer> pipeline = model.PreProcessData();
            //EstimatorChain<KeyToValueMappingTransformer> trainedPipeline = model.BuildAndTrainModel(pipeline);
            model.Evaluate(_testDataPath,classificator);
        }
    }
}
