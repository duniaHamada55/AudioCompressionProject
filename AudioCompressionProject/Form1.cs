using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave; 

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
    }
}