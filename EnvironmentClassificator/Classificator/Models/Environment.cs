using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace Classificator.Models
{
    class Environment
    {
        [Column("0")]
        public float Luminosity;

        [Column("1")]
        public float Humidity;

        [Column("2")]
        public float Temperature;

        [Column("3")]
        public float NoiseLevel;

        [Column("4")]
        public string EnvironmentState;
    }
}
