using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using System.Collections.Generic;

namespace AudioCompressionProject
{
    public partial class Form1 : Form
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        public Form1()
        {
            InitializeComponent();
            txtFilePath.ReadOnly = true;
            chartSpeed.Series.Clear();
            chartSpeed.Series.Add("Speed");
            chartSpeed.Series["Speed"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            
            chartSpeed.ChartAreas[0].AxisX.Title = "Progress (%)";
            chartSpeed.ChartAreas[0].AxisY.Title = "Samples / Sec";

            
            chartRatio.Series.Clear();
            chartRatio.Series.Add("Ratio");
            chartRatio.Series["Ratio"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            
            chartRatio.ChartAreas[0].AxisX.Title = "Progress (%)";
            chartRatio.ChartAreas[0].AxisY.Title = "Compression Ratio (x)";
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            outputDevice?.Dispose();
            audioFile?.Dispose();
            base.OnFormClosing(e);
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter ="Audio Files (*.wav;*.mp3;*.aac;*.flac)|*.wav;*.mp3;*.aac;*.flac";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
                DisplayAudioProperties(openFileDialog.FileName);
            }
        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                string filePath = files[0];
                txtFilePath.Text = filePath;

                DisplayAudioProperties(filePath);
            }
        }
        private void DisplayAudioProperties(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                double fileSizeInMB = (double)fileInfo.Length / (1024 * 1024);
                lblFileSize.Text = $"File Size: {fileSizeInMB:F2} MB";

                using (var audioReader = new AudioFileReader(filePath))
                {
                    TimeSpan duration = audioReader.TotalTime;
                    lblDuration.Text = $"Duration: {duration:hh\\:mm\\:ss}";

                    lblSamplingRate.Text = $"Sampling Rate: {audioReader.WaveFormat.SampleRate} Hz";

                    int channels = audioReader.WaveFormat.Channels;
                    string channelType = (channels == 2) ? "Stereo" : (channels == 1) ? "Mono" : $"{channels} Channels";
                    lblChannels.Text = $"Channels: {channelType}";

                    lblBitRate.Text = $"Bit Rate: {audioReader.WaveFormat.AverageBytesPerSecond * 8 / 1000} kbps";

                    lblCodec.Text = $"Audio Codec: {audioReader.WaveFormat.Encoding.ToString()}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading audio properties: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text))
            {
                MessageBox.Show("Please select an audio file first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                if (outputDevice != null)
                {
                    outputDevice.Dispose();
                    outputDevice = null;
                }
                if (audioFile != null)
                {
                    audioFile.Dispose();
                    audioFile = null;
                }
                outputDevice = new WaveOutEvent();
                audioFile = new AudioFileReader(txtFilePath.Text);
                outputDevice.Init(audioFile);
                outputDevice.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing audio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (outputDevice != null)
            {
                outputDevice.Pause();
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (outputDevice != null)
            {
                outputDevice.Stop();
                outputDevice.Dispose();
                outputDevice = null;
            }
            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
        public static float[] ApplySampleRate(
           float[] samples,
           int originalSampleRate,
           int targetSampleRate)
        {
            if (targetSampleRate <= 0 || targetSampleRate == originalSampleRate)
                return samples;

            double ratio =
                (double)originalSampleRate / targetSampleRate;

            List<float> result = new List<float>();

            for (double i = 0; i < samples.Length; i += ratio)
            {
                int index = (int)i;

                if (index < samples.Length)
                    result.Add(samples[index]);
            }

            return result.ToArray();
        }
        CompressionSettings settings = new CompressionSettings();
        private void btnCompress_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text))
            {
                MessageBox.Show("Please select an audio file first.");
                return;
            }

            if (cmbAlgorithm.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a compression algorithm.");
                return;
            }

            try
            {
                List<float> samples = new List<float>();

                int sampleRate = 0;
                int channels = 0;

                using (AudioFileReader reader = new AudioFileReader(txtFilePath.Text))
                {
                    sampleRate = reader.WaveFormat.SampleRate;
                    channels = reader.WaveFormat.Channels;

                    float[] buffer = new float[1024];
                    int read;

                    while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        for (int i = 0; i < read; i++)
                            samples.Add(buffer[i]);
                    }
                }

               

                List<short> compressedData = null;
               
                try
                {


                    settings.Algorithm = cmbAlgorithm.SelectedItem?.ToString();
                    settings.SampleRate = int.Parse(cmbSampleRate.SelectedItem.ToString());                 
                    settings.QuantizationLevels = cmbQuantization.SelectedItem != null
                    ? int.Parse(cmbQuantization.SelectedItem.ToString())
                    : 0;
                    settings.StepSize = (int)numStepSize.Value;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                float[] processedSamples =
                         ApplySampleRate(
                         samples.ToArray(),
                         sampleRate,
                         settings.SampleRate);
                progressBar1.Value = 0;
                chartSpeed.Series["Speed"].Points.Clear();
                chartRatio.Series["Ratio"].Points.Clear();

                Action<CompressionProgressInfo> progressCallback = info =>
                {
                   
                    int progress = (int)((info.ProcessedSamples + 1) * 100.0 / info.TotalSamples);
                    progressBar1.Value = Math.Min(100, Math.Max(0, progress));

                    double speed = info.ElapsedSeconds > 0 ? (info.ProcessedSamples / info.ElapsedSeconds) : 0;
                   
                    double originalSizeBytes = (info.ProcessedSamples + 1) * 2; 
                    double compressedSizeBytes = info.CompressedSize * 2;       
                    double ratio = compressedSizeBytes > 0 ? (originalSizeBytes / compressedSizeBytes) : 1.0;
                   
                    int currentProgressPercent = (int)(info.ProcessedSamples * 100.0 / info.TotalSamples);
                  
                    if (info.ProcessedSamples % 5000 == 0 || info.ProcessedSamples == info.TotalSamples - 1)
                    {
                        chartSpeed.Series["Speed"].Points.AddXY(currentProgressPercent, speed);
                        chartRatio.Series["Ratio"].Points.AddXY(currentProgressPercent, ratio);
                    }

                    Application.DoEvents();
                };

                switch (settings.Algorithm)
                {
                    case "DPCM":
                        compressedData = AudioCompressor.CompressDPCM(processedSamples, settings, progressCallback);
                        break;

                    case "Delta Modulation":
                        compressedData = AudioCompressor.CompressDelta(processedSamples, settings, progressCallback);
                        break;

                    case "Adaptive Delta Modulation":
                        compressedData = AudioCompressor.CompressAdaptiveDelta(processedSamples, settings, progressCallback);
                        break;

                    case "Predictive Differential Coding":
                        compressedData = AudioCompressor.CompressPDC(processedSamples, settings, progressCallback);
                        break;

                    case "Nonlinear Quantization":
                        compressedData = AudioCompressor.CompressNonlinearQuantization(processedSamples, settings, progressCallback);
                        break;
                }

                if (compressedData == null || compressedData.Count == 0)
                {
                    MessageBox.Show("Compression failed.");
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Compressed File (*.cmp)|*.cmp";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (BinaryWriter writer =
                           new BinaryWriter(File.Open(saveDialog.FileName, FileMode.Create)))
                    {
                        writer.Write(settings.SampleRate);
                        writer.Write(channels);
                        writer.Write(compressedData.Count);

                        foreach (short value in compressedData)
                            writer.Write(value);
                    }

                    MessageBox.Show("Compression completed successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDecompress_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;

            if (cmbAlgorithm.SelectedIndex == -1)
            {
                MessageBox.Show("Please select the algorithm used for compression.");
                return;
            }

            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Compressed File (*.cmp)|*.cmp";

            if (open.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                List<short> data = new List<short>();

                int sampleRate;
                int channels;
                int count;

                using (BinaryReader reader =
                       new BinaryReader(File.Open(open.FileName, FileMode.Open)))
                {
                    if (reader.BaseStream.Length < 12)
                    {
                        MessageBox.Show("Invalid compressed file.");
                        return;
                    }

                    sampleRate = reader.ReadInt32();
                    channels = reader.ReadInt32();
                    count = reader.ReadInt32();

 
                    if (count <= 0 || count > reader.BaseStream.Length)
                    {
                        MessageBox.Show("Corrupted file data.");
                        return;
                    }

                    for (int i = 0; i < count; i++)
                        data.Add(reader.ReadInt16());
                }
                settings.StepSize = (int)numStepSize.Value;

                List<short> result = null;

                string algorithm = cmbAlgorithm.SelectedItem.ToString();

                switch (algorithm)
                {
                    case "DPCM":
                        result = AudioCompressor.DecompressDPCM(data , settings , progress =>
                        {
                            progressBar1.Value = progress;
                            Application.DoEvents();
                        });
                        break;

                    case "Delta Modulation":
                        result = AudioCompressor.DecompressDelta(data , settings , progress =>
                        {
                            progressBar1.Value = progress;
                            Application.DoEvents();
                        });
                        break;

                    case "Adaptive Delta Modulation":
                        result = AudioCompressor.DecompressAdaptiveDelta(data,settings, progress =>
                        {
                            progressBar1.Value = progress;
                            Application.DoEvents();
                        });
                        break;

                    case "Predictive Differential Coding":
                        result = AudioCompressor.DecompressPDC(data,settings, progress =>
                        {
                            progressBar1.Value = progress;
                            Application.DoEvents();
                        });
                        break;

                    case "Nonlinear Quantization":
                        result = AudioCompressor.DecompressNonlinearQuantization(data, progress =>
                        {
                            progressBar1.Value = progress;
                            Application.DoEvents();
                        });
                        break;
                }

                if (result == null || result.Count == 0)
                {
                    MessageBox.Show("Decompression failed.");
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Wave File (*.wav)|*.wav";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (WaveFileWriter writer =
                           new WaveFileWriter(
                               saveDialog.FileName,
                               new WaveFormat(sampleRate, 16, channels)))
                    {
                        foreach (short sample in result)
                            writer.WriteSample(sample / 32768f);
                    }

                    MessageBox.Show("Audio file restored successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            string algorithm =
       cmbAlgorithm.SelectedItem.ToString();

            switch (algorithm)
            {
                case "DPCM":
                case "Predictive Differential Coding":
                case "Nonlinear Quantization":

                    lblQuantization.Visible = true; 
                    cmbQuantization.Visible = true;

                    lblStepSize.Visible = false;
                    numStepSize.Visible = false;

                    break;

                case "Delta Modulation":
                case "Adaptive Delta Modulation":

                    lblQuantization.Visible = false;
                    cmbQuantization.Visible = false;

                    lblStepSize.Visible = true;
                    numStepSize.Visible = true;

                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void chartRatio_Click(object sender, EventArgs e)
        {

        }
    }
}