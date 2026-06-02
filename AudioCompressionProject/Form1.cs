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

                string algorithm = cmbAlgorithm.SelectedItem.ToString();

                switch (algorithm)
                {
                    case "DPCM":
                        compressedData = AudioCompressor.CompressDPCM(samples.ToArray());
                        break;

                    case "Delta Modulation":
                        compressedData = AudioCompressor.CompressDelta(samples.ToArray());
                        break;

                    case "Adaptive Delta Modulation":
                        compressedData = AudioCompressor.CompressAdaptiveDelta(samples.ToArray());
                        break;

                    case "Predictive Differential Coding":
                        compressedData = AudioCompressor.CompressPDC(samples.ToArray());
                        break;

                    case "Nonlinear Quantization":
                        compressedData = AudioCompressor.CompressNonlinearQuantization(samples.ToArray());
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
                        writer.Write(sampleRate);
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

                List<short> result = null;

                string algorithm = cmbAlgorithm.SelectedItem.ToString();

                switch (algorithm)
                {
                    case "DPCM":
                        result = AudioCompressor.DecompressDPCM(data);
                        break;

                    case "Delta Modulation":
                        result = AudioCompressor.DecompressDelta(data);
                        break;

                    case "Adaptive Delta Modulation":
                        result = AudioCompressor.DecompressAdaptiveDelta(data);
                        break;

                    case "Predictive Differential Coding":
                        result = AudioCompressor.DecompressPDC(data);
                        break;

                    case "Nonlinear Quantization":
                        result = AudioCompressor.DecompressNonlinearQuantization(data);
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
    }
}