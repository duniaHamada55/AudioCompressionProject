using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioCompressionProject
{
   public class CompressionProgressInfo
    {
        public int ProcessedSamples { get; set; }
        public int TotalSamples { get; set; }
        public double ElapsedSeconds { get; set; }
        public int CompressedSize { get; set; }

    }
}
