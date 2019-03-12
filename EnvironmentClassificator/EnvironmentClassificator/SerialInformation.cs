using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;

namespace EnvironmentClassificator
{
    class SerialInformation
    {
        private SerialPort _arduinoPort;



        public void GetPorts()
        {
            Console.WriteLine("Serial ports available: ");
            Console.WriteLine("------------------------");
            foreach(var portName in SerialPort.GetPortNames())
            {
                Console.WriteLine(portName);
            }
        }

        public void ReadFromPort(string portName)
        {
            _arduinoPort = new SerialPort(portName)
            {
                BaudRate = 9600,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None
            };

            // Subscribe to the DataReceived event.
            _arduinoPort.DataReceived += SerialPortDataReceived;

            // Open the port
            _arduinoPort.Open();
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = (SerialPort)sender;

            // Read the data that is in the serial buffer

            var serialData = serialPort.ReadExisting();
            WriteDataToFile(serialData);

            // Display data
            Console.WriteLine(serialData);
        }

        public void Close()
        {
            _arduinoPort.Close();
        }

        private void WriteDataToFile(string serialData)
        {
            float Luminosity = 0, Humidity = 0, Temperature= 0, NoiseLevel= 0;

            using (StreamWriter writer = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "Data", "environment-sensors-test.csv")))
            {
                
                string newLine = string.Format($"{Luminosity},{Humidity},{Temperature},{NoiseLevel}");
                writer.WriteLine(newLine);
                writer.Flush();
            }
        }
    }
}
