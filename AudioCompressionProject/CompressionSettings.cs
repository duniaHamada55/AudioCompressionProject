using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioCompressionProject
{
    public class CompressionSettings
    {
        public int SampleRate { get; set; }

        public int QuantizationLevels { get; set; }

        public int StepSize { get; set; }

        public string Algorithm { get; set; }
    }
}
