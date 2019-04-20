using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Classificator.ProcessData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            string messageToReturn = JsonConvert.SerializeObject("Process of gathering training data have been started");
            return Ok(messageToReturn);
        }

        [HttpGet("writeEnvironmentState/{state}")]
        public IActionResult ReadTrainingData(string state)
        {
            if (SerialPortReader.environmentValue != state)
            {
                SerialPortReader.environmentValue = state;
            }

            string messageToReturn = JsonConvert.SerializeObject(String.Format("Environment state have been changed to {0}", SerialPortReader.environmentValue));
            return Ok(messageToReturn);
        }

        [HttpGet("stopGatheringTrainingData")]
        public IActionResult StopGatheringTrainingData()
        {

            _arduinoPort.Close();
            _arduinoPort.ThreadStopReadingTrainingData();
            string messageToReturn = JsonConvert.SerializeObject("Process of gathering training data have been stoped");
            return Ok(messageToReturn);
        }
    }
}
