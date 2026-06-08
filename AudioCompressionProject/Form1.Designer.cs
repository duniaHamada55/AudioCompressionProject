
namespace AudioCompressionProject
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCodec = new System.Windows.Forms.Label();
            this.lblBitRate = new System.Windows.Forms.Label();
            this.lblChannels = new System.Windows.Forms.Label();
            this.lblSamplingRate = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblFileSize = new System.Windows.Forms.Label();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCompress = new System.Windows.Forms.Button();
            this.btnDecompress = new System.Windows.Forms.Button();
            this.cmbSampleRate = new System.Windows.Forms.ComboBox();
            this.numStepSize = new System.Windows.Forms.NumericUpDown();
            this.cmbQuantization = new System.Windows.Forms.ComboBox();
            this.lblSampleRate = new System.Windows.Forms.Label();
            this.lblQuantization = new System.Windows.Forms.Label();
            this.lblStepSize = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.chartSpeed = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartRatio = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStepSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRatio)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(1, 82);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(126, 57);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(0, 712);
            this.txtFilePath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(589, 27);
            this.txtFilePath.TabIndex = 1;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(0, 158);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(126, 39);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(0, 218);
            this.btnPause.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(125, 38);
            this.btnPause.TabIndex = 3;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(0, 276);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(125, 33);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCodec);
            this.groupBox1.Controls.Add(this.lblBitRate);
            this.groupBox1.Controls.Add(this.lblChannels);
            this.groupBox1.Controls.Add(this.lblSamplingRate);
            this.groupBox1.Controls.Add(this.lblDuration);
            this.groupBox1.Controls.Add(this.lblFileSize);
            this.groupBox1.Location = new System.Drawing.Point(198, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(688, 431);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Audio file properties";
            // 
            // lblCodec
            // 
            this.lblCodec.AutoSize = true;
            this.lblCodec.Location = new System.Drawing.Point(60, 341);
            this.lblCodec.Name = "lblCodec";
            this.lblCodec.Size = new System.Drawing.Size(52, 19);
            this.lblCodec.TabIndex = 5;
            this.lblCodec.Text = "Codec";
            // 
            // lblBitRate
            // 
            this.lblBitRate.AutoSize = true;
            this.lblBitRate.Location = new System.Drawing.Point(385, 341);
            this.lblBitRate.Name = "lblBitRate";
            this.lblBitRate.Size = new System.Drawing.Size(58, 19);
            this.lblBitRate.TabIndex = 4;
            this.lblBitRate.Text = "BitRate";
            // 
            // lblChannels
            // 
            this.lblChannels.AutoSize = true;
            this.lblChannels.Location = new System.Drawing.Point(47, 184);
            this.lblChannels.Name = "lblChannels";
            this.lblChannels.Size = new System.Drawing.Size(73, 19);
            this.lblChannels.TabIndex = 3;
            this.lblChannels.Text = "Channels";
            // 
            // lblSamplingRate
            // 
            this.lblSamplingRate.AutoSize = true;
            this.lblSamplingRate.Location = new System.Drawing.Point(360, 184);
            this.lblSamplingRate.Name = "lblSamplingRate";
            this.lblSamplingRate.Size = new System.Drawing.Size(111, 19);
            this.lblSamplingRate.TabIndex = 2;
            this.lblSamplingRate.Text = "Sampling Rate";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(360, 66);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(74, 19);
            this.lblDuration.TabIndex = 1;
            this.lblDuration.Text = "lDuration";
            // 
            // lblFileSize
            // 
            this.lblFileSize.AutoSize = true;
            this.lblFileSize.Location = new System.Drawing.Point(47, 66);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new System.Drawing.Size(61, 19);
            this.lblFileSize.TabIndex = 0;
            this.lblFileSize.Text = "FileSize";
            this.lblFileSize.Click += new System.EventHandler(this.label1_Click);
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.Items.AddRange(new object[] {
            "DPCM",
            "Delta Modulation",
            "Adaptive Delta Modulation",
            "Predictive Differential Coding",
            "Nonlinear Quantization"});
            this.cmbAlgorithm.Location = new System.Drawing.Point(0, 370);
            this.cmbAlgorithm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(162, 27);
            this.cmbAlgorithm.TabIndex = 6;
            this.cmbAlgorithm.SelectedIndexChanged += new System.EventHandler(this.cmbAlgorithm_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 331);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "Compression Algorithm";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // btnCompress
            // 
            this.btnCompress.Location = new System.Drawing.Point(0, 603);
            this.btnCompress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCompress.Name = "btnCompress";
            this.btnCompress.Size = new System.Drawing.Size(125, 30);
            this.btnCompress.TabIndex = 8;
            this.btnCompress.Text = "compress";
            this.btnCompress.UseVisualStyleBackColor = true;
            this.btnCompress.Click += new System.EventHandler(this.btnCompress_Click);
            // 
            // btnDecompress
            // 
            this.btnDecompress.Location = new System.Drawing.Point(4, 653);
            this.btnDecompress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDecompress.Name = "btnDecompress";
            this.btnDecompress.Size = new System.Drawing.Size(123, 32);
            this.btnDecompress.TabIndex = 9;
            this.btnDecompress.Text = "Decompress";
            this.btnDecompress.UseVisualStyleBackColor = true;
            this.btnDecompress.Click += new System.EventHandler(this.btnDecompress_Click);
            // 
            // cmbSampleRate
            // 
            this.cmbSampleRate.FormattingEnabled = true;
            this.cmbSampleRate.Items.AddRange(new object[] {
            "44100",
            "",
            "22050",
            "",
            "11025",
            "",
            "8000"});
            this.cmbSampleRate.Location = new System.Drawing.Point(-1, 436);
            this.cmbSampleRate.Margin = new System.Windows.Forms.Padding(2);
            this.cmbSampleRate.Name = "cmbSampleRate";
            this.cmbSampleRate.Size = new System.Drawing.Size(92, 27);
            this.cmbSampleRate.TabIndex = 10;
            this.cmbSampleRate.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // numStepSize
            // 
            this.numStepSize.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numStepSize.Location = new System.Drawing.Point(-1, 555);
            this.numStepSize.Margin = new System.Windows.Forms.Padding(2);
            this.numStepSize.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numStepSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStepSize.Name = "numStepSize";
            this.numStepSize.Size = new System.Drawing.Size(90, 27);
            this.numStepSize.TabIndex = 11;
            this.numStepSize.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // cmbQuantization
            // 
            this.cmbQuantization.FormattingEnabled = true;
            this.cmbQuantization.Items.AddRange(new object[] {
            "2",
            "4",
            "8",
            "16",
            "32",
            "64",
            "128",
            "256"});
            this.cmbQuantization.Location = new System.Drawing.Point(1, 499);
            this.cmbQuantization.Margin = new System.Windows.Forms.Padding(2);
            this.cmbQuantization.Name = "cmbQuantization";
            this.cmbQuantization.Size = new System.Drawing.Size(92, 27);
            this.cmbQuantization.TabIndex = 12;
            this.cmbQuantization.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // lblSampleRate
            // 
            this.lblSampleRate.AutoSize = true;
            this.lblSampleRate.Location = new System.Drawing.Point(1, 407);
            this.lblSampleRate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSampleRate.Name = "lblSampleRate";
            this.lblSampleRate.Size = new System.Drawing.Size(98, 19);
            this.lblSampleRate.TabIndex = 13;
            this.lblSampleRate.Text = "samples rate";
            // 
            // lblQuantization
            // 
            this.lblQuantization.AutoSize = true;
            this.lblQuantization.Location = new System.Drawing.Point(-2, 470);
            this.lblQuantization.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQuantization.Name = "lblQuantization";
            this.lblQuantization.Size = new System.Drawing.Size(146, 19);
            this.lblQuantization.TabIndex = 14;
            this.lblQuantization.Text = "Quantization Levels";
            // 
            // lblStepSize
            // 
            this.lblStepSize.AutoSize = true;
            this.lblStepSize.Location = new System.Drawing.Point(-2, 526);
            this.lblStepSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStepSize.Name = "lblStepSize";
            this.lblStepSize.Size = new System.Drawing.Size(73, 19);
            this.lblStepSize.TabIndex = 15;
            this.lblStepSize.Text = "Step Size";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(147, 619);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(292, 37);
            this.progressBar1.TabIndex = 16;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // chartSpeed
            // 
            chartArea1.AxisX.Interval = 20D;
            chartArea1.AxisX.Maximum = 100D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.Name = "ChartArea1";
            this.chartSpeed.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartSpeed.Legends.Add(legend1);
            this.chartSpeed.Location = new System.Drawing.Point(606, 499);
            this.chartSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.chartSpeed.Name = "chartSpeed";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartSpeed.Series.Add(series1);
            this.chartSpeed.Size = new System.Drawing.Size(593, 256);
            this.chartSpeed.TabIndex = 17;
            this.chartSpeed.Text = "chart1";
            // 
            // chartRatio
            // 
            chartArea2.AxisX.Interval = 20D;
            chartArea2.AxisX.Maximum = 100D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.Name = "ChartArea1";
            this.chartRatio.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartRatio.Legends.Add(legend2);
            this.chartRatio.Location = new System.Drawing.Point(1216, 499);
            this.chartRatio.Margin = new System.Windows.Forms.Padding(2);
            this.chartRatio.Name = "chartRatio";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartRatio.Series.Add(series2);
            this.chartRatio.Size = new System.Drawing.Size(534, 256);
            this.chartRatio.TabIndex = 18;
            this.chartRatio.Text = "chart1";
            this.chartRatio.Click += new System.EventHandler(this.chartRatio_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(990, 60);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 49);
            this.button1.TabIndex = 19;
            this.button1.Text = "cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(992, 143);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 53);
            this.button2.TabIndex = 20;
            this.button2.Text = "Reset";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1617, 775);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chartRatio);
            this.Controls.Add(this.chartSpeed);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblStepSize);
            this.Controls.Add(this.lblQuantization);
            this.Controls.Add(this.lblSampleRate);
            this.Controls.Add(this.cmbQuantization);
            this.Controls.Add(this.numStepSize);
            this.Controls.Add(this.cmbSampleRate);
            this.Controls.Add(this.btnDecompress);
            this.Controls.Add(this.btnCompress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbAlgorithm);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnBrowse);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStepSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRatio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblFileSize;
        private System.Windows.Forms.Label lblCodec;
        private System.Windows.Forms.Label lblBitRate;
        private System.Windows.Forms.Label lblChannels;
        private System.Windows.Forms.Label lblSamplingRate;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.ComboBox cmbAlgorithm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCompress;
        private System.Windows.Forms.Button btnDecompress;
        private System.Windows.Forms.ComboBox cmbSampleRate;
        private System.Windows.Forms.NumericUpDown numStepSize;
        private System.Windows.Forms.ComboBox cmbQuantization;
        private System.Windows.Forms.Label lblSampleRate;
        private System.Windows.Forms.Label lblQuantization;
        private System.Windows.Forms.Label lblStepSize;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSpeed;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRatio;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

