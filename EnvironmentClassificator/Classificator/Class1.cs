
using Classificator.Models;
using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classificator
{
    public class EnvironmentModel
    {
        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "environment-sensors-train.csv");
        static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "environment-sensors-test.csv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        static TextLoader _textLoader;

        public void InitializeTheContext()
        {
            MLContext mlContext = new MLContext(seed: 0);

            _textLoader = mlContext.Data.CreateTextLoader(new TextLoader.Options()
            {
                Separators = new[] { ',' },
                HasHeader = true,
                Columns = new[]
                    {
                        new TextLoader.Column("Luminosity", DataKind.Double, 0),
                        new TextLoader.Column("Humidity", DataKind.Double, 1),
                        new TextLoader.Column("Temperature", DataKind.Double, 2),
                        new TextLoader.Column("NoiseLevel", DataKind.Double, 3),
                        new TextLoader.Column("EnvironmentState", DataKind.String, 4)
                    }
            });

            var model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model);

        }

        public ITransformer Train(MLContext mlContext, string dataPath)
        {
            IDataView dataView = _textLoader.Load(dataPath);

            var pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "EnvironmentState", outputColumnName: "Label")
                                    .Append(mlContext.Transforms.Concatenate("Features", "Luminosity", "Humidity", "Temperature", "NoiseLevel")
                                    .Append(mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(DefaultColumnNames.Label, DefaultColumnNames.Features)))
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var model = pipeline.Fit(dataView);

            SaveModelAsFile(mlContext, model);
            return model;
        }

        public void SaveModelAsFile(MLContext mlContext, ITransformer model)
        {
            using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                mlContext.Model.Save(model, fileStream);
                Console.WriteLine("The Model is saved to {0}", _modelPath);
            }
        }

        private void Evaluate(MLContext mlContext, ITransformer model)
        {
            PredictionEngine<Models.Environment, EnvironmentPredicted> _predEngine = model.CreatePredictionEngine<Models.Environment, EnvironmentPredicted>(mlContext);
            https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/github-issue-classification
        }
    }
}
