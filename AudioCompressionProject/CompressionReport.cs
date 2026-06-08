using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace AudioCompressionProject
{
    public static class CompressionReport
    {
        public static void GenerateReport(
            string originalPath,
            string compressedPath,
            string algorithm,
            CompressionSettings settings,
            double elapsedSeconds)
        {
            try
            {
                FileInfo originalInfo = new FileInfo(originalPath);
                FileInfo compressedInfo = new FileInfo(compressedPath);

                if (!originalInfo.Exists || !compressedInfo.Exists)
                {
                    MessageBox.Show(
                        "Unable to generate report because one or more files are missing.",
                        "Report Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                double originalMb =
                    originalInfo.Length / 1024.0 / 1024.0;

                double compressedMb =
                    compressedInfo.Length / 1024.0 / 1024.0;

                double savedPercentage =
                    (1.0 - ((double)compressedInfo.Length / originalInfo.Length)) * 100.0;

                double ratio =
                    (double)originalInfo.Length / compressedInfo.Length;

                string encodedBits =
                    GetEncodedBitsPerSample(algorithm, settings);

                string algorithmRows =
                    GetAlgorithmSpecificRows(algorithm, settings);

              

                string html = $@"
<!DOCTYPE html>
<html lang='en'>
<head>

<meta charset='UTF-8'>

<title>Audio Compression Report</title>

<style>

body
{{
    margin:0;
    padding:40px;
    background:#f4f6f9;
    font-family:'Segoe UI',Tahoma;
    color:#2c3e50;
}}

.report
{{
    max-width:900px;
    margin:auto;
    background:white;
    border-radius:16px;
    overflow:hidden;
    box-shadow:0 8px 30px rgba(0,0,0,0.08);
}}

.header
{{
    background:linear-gradient(135deg,#1e3c72,#2a5298);
    color:white;
    padding:35px;
    text-align:center;
}}

.header h1
{{
    margin:0;
    font-size:34px;
    letter-spacing:1px;
}}

.header p
{{
    margin-top:10px;
    opacity:0.9;
}}

.summary
{{
    display:flex;
    flex-wrap:wrap;
    justify-content:center;
    gap:20px;
    padding:30px;
    background:#fafbfd;
}}

.card
{{
    width:180px;
    background:white;
    border-radius:12px;
    padding:20px;
    text-align:center;
    box-shadow:0 3px 10px rgba(0,0,0,0.06);
}}

.card .value
{{
    font-size:28px;
    font-weight:bold;
    color:#2a5298;
}}

.card .label
{{
    margin-top:10px;
    color:#555;
    font-size:14px;
}}

.section-title
{{
    padding:25px 35px 10px 35px;
    font-size:24px;
    font-weight:600;
    color:#1e3c72;
}}

table
{{
    width:92%;
    margin:0 auto 35px auto;
    border-collapse:collapse;
}}

th
{{
    background:#2a5298;
    color:white;
    padding:14px;
    text-align:left;
}}

td
{{
    padding:13px;
    border-bottom:1px solid #e5e7eb;
}}

tr:nth-child(even)
{{
    background:#f9fbfd;
}}

tr:hover
{{
    background:#eef5ff;
}}

.good
{{
    color:#16a34a;
    font-weight:bold;
}}

.algorithm
{{
    color:#2563eb;
    font-weight:bold;
}}

.footer
{{
    text-align:center;
    padding:20px;
    background:#f8fafc;
    color:#666;
    font-size:13px;
    border-top:1px solid #e5e7eb;
}}

</style>

</head>

<body>

<div class='report'>

<div class='header'>
    <h1>Audio Compression Report</h1>
    <p>Faculty of Information Engineering — Multimedia Project 2026</p>
</div>

<div class='summary'>

    <div class='card'>
        <div class='value'>{originalMb:F2} MB</div>
        <div class='label'>Original Size</div>
    </div>

    <div class='card'>
        <div class='value'>{compressedMb:F2} MB</div>
        <div class='label'>Compressed Size</div>
    </div>

    <div class='card'>
        <div class='value good'>{savedPercentage:F2}%</div>
        <div class='label'>Space Saved</div>
    </div>

    <div class='card'>
        <div class='value'>{elapsedSeconds:F2}s</div>
        <div class='label'>Compression Time</div>
    </div>

</div>

<div class='section-title'>
Compression Details
</div>

<table>

<tr>
    <th>Property</th>
    <th>Value</th>
</tr>

<tr>
    <td>Source File</td>
    <td>{Path.GetFileName(originalPath)}</td>
</tr>

<tr>
    <td>Compression Algorithm</td>
    <td class='algorithm'>{algorithm}</td>
</tr>

<tr>
    <td>Original File Size</td>
    <td>{originalMb:F2} MB ({originalInfo.Length:N0} bytes)</td>
</tr>

<tr>
    <td>Compressed File Size</td>
    <td>{compressedMb:F2} MB ({compressedInfo.Length:N0} bytes)</td>
</tr>

<tr>
    <td>Compression Ratio</td>
    <td>{ratio:F2} : 1</td>
</tr>

<tr>
    <td>Space Saving Percentage</td>
    <td class='good'>{savedPercentage:F2}%</td>
</tr>

<tr>
    <td>Compression Execution Time</td>
    <td>{elapsedSeconds:F4} seconds</td>
</tr>

<tr>
    <td>Configured Sampling Rate</td>
    <td>{settings.SampleRate:N0} Hz</td>
</tr>

<tr>
    <td>Encoded Bits Per Sample</td>
    <td>{encodedBits}</td>
</tr>

{algorithmRows}

<tr>
    <td>Report Generated</td>
    <td>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</td>
</tr>

</table>



</div>

</body>
</html>";


                string desktop =
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.Desktop);

                string fileName =
                    $"AudioCompressionReport_{DateTime.Now:yyyyMMdd_HHmmss}.html";

                string fullPath =
                    Path.Combine(desktop, fileName);

                File.WriteAllText(fullPath, html);

              

                Process.Start(new ProcessStartInfo
                {
                    FileName = fullPath,
                    UseShellExecute = true
                });

                MessageBox.Show(
                    "Professional compression report generated successfully.",
                    "Report Generated",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to generate report:\n\n" + ex.Message,
                    "Report Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private static string GetEncodedBitsPerSample(
            string algorithm,
            CompressionSettings settings)
        {
            if (string.IsNullOrWhiteSpace(algorithm))
                return "N/A";

            if (algorithm.Contains("Delta"))
            {
                return "1 bit / sample";
            }

            if (algorithm.Equals("DPCM", StringComparison.OrdinalIgnoreCase)
                || algorithm.Contains("Predictive"))
            {
                if (settings.QuantizationLevels > 0 &&
                    IsPowerOfTwo(settings.QuantizationLevels))
                {
                    int bits =
                        (int)Math.Log(settings.QuantizationLevels, 2);

                    return $"{bits} bits / sample";
                }

                return "Variable";
            }

            if (algorithm.Contains("Nonlinear"))
            {
                return "8 bits / sample (μ-law logarithmic companding)";
            }

            return "N/A";
        }

        

        private static string GetAlgorithmSpecificRows(
            string algorithm,
            CompressionSettings settings)
        {
            if (algorithm.Contains("Adaptive Delta"))
            {
                return $@"
<tr>
    <td>Initial Step Size (δ)</td>
    <td>{settings.StepSize}</td>
</tr>

<tr>
    <td>Adaptation Strategy</td>
    <td>Dynamic Step Adjustment</td>
</tr>";
            }

            if (algorithm.Contains("Delta"))
            {
                return $@"
<tr>
    <td>Fixed Step Size (δ)</td>
    <td>{settings.StepSize}</td>
</tr>";
            }

            if (algorithm.Equals("DPCM", StringComparison.OrdinalIgnoreCase)
                || algorithm.Contains("Predictive"))
            {
                return $@"
<tr>
    <td>Quantization Levels (L)</td>
    <td>{settings.QuantizationLevels}</td>
</tr>";
            }

            if (algorithm.Contains("Nonlinear"))
            {
                return $@"
<tr>
    <td>Companding Technique</td>
    <td>μ-law Nonlinear Quantization</td>
</tr>";
            }

            return "";
        }

       

        private static bool IsPowerOfTwo(int value)
        {
            return value > 0 && (value & (value - 1)) == 0;
        }
    }
}