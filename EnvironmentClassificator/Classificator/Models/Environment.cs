using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classificator.Models
{
    class Environment
    {
        [LoadColumn(0)]
        public float Luminosity;

        [LoadColumn(1)]
        public float Humidity;

        [LoadColumn(2)]
        public float Temperature;

        [LoadColumn(3)]
        public float NoiseLevel;

        [LoadColumn(4)]
        public string EnvironmentState;
    }
}
