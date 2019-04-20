using Classificator;
using Classificator.ProcessData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentPredictor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildModelController: ControllerBase
    {
        private readonly SerialPortReader _arduinoPort;

        public BuildModelController(SerialPortReader arduinoPort)
        {
            _arduinoPort = arduinoPort;
        }

        [HttpGet()]
        public IActionResult BuildModel()
        {

            EnvironmentModel model = new EnvironmentModel();
            model.LoadDataFromFile(DataWriterToCsv._trainDataPath);
            EstimatorChain<ColumnConcatenatingTransformer> pipeline = model.PreProcessData();
            model.BuildAndTrainModel(pipeline);
            model.SaveModelAsFile();

            //return NoContent();
            string messageToReturn = JsonConvert.SerializeObject("Model have been built!");
            return Ok(messageToReturn);
        }

        [HttpGet("deleteTrainingData")]
        public IActionResult DeleteTrainingData()
        {
            DataWriterToCsv.RemoveRowsFromCsv(DataWriterToCsv._trainDataPath);

            //return NoContent();
            string messageToReturn = JsonConvert.SerializeObject("Training data have been deleted!");
            return Ok(messageToReturn);
        }
    }
}
