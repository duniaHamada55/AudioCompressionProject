

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
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

        public static List<byte> CompressDPCM(
           CancellationToken token,
           float[] samples,
           CompressionSettings settings,
           Action<CompressionProgressInfo> reportProgress = null
          )
        {
            List<byte> compressed = new List<byte>();

            int prev1 = 0;
            int prev2 = 0;

            int levels = settings.QuantizationLevels;
            int bitsPerSample = (int)Math.Log(levels, 2);
            float interval = 65535f / levels;

            int maxLevel = (levels / 2) - 1;
            int minLevel = -(levels / 2);

            byte currentByte = 0;
            int bitPosition = 0;

            Stopwatch sw = Stopwatch.StartNew();
            CompressionProgressInfo info = new CompressionProgressInfo();

            for (int i = 0; i < samples.Length; i++)
            {
                token.ThrowIfCancellationRequested();
                int current = ToPCM(samples[i]);

                int predicted = (prev1 + prev2) / 2;

                int error = current - predicted;

                int quantizedError = (int)Math.Round(error / interval);

                quantizedError = Math.Max(minLevel,
                                  Math.Min(maxLevel, quantizedError));

                int packedValue = quantizedError - minLevel;

                for (int b = 0; b < bitsPerSample; b++)
                {
                    int bit = (packedValue >> b) & 1;

                    currentByte |= (byte)(bit << bitPosition);
                    bitPosition++;

                    if (bitPosition == 8)
                    {
                        compressed.Add(currentByte);
                        currentByte = 0;
                        bitPosition = 0;
                    }
                }

                int reconstructedError = (int)(quantizedError * interval);
                int reconstructed = predicted + reconstructedError;

                reconstructed = Math.Max(
                     short.MinValue, Math.Min(short.MaxValue, reconstructed));

                prev2 = prev1;
                prev1 = reconstructed;

                if (i % 200 == 0 || i == samples.Length - 1)
                {
                    info.ProcessedSamples = i;
                    info.TotalSamples = samples.Length;
                    info.ElapsedSeconds = sw.Elapsed.TotalSeconds;
                    info.CompressedSize = compressed.Count;

                    reportProgress?.Invoke(info);
                }
            }

            if (bitPosition > 0)
                compressed.Add(currentByte);

            return compressed;
        }

        public static List<short> DecompressDPCM(
          List<byte> data,
          CompressionSettings settings,
          int originalSampleCount,
          Action<int> reportProgress = null)
        {
            List<short> samples = new List<short>();

            int prev1 = 0;
            int prev2 = 0;

            int levels = settings.QuantizationLevels;
            int bitsPerSample = (int)Math.Log(levels, 2);
            float interval = 65535f / levels;

            int minLevel = -(levels / 2);

            int currentValue = 0;
            int bitsRead = 0;
            int processed = 0;

            foreach (byte currentByte in data)
            {
                for (int b = 0; b < 8; b++)
                {
                    int bit = (currentByte >> b) & 1;

                    currentValue |= (bit << bitsRead);
                    bitsRead++;

                    if (bitsRead == bitsPerSample)
                    {
                        int qError = currentValue + minLevel;

                        int predicted = (prev1 + prev2) / 2;

                        int reconstructedError = (int)(qError * interval);
                        int sample = predicted + reconstructedError;

                        short finalSample = (short)Math.Max(
                            short.MinValue,
                            Math.Min(short.MaxValue, sample));

                        samples.Add(finalSample);

                        prev2 = prev1;
                        prev1 = finalSample;
                        //prev1 = sample;

                        processed++;

                        reportProgress?.Invoke(
                            (processed * 100) / originalSampleCount);

                        currentValue = 0;
                        bitsRead = 0;

                        if (processed >= originalSampleCount)
                            return samples;
                    }
                }
            }

            return samples;
        }
        public static List<byte> CompressDelta(
            CancellationToken token,
           float[] samples,
           CompressionSettings settings,
           Action<CompressionProgressInfo> reportProgress = null)
        {
            List<byte> compressed = new List<byte>();

            int predicted = 0;
            int step = settings.StepSize;

            byte currentByte = 0;
            int bitPosition = 0;

            Stopwatch sw = Stopwatch.StartNew();
            CompressionProgressInfo info = new CompressionProgressInfo();

            for (int i = 0; i < samples.Length; i++)
            {
                token.ThrowIfCancellationRequested();
                float sample = samples[i];
                int current = ToPCM(sample);

                int error = current - predicted;

                int bit;

                if (error >= 0)
                {
                    bit = 1;
                    predicted += step;
                }
                else
                {
                    bit = 0;
                    predicted -= step;
                }

                predicted = Math.Max(short.MinValue,
                            Math.Min(short.MaxValue, predicted));

                currentByte |= (byte)(bit << bitPosition);

                bitPosition++;

                if (bitPosition == 8)
                {
                    compressed.Add(currentByte);

                    currentByte = 0;
                    bitPosition = 0;
                }

                if (i % 200 == 0 || i == samples.Length - 1)
                {
                    info.ProcessedSamples = i;
                    info.TotalSamples = samples.Length;
                    info.ElapsedSeconds = sw.Elapsed.TotalSeconds;

                    info.CompressedSize = compressed.Count;

                    reportProgress?.Invoke(info);
                }
            }

            if (bitPosition > 0)
            {
                compressed.Add(currentByte);
            }

            sw.Stop();

            return compressed;
        }

        public static List<short> DecompressDelta(
          List<byte> data,
          CompressionSettings settings,
          int originalSampleCount,
          Action<int> reportProgress = null)
        {
            List<short> samples = new List<short>();

            int value = 0;
            int step = settings.StepSize;

            int processedSamples = 0;

            foreach (byte currentByte in data)
            {
                for (int bitPosition = 0; bitPosition < 8; bitPosition++)
                {
                    if (processedSamples >= originalSampleCount)
                        break;

                    int bit = (currentByte >> bitPosition) & 1;

                    if (bit == 1)
                        value += step;
                    else
                        value -= step;

                    value = Math.Max(
                        short.MinValue,
                        Math.Min(short.MaxValue, value));

                    samples.Add((short)value);

                    processedSamples++;

                    reportProgress?.Invoke(
                        (processedSamples * 100) / originalSampleCount);
                }
            }

            return samples;
        }

        public static List<byte> CompressAdaptiveDelta(
           CancellationToken token,
          float[] samples,
          CompressionSettings settings,
          Action<CompressionProgressInfo> reportProgress = null)
        {
            List<byte> compressed = new List<byte>();

            int predicted = 0;
            int step = settings.StepSize;
            int previousBit = -1;

            byte currentByte = 0;
            int bitPosition = 0;

            Stopwatch sw = Stopwatch.StartNew();
            CompressionProgressInfo info = new CompressionProgressInfo();

            for (int i = 0; i < samples.Length; i++)
            {
                token.ThrowIfCancellationRequested();
                float sample = samples[i];
                int current = ToPCM(sample);

                int error = current - predicted;
                int currentBit;

                if (error >= 0)
                {
                    currentBit = 1;
                    predicted += step;
                }
                else
                {
                    currentBit = 0;
                    predicted -= step;
                }

                predicted = Math.Max(
                    short.MinValue,
                    Math.Min(short.MaxValue, predicted));

                currentByte |= (byte)(currentBit << bitPosition);

                bitPosition++;

                if (bitPosition == 8)
                {
                    compressed.Add(currentByte);

                    currentByte = 0;
                    bitPosition = 0;
                }

                if (currentBit == previousBit)
                    step = Math.Min(10000, (int)(step * 1.5));
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

            if (bitPosition > 0)
            {
                compressed.Add(currentByte);
            }

            sw.Stop();

            return compressed;
        }

        public static List<short> DecompressAdaptiveDelta(
           List<byte> data,
           CompressionSettings settings,
           int originalSampleCount,
           Action<int> reportProgress = null)
        {
            List<short> samples = new List<short>();

            int value = 0;
            int step = settings.StepSize;
            int previousBit = -1;

            int processedSamples = 0;

            foreach (byte currentByte in data)
            {
                for (int bitPosition = 0; bitPosition < 8; bitPosition++)
                {
                    if (processedSamples >= originalSampleCount)
                        break;

                    int bit = (currentByte >> bitPosition) & 1;

                    if (bit == 1)
                        value += step;
                    else
                        value -= step;

                    value = Math.Max(
                        short.MinValue,
                        Math.Min(short.MaxValue, value));

                    samples.Add((short)value);

                    if (bit == previousBit)
                        step = Math.Min(10000, (int)(step * 1.5));
                    else
                        step = Math.Max(10, (int)(step * 0.5));

                    previousBit = bit;

                    processedSamples++;

                    reportProgress?.Invoke(
                        (processedSamples * 100) / originalSampleCount);
                }
            }

            return samples;
        }

        public static List<byte> CompressPDC(
            CancellationToken token,
          float[] samples,
          CompressionSettings settings,
          Action<CompressionProgressInfo> reportProgress = null)
        {
            List<byte> compressed = new List<byte>();

            int prev1 = 0;
            int prev2 = 0;
            int prev3 = 0;

            int levels = settings.QuantizationLevels;

            int bitsPerSample = (int)Math.Log(levels, 2);
            float interval = 65535f / levels;

            int maxLevel = (levels / 2) - 1;
            int minLevel = -(levels / 2);

            byte currentByte = 0;
            int bitPosition = 0;

            Stopwatch sw = Stopwatch.StartNew();
            CompressionProgressInfo info = new CompressionProgressInfo();

            for (int i = 0; i < samples.Length; i++)
            {
                token.ThrowIfCancellationRequested();
                int current = ToPCM(samples[i]);

                int predicted =
                    (int)((0.6f * prev1) +
                          (0.3f * prev2) +
                          (0.1f * prev3));

                int error = current - predicted;

                int quantizedError =
                    (int)Math.Round(error / interval);

                quantizedError =
                    Math.Max(minLevel,
                    Math.Min(maxLevel, quantizedError));

                int packedValue =
                    quantizedError - minLevel;

                for (int b = 0; b < bitsPerSample; b++)
                {
                    int bit = (packedValue >> b) & 1;

                    currentByte |=
                        (byte)(bit << bitPosition);

                    bitPosition++;

                    if (bitPosition == 8)
                    {
                        compressed.Add(currentByte);

                        currentByte = 0;
                        bitPosition = 0;
                    }
                }

                int reconstructedError =
                    (int)(quantizedError * interval);

                int currentReconstructed =
                    predicted + reconstructedError;

                currentReconstructed =
                    Math.Max(short.MinValue,
                    Math.Min(short.MaxValue,
                    currentReconstructed));

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

            if (bitPosition > 0)
            {
                compressed.Add(currentByte);
            }

            sw.Stop();

            return compressed;
        }
        public static List<short> DecompressPDC(
          List<byte> data,
          CompressionSettings settings,
          int originalSampleCount,
          Action<int> reportProgress = null)
        {
            List<short> samples = new List<short>();

            int prev1 = 0;
            int prev2 = 0;
            int prev3 = 0;

            int levels = settings.QuantizationLevels;
            int bitsPerSample = (int)Math.Log(levels, 2);
            float interval = 65535f / levels;

            int minLevel = -(levels / 2);

            int currentValue = 0;
            int bitsRead = 0;
            int processedSamples = 0;

            foreach (byte currentByte in data)
            {
                for (int bitPosition = 0; bitPosition < 8; bitPosition++)
                {
                    int bit = (currentByte >> bitPosition) & 1;

                    currentValue |= (bit << bitsRead);
                    bitsRead++;

                    if (bitsRead == bitsPerSample)
                    {
                        int qError = currentValue + minLevel;

                        int predicted =
                            (int)((0.6f * prev1) +
                                  (0.3f * prev2) +
                                  (0.1f * prev3));

                        int reconstructedError =
                            (int)(qError * interval);

                        int currentReconstructed =
                            predicted + reconstructedError;

                        currentReconstructed =
                            Math.Max(short.MinValue,
                            Math.Min(short.MaxValue,
                            currentReconstructed));

                        samples.Add((short)currentReconstructed);

                        prev3 = prev2;
                        prev2 = prev1;
                        prev1 = currentReconstructed;

                        processedSamples++;

                        reportProgress?.Invoke(
                            (processedSamples * 100) /
                            originalSampleCount);

                        currentValue = 0;
                        bitsRead = 0;

                        if (processedSamples >= originalSampleCount)
                            return samples;
                    }
                }
            }

            return samples;
        }

        public static List<byte> CompressNonlinearQuantization(
            CancellationToken token,
            float[] samples,
            CompressionSettings settings,
            Action<CompressionProgressInfo> reportProgress = null)
        {
            List<byte> compressed = new List<byte>();

            const float mu = 255f;

            byte currentByte = 0;
            int bitPosition = 0;

            Stopwatch sw = Stopwatch.StartNew();
            CompressionProgressInfo info = new CompressionProgressInfo();

            for (int i = 0; i < samples.Length; i++)
            {
                token.ThrowIfCancellationRequested();
                float x = samples[i];
                float compressedValue =
                    Math.Sign(x) *
                    (float)(Math.Log(1 + mu * Math.Abs(x)) / Math.Log(1 + mu));

                int quantized = (int)((compressedValue + 1f) * 127.5f);

                quantized = Math.Max(0, Math.Min(255, quantized));
                compressed.Add((byte)quantized);

                if (i % 200 == 0 || i == samples.Length - 1)
                {
                    info.ProcessedSamples = i;
                    info.TotalSamples = samples.Length;
                    info.ElapsedSeconds = sw.Elapsed.TotalSeconds;
                    info.CompressedSize = compressed.Count;

                    reportProgress?.Invoke(info);
                }
            }

            if (bitPosition > 0)
                compressed.Add(currentByte);

            sw.Stop();
            return compressed;
        }
        public static List<short> DecompressNonlinearQuantization(
           List<byte> data,
           int originalSampleCount,
           Action<int> reportProgress = null)
        {
            List<short> samples = new List<short>();

            const float mu = 255f;

            int processed = 0;

            foreach (byte quantized in data)
            {
                float x = (quantized / 127.5f) - 1f;

                x = Math.Max(-1f, Math.Min(1f, x));

                float expanded =
                    Math.Sign(x) *
                    (1f / mu) *
                    ((float)Math.Pow(1 + mu, Math.Abs(x)) - 1);

                samples.Add((short)(expanded * 32767));

                processed++;

                reportProgress?.Invoke(
                    (processed * 100) / originalSampleCount);

                if (processed >= originalSampleCount)
                    break;
            }

            return samples;
        }
    }
}