using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace Classificator.Models
{
    public class EnvironmentPredicted
    {
        [ColumnName("PredictedLabel")]
        public string EnvironmentState;
    }
}
