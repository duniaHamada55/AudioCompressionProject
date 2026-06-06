

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace AudioCompressionProject
{
    public class AudioCompressor
    {
      
        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private static int ToPCM(float sample)
        {
            return (int)(Clamp((int)(sample * 32767), short.MinValue, short.MaxValue));
        }

        /*  public static List<short> CompressDPCM(float[] samples , CompressionSettings settings)
           {
               List<short> compressed = new List<short>();

               int previous = 0;

               foreach (float sample in samples)
               {
                   int current = ToPCM(sample);

                   int diff = current - previous;

                   compressed.Add((short)Clamp(diff, short.MinValue, short.MaxValue));

                   previous = current;
               }

               return compressed;
           }*/

        public static List<short> CompressDPCM(float[] samples, CompressionSettings settings , Action<CompressionProgressInfo> reportProgress = null)
        {
            List<short> compressed = new List<short>();

            int previousReconstructed = 0;

            int levels = settings.QuantizationLevels;

            float interval = 65535f / levels;
            int maxLevel = (levels / 2) - 1;
            int minLevel = -(levels / 2);
            Stopwatch sw = Stopwatch.StartNew();
            CompressionProgressInfo info = new CompressionProgressInfo();
            for (int i = 0; i < samples.Length; i++)
            {
                float sample = samples[i];
                int current = ToPCM(sample);

                int error = current - previousReconstructed;

                int quantizedError = (int)Math.Round(error / interval);

                quantizedError = Math.Max(minLevel, Math.Min(maxLevel, quantizedError));
                compressed.Add((short)quantizedError);

                int reconstructedError = (int)(quantizedError * interval);
                int currentReconstructed = previousReconstructed + reconstructedError;

                previousReconstructed = Math.Max(short.MinValue, Math.Min(short.MaxValue, currentReconstructed));
               
                if (i % 200 == 0 || i == samples.Length - 1)
                {
                    info.ProcessedSamples = i;
                    info.TotalSamples = samples.Length;
                    info.ElapsedSeconds = sw.Elapsed.TotalSeconds;
                    info.CompressedSize = compressed.Count;
                 

                    reportProgress?.Invoke(info);
                }
            }
            sw.Stop();
            return compressed;
        }

        /*   public static List<short> DecompressDPCM(List<short> data)
           {
               List<short> samples = new List<short>();

               int previous = 0;

               foreach (short diff in data)
               {
                   int current = previous + diff;

                   samples.Add((short)Clamp(current, short.MinValue, short.MaxValue));

                   previous = current;
               }

               return samples;
           }*/
        public static List<short> DecompressDPCM(List<short> data, CompressionSettings settings, Action<int> reportProgress = null)
        {
            List<short> samples = new List<short>();

            int previousReconstructed = 0;

            int levels = settings.QuantizationLevels;
            float interval = 65535f / levels;
            int i = 1;
            foreach (short qError in data)
            {

                int reconstructedError = (int)(qError * interval);


                int currentReconstructed = previousReconstructed + reconstructedError;


                short finalSample = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, currentReconstructed));
                samples.Add(finalSample);

                previousReconstructed = currentReconstructed;
                reportProgress?.Invoke((i * 100) / data.Count);
                i++;
            }

            return samples;
        }

        /*   public static List<short> CompressDelta(float[] samples , CompressionSettings settings)
           {
               List<short> compressed = new List<short>();

               int predicted = 0;
               int step = 500;

               foreach (float sample in samples)
               {
                   int current = (int)(sample * 32767);

                   int error = current - predicted;

                   if (error >= 0)
                   {
                       compressed.Add(1);
                       predicted += step;
                   }
                   else
                   {
                       compressed.Add(0);
                       predicted -= step;
                   }

                   step = Math.Max(10, Math.Min(2000, step + (Math.Abs(error) > step ? 50 : -20)));
               }

               return compressed;
           }*/
        public static List<short> CompressDelta(float[] samples, CompressionSettings settings , Action<CompressionProgressInfo> reportProgress = null)
        {
            List<short> compressed = new List<short>();

            int predicted = 0;

            int step = settings.StepSize;
            Stopwatch sw = Stopwatch.StartNew();
            CompressionProgressInfo info = new CompressionProgressInfo();
            for (int i = 0; i < samples.Length; i++)
            {
                float sample = samples[i];
                int current = ToPCM(sample);

                int error = current - predicted;
              
                if (error >= 0)
                {
                    compressed.Add(1);
                    predicted += step;
                }
                else
                {
                    compressed.Add(0);
                    predicted -= step;
                }
              
                predicted = Math.Max(short.MinValue, Math.Min(short.MaxValue, predicted));
                

                if (i % 200 == 0 || i == samples.Length - 1)
                {
                    info.ProcessedSamples = i;
                    info.TotalSamples = samples.Length;
                    info.ElapsedSeconds = sw.Elapsed.TotalSeconds;
                    info.CompressedSize = compressed.Count;


                    reportProgress?.Invoke(info);
                }
            }
            sw.Stop();

            return compressed;
        }

        /* public static List<short> DecompressDelta(List<short> data)
         {
             List<short> samples = new List<short>();

             int value = 0;
             int step = 500;

             foreach (short bit in data)
             {
                 if (bit == 1)
                     value += step;
                 else
                     value -= step;

                 step = Math.Max(10, Math.Min(2000, step + 10));

                 samples.Add((short)value);
             }

             return samples;
         }*/

        public static List<short> DecompressDelta(List<short> data, CompressionSettings settings , Action<int> reportProgress = null)
        {
            List<short> samples = new List<short>();

            int value = 0;
         
            int step = settings.StepSize;
            int i = 1;
            foreach (short bit in data)
            {
                if (bit == 1)
                    value += step;
                else
                    value -= step;

                
                value = Math.Max(short.MinValue, Math.Min(short.MaxValue, value));

                samples.Add((short)value);
                reportProgress?.Invoke((i * 100) / data.Count);
                i++;
            }

            return samples;
        }

        /* public static List<short> CompressAdaptiveDelta(float[] samples , CompressionSettings settings)
         {
             List<short> compressed = new List<short>();

             int predicted = 0;
             int step = 500;

             foreach (float sample in samples)
             {
                 int current = (int)(sample * 32767);

                 int error = current - predicted;

                 if (error >= 0)
                 {
                     compressed.Add(1);
                     predicted += step;
                 }
                 else
                 {
                     compressed.Add(0);
                     predicted -= step;
                 }

                 if (Math.Abs(error) > step)
                     step = Math.Min(3000, step + 100);
                 else
                     step = Math.Max(10, step - 50);
             }

             return compressed;
         }*/
        public static List<short> CompressAdaptiveDelta(float[] samples, CompressionSettings settings , Action<CompressionProgressInfo> reportProgress = null)
        {
        
            List<short> compressed = new List<short>();
            int predicted = 0;
            int step = settings.StepSize;
            int previousBit = -1;
            Stopwatch sw = Stopwatch.StartNew();
            CompressionProgressInfo info = new CompressionProgressInfo();
            for (int i = 0; i < samples.Length; i++)
            {
                float sample = samples[i];
                int current = ToPCM(sample);
                int error = current - predicted;
                int currentBit;

                if (error >= 0)
                {
                    compressed.Add(1);
                    predicted += step;
                    currentBit = 1;
                }
                else
                {
                    compressed.Add(0);
                    predicted -= step;
                    currentBit = 0;
                }

                predicted = Math.Max(short.MinValue, Math.Min(short.MaxValue, predicted));

                
                if (currentBit == previousBit)
                    step = Math.Min(10000, (int)(step* 1.5)); 
                else
                    step = Math.Max(10, (int)(step * 0.5));   

                previousBit = currentBit;

                if (i % 200 == 0 || i == samples.Length - 1)
                {
                    info.ProcessedSamples = i;
                    info.TotalSamples = samples.Length;
                    info.ElapsedSeconds = sw.Elapsed.TotalSeconds;
                    info.CompressedSize = compressed.Count;


                    reportProgress?.Invoke(info);
                }
            }
            sw.Stop();
            return compressed;
        }

        /* public static List<short> DecompressAdaptiveDelta(List<short> data)
         {
             List<short> samples = new List<short>();

             int value = 0;
             int step = 500;

             foreach (short bit in data)
             {
                 if (bit == 1)
                     value += step;
                 else
                     value -= step;

                 step = Math.Max(10, Math.Min(3000, step + 20));

                 samples.Add((short)value);
             }

             return samples;
         }*/

        public static List<short> DecompressAdaptiveDelta(List<short> data, CompressionSettings settings, Action<int> reportProgress = null)
        {
            List<short> samples = new List<short>();
            int value = 0;
            int step = settings.StepSize; 
            int previousBit = -1;
            int i = 1;
            foreach (short bit in data)
            {
                if (bit == 1)
                    value += step;
                else
                    value -= step;

                value = Math.Max(short.MinValue, Math.Min(short.MaxValue, value));
                samples.Add((short)value);

                
                if (bit == previousBit)
                    step = Math.Min(10000, (int)(step * 1.5));
                else
                    step = Math.Max(10, (int)(step* 0.5));

                previousBit = bit;
                reportProgress?.Invoke((i * 100) / data.Count);
                i++;
            }
            return samples;
        }

        /*  public static List<short> CompressPDC(float[] samples , CompressionSettings settings)
          {
              List<short> compressed = new List<short>();

              int previous = 0;

              foreach (float sample in samples)
              {
                  int current = ToPCM(sample);

                  int error = current - previous;

                  compressed.Add((short)Clamp(error, short.MinValue, short.MaxValue));

                  previous = current;
              }

              return compressed;
          }*/

        public static List<short> CompressPDC(float[] samples, CompressionSettings settings, Action<CompressionProgressInfo> reportProgress = null)
        {
            List<short> compressed = new List<short>();

            int prev1 = 0; 
            int prev2 = 0; 
            int prev3 = 0; 

            int levels = settings.QuantizationLevels;
            float interval = 65535f / levels;
            int maxLevel = (levels / 2) - 1;
            int minLevel = -(levels / 2);
            Stopwatch sw = Stopwatch.StartNew();
            CompressionProgressInfo info = new CompressionProgressInfo();
            for (int i = 0; i < samples.Length; i++)
            {
                float sample = samples[i];
                int current = ToPCM(sample);

               
                int predicted = (int)((0.6f * prev1) + (0.3f * prev2) + (0.1f * prev3));

              
                int error = current - predicted;

                int quantizedError = (int)Math.Round(error / interval);
                quantizedError = Math.Max(minLevel, Math.Min(maxLevel, quantizedError));
                compressed.Add((short)quantizedError);
               
                int reconstructedError = (int)(quantizedError * interval);
                int currentReconstructed = predicted + reconstructedError;
                currentReconstructed = Math.Max(short.MinValue, Math.Min(short.MaxValue, currentReconstructed));
             
                prev3 = prev2; 
                prev2 = prev1; 
                prev1 = currentReconstructed;

                if (i % 200 == 0 || i == samples.Length - 1)
                {
                    info.ProcessedSamples = i;
                    info.TotalSamples = samples.Length;
                    info.ElapsedSeconds = sw.Elapsed.TotalSeconds;
                    info.CompressedSize = compressed.Count;


                    reportProgress?.Invoke(info);
                }
            }
            sw.Stop();

            return compressed;
        }

        /*  public static List<short> DecompressPDC(List<short> data)
          {
              List<short> samples = new List<short>();

              int previous = 0;

              foreach (short error in data)
              {
                  int current = previous + error;

                  samples.Add((short)Clamp(current, short.MinValue, short.MaxValue));

                  previous = current;
              }

              return samples;
          }*/

       
        public static List<short> DecompressPDC(List<short> data, CompressionSettings settings, Action<int> reportProgress = null)
        {
            List<short> samples = new List<short>();

            int prev1 = 0;
            int prev2 = 0;
            int prev3 = 0;

            int levels = settings.QuantizationLevels;
            float interval = 65535f / levels;
            int i = 1;
            foreach (short qError in data)
            {
             
                int predicted = (int)((0.6f * prev1) + (0.3f * prev2) + (0.1f * prev3));
          
                int reconstructedError = (int)(qError * interval);
     
                int currentReconstructed = predicted + reconstructedError;
                short finalSample = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, currentReconstructed));
                samples.Add(finalSample);
       
                prev3 = prev2;
                prev2 = prev1;
                prev1 = currentReconstructed;

                reportProgress?.Invoke((i * 100) / data.Count);
                i++;
            }

            return samples;
        }

        public static List<short> CompressNonlinearQuantization(float[] samples , CompressionSettings settings , Action<CompressionProgressInfo> reportProgress = null)
        {
            List<short> compressed = new List<short>();

            const float mu = 255f;
            Stopwatch sw = Stopwatch.StartNew();
            CompressionProgressInfo info = new CompressionProgressInfo();
            for (int i = 0; i < samples.Length; i++)
            {
                float sample = samples[i];
                float x = sample;
                float compressedValue =
                    Math.Sign(x) *
                    (float)(Math.Log(1 + mu * Math.Abs(x)) / Math.Log(1 + mu));

                compressed.Add((short)(compressedValue * 32767));

                if (i % 200 == 0 || i == samples.Length - 1)
                {
                    info.ProcessedSamples = i;
                    info.TotalSamples = samples.Length;
                    info.ElapsedSeconds = sw.Elapsed.TotalSeconds;
                    info.CompressedSize = compressed.Count;


                    reportProgress?.Invoke(info);
                }
            }
            sw.Stop();

            return compressed;
        }

        public static List<short> DecompressNonlinearQuantization(List<short> data, Action<int> reportProgress = null)
        {
            List<short> samples = new List<short>();

            const float mu = 255f;
            int i = 1;  
            foreach (short q in data)
            {
                float x = q / 32767f;

                float expanded =
                    Math.Sign(x) *
                    (1f / mu) *
                    ((float)Math.Pow(1 + mu, Math.Abs(x)) - 1);

                samples.Add((short)(expanded * 32767));

                reportProgress?.Invoke((i * 100) / data.Count);
                i++;
            }

            return samples;
        }
    }
}