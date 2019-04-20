using Classificator;
using Classificator.ProcessData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentPredictor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController: ControllerBase
    {
        private readonly SerialPortReader _arduinoPort;

        public PredictionController(SerialPortReader arduinoPort)
        {
            _arduinoPort = arduinoPort;
        }

        [HttpGet("startPredictingOnTestData")]
        public IActionResult StartPredictingOnTestData()
        {
            EnvironmentModel createdModel = new EnvironmentModel();
            createdModel.LoadModelFromFile();
            _arduinoPort.SetUpPredictorModel(createdModel);
            _arduinoPort.ThreadStartReadingTestingData();

            //return NoContent();
            string messageToReturn = JsonConvert.SerializeObject("Process of gathering test data have been started");
            return Ok(messageToReturn);
        }

        [HttpGet("getPredictedValue")]
        public IActionResult GetPredictedValue(string state)
        {
            string messageToReturn = JsonConvert.SerializeObject(_arduinoPort.PredictedValue.EnvironmentState);
            return Ok(messageToReturn);
        }

        [HttpGet("stopPredictingOnTestData")]
        public IActionResult StopPredictingOnTestData()
        {

            _arduinoPort.CloseAfterPredict();
            _arduinoPort.ThreadStopReadingTestingData();
            string messageToReturn = JsonConvert.SerializeObject("Process of gathering test data have been started");
            return Ok(messageToReturn);

        }
    }
}
