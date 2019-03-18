using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classificator.ProcessData
{
    public class SerialPortReader
    {
        private SerialPort _arduinoPort;
        public volatile static string environmentValue;
        IList<string> serialDataForTrain;
        private EnvironmentModel classifier;

        public SerialPortReader(string defaultEnvironmentValue, string portName)
        {
            environmentValue = defaultEnvironmentValue;

            _arduinoPort = new SerialPort(portName)
            {
                BaudRate = 9600,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None
            };

            serialDataForTrain = new List<string>();
        }

        public void GetPorts()
        {
            Console.WriteLine("Serial ports available: ");
            Console.WriteLine("------------------------");
            foreach (var portName in SerialPort.GetPortNames())
            {
                Console.WriteLine(portName);
            }
        }
        
        public void ReadValuesToPredict(EnvironmentModel createdModel)
        {
            classifier = createdModel;

            if (!_arduinoPort.IsOpen)
            {
                _arduinoPort.Open();
            }
            // Subscribe to the DataReceived event.
            _arduinoPort.DataReceived += SerialPortDataReceivedToPredict;
        }

        private void SerialPortDataReceivedToPredict(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = (SerialPort)sender;

            // Read the data that is in the serial buffer
            var serialData = serialPort.ReadExisting();

            var sensorsValue = serialData.Split(',');

            if (sensorsValue.Length >= 4)
            {
                float lum = 0, hum = 0, temp = 0, nois = 0;
                float.TryParse(sensorsValue[0], out lum);
                float.TryParse(sensorsValue[1], out hum);
                float.TryParse(sensorsValue[2], out temp);
                float.TryParse(sensorsValue[3], out nois);

                Models.Environment readData = new Models.Environment()
                {
                    Luminosity = lum,
                    Humidity = hum,
                    Temperature = temp,
                    NoiseLevel = nois,
                };

                classifier.PredictOnTestData(readData);
            }
        }

        public void CloseAfterPredict()
        {
            _arduinoPort.Close();
        }



        public void ReadFromPort()
        {
            // Open the port
            if (!_arduinoPort.IsOpen)
            {
                _arduinoPort.Open();
            }
            // Subscribe to the DataReceived event.
            _arduinoPort.DataReceived += SerialPortDataReceived;
        }


        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = (SerialPort)sender;

            // Read the data that is in the serial buffer
            var serialData = serialPort.ReadExisting();

            if(serialData != string.Empty)
            {
                string record = string.Format("{0},{1}", serialData, environmentValue);

                serialDataForTrain.Add(record);
            }
        }

        public void Close()
        {
            _arduinoPort.Close();
            DataWriterToCsv.WriteTrainDataToCsv(serialDataForTrain);
            serialDataForTrain.Clear();
        }

        
    }
}
