using Classificator;
using Classificator.ProcessData;
using Microsoft.AspNetCore.Mvc;
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

            return NoContent();
        }

        [HttpGet("getPredictedValue")]
        public ActionResult<string> GetPredictedValue(string state)
        {
            return _arduinoPort.PredictedValue.EnvironmentState;
        }

        [HttpGet("stopPredictingOnTestData")]
        public IActionResult StopPredictingOnTestData()
        {

            _arduinoPort.CloseAfterPredict();
            _arduinoPort.ThreadStopReadingTestingData();
            return NoContent();
        }
    }
}
