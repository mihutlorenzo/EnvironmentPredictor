using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Classificator.ProcessData;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentPredictor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatherDataController: ControllerBase
    {
        private readonly SerialPortReader _arduinoPort;

        public GatherDataController(SerialPortReader arduinoPort)
        {
            _arduinoPort = arduinoPort;
        }

        [HttpGet("startGatheringTrainingData")]
        public IActionResult StartGatherTrainingData()
        {

            _arduinoPort.ThreadStartReadingTrainingData();
            return NoContent();
        }

        [HttpGet("writeEnvironmentState/{state}")]
        public IActionResult ReadTrainingData(string state)
        {
            if (SerialPortReader.environmentValue != state)
            {
                 SerialPortReader.environmentValue = state;
            }

            return NoContent();
        }

        [HttpGet("stopGatheringTrainingData")]
        public IActionResult StopGatheringTrainingData()
        {

            _arduinoPort.Close();
            _arduinoPort.ThreadStopReadingTrainingData();
            return NoContent();
        }
    }
}
