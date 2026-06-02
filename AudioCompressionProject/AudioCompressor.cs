

using System;
using System.Collections.Generic;

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

        public static List<short> CompressDPCM(float[] samples)
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
        }

        public static List<short> DecompressDPCM(List<short> data)
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
        }
      
        public static List<short> CompressDelta(float[] samples)
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
        }

        public static List<short> DecompressDelta(List<short> data)
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
        }

        public static List<short> CompressAdaptiveDelta(float[] samples)
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
        }

        public static List<short> DecompressAdaptiveDelta(List<short> data)
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
        }

        public static List<short> CompressPDC(float[] samples)
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
        }

        public static List<short> DecompressPDC(List<short> data)
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
        }

        public static List<short> CompressNonlinearQuantization(float[] samples)
        {
            List<short> compressed = new List<short>();

            const float mu = 255f;

            foreach (float sample in samples)
            {
                float x = sample;
                float compressedValue =
                    Math.Sign(x) *
                    (float)(Math.Log(1 + mu * Math.Abs(x)) / Math.Log(1 + mu));

                compressed.Add((short)(compressedValue * 32767));
            }

            return compressed;
        }

        public static List<short> DecompressNonlinearQuantization(List<short> data)
        {
            List<short> samples = new List<short>();

            const float mu = 255f;

            foreach (short q in data)
            {
                float x = q / 32767f;

                float expanded =
                    Math.Sign(x) *
                    (1f / mu) *
                    ((float)Math.Pow(1 + mu, Math.Abs(x)) - 1);

                samples.Add((short)(expanded * 32767));
            }

            return samples;
        }
    }
}