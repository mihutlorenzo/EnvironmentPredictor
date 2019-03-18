
using Classificator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using System.Data;
//using Microsoft.ML.Trainers;
//using Microsoft.ML.Transforms.Conversions;
//using Microsoft.ML.Transforms.Normalizers;

namespace Classificator
{
    public class EnvironmentModel
    {
        private readonly string _modelPath = @"E:\Repositories\Github\EnvironmentPredictor\EnvironmentClassificator\Classificator\Data\Model.zip";

        private MLContext _mlContext;
        private PredictionEngine<Models.Environment, EnvironmentPredicted> _predEngine;
        private ITransformer _trainedModel;
        private IDataView _trainingDataView;

        // static TextLoader _textLoader;

        public EnvironmentModel()
        { 
            // Read the data from the file
            _mlContext = new MLContext(seed: 0);
           
        }

        public void LoadDataFromFile(string trainDataPath)
        {
            _trainingDataView = _mlContext.Data.CreateTextLoader<Models.Environment>(hasHeader: true, separatorChar: ',').Load(trainDataPath);
        }

        public EstimatorChain<ColumnConcatenatingTransformer> PreProcessData()
        {
            EstimatorChain<ColumnConcatenatingTransformer> pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "EnvironmentState", outputColumnName: "Label")
                                        .Append(_mlContext.Transforms.Concatenate("Features", "Luminosity", "Humidity", "Temperature", "NoiseLevel"))
                                        .AppendCacheCheckpoint(_mlContext);

            return pipeline;
        }



        public void BuildAndTrainModel(EstimatorChain<ColumnConcatenatingTransformer> pipeline)
        {
            var trainingPipeline = pipeline.Append(_mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(DefaultColumnNames.Label, DefaultColumnNames.Features))
                                           .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            _trainedModel = trainingPipeline.Fit(_trainingDataView);

            _predEngine = _trainedModel.CreatePredictionEngine<Models.Environment, EnvironmentPredicted>(_mlContext);

            Models.Environment sensors = new Models.Environment()
            {
                Luminosity = 155,
                Humidity = 48,
                Temperature = 28,
                NoiseLevel = 160,
            };

            var prediction = _predEngine.Predict(sensors);
            Console.WriteLine($"============= Single Prediction just-trained-model - Result: {prediction.EnvironmentState}");

        }

        public void PredictOnTestData(Models.Environment sensorsNewValue)
        {
            //Models.Environment sensors = new Models.Environment()
            //{
            //    Luminosity = 85,
            //    Humidity = 24,
            //    NoiseLevel = 8,
            //    Temperature = 27
            //};

            //Models.Environment sensors = new Models.Environment()
            //{
            //    Luminosity = 155,
            //    Humidity = 48,
            //    Temperature = 28,
            //    NoiseLevel = 160,
            //};

            var prediction = _predEngine.Predict(sensorsNewValue);

            Console.WriteLine($"============= Single Prediction just-trained-model - Result: {prediction.EnvironmentState}");
        }

        public void Evaluate(string testDataPath, ITransformer trainedModel)
        {
            var testDataView = _mlContext.Data.CreateTextLoader<Models.Environment>(hasHeader: true, separatorChar: ',').Load(testDataPath);

            var testMetrics = _mlContext.MulticlassClassification.Evaluate(trainedModel.Transform(testDataView));

            Console.WriteLine($"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for Multi-class Classification model - Test Data     ");
            Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"*       MicroAccuracy:    {testMetrics.AccuracyMicro:0.###}");
            Console.WriteLine($"*       MacroAccuracy:    {testMetrics.AccuracyMacro:0.###}");
            Console.WriteLine($"*       LogLoss:          {testMetrics.LogLoss:#.###}");
            Console.WriteLine($"*       LogLossReduction: {testMetrics.LogLossReduction:#.###}");
            Console.WriteLine($"*************************************************************************************************************");
        }

        // Saves the model as a.zip file.
        public void SaveModelAsFile()
        {
            using (var fs = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                _mlContext.Model.Save(_trainedModel, fs);
                Console.WriteLine("The model is saved to {0}", _modelPath);
            }
        }

        public void LoadModelFromFile()
        {
            // ITransformer loadedModel;
            using (var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _trainedModel = _mlContext.Model.Load(stream);
            }

            //Models.Environment sensorsTest = new Models.Environment()
            //{
            //    Luminosity = 20,
            //    Humidity = 24,
            //    NoiseLevel = 8,
            //    Temperature = 27
            //};

            _predEngine = _trainedModel.CreatePredictionEngine<Models.Environment, EnvironmentPredicted>(_mlContext);

            // var prediction = _predEngine.Predict(sensorsTest);

            // Console.WriteLine($"=============== Single Prediction - Result: {prediction.EnvironmentState} ===============");
        }

            //public ITransformer Train(MLContext mlContext, string dataPath)
            //{
            //    IDataView dataView = _textLoader.Load(dataPath);

        //    var pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "EnvironmentState", outputColumnName: "Label")
        //                            .Append(mlContext.Transforms.Concatenate("Features", "Luminosity", "Humidity", "Temperature", "NoiseLevel")
        //                            .Append(mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(DefaultColumnNames.Label, DefaultColumnNames.Features)))
        //                            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        //    var model = pipeline.Fit(dataView);

        //    SaveModelAsFile(mlContext, model);
        //    return model;
        //}

        //public void SaveModelAsFile(MLContext mlContext, ITransformer model)
        //{
        //    using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
        //    {
        //        mlContext.Model.Save(model, fileStream);
        //        Console.WriteLine("The Model is saved to {0}", _modelPath);
        //    }
        //}

        //private void Evaluate(MLContext mlContext, ITransformer model)
        //{
        //    PredictionEngine<Models.Environment, EnvironmentPredicted> _predEngine = model.CreatePredictionEngine<Models.Environment, EnvironmentPredicted>(mlContext);

        //}
    }
}
