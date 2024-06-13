using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace PythonExecutorLibrary
{
    public class PythonExecutor
    {
        public TimeSpan Interval { get; set; }
        public DateTime LastExecutionTime { get; set; }

        public PythonExecutor(TimeSpan _dateTimeOffset)
        {
            LastExecutionTime = DateTime.MinValue;
            Interval = _dateTimeOffset;
        }

        public PythonExecutor()
        {
            LastExecutionTime = DateTime.MinValue;
            Interval = TimeSpan.Zero;
        }


        public async Task<(string output, string error)> ExecutePythonCodeAsync(string code, string fullPythonPath)
        {

            if(LastExecutionTime.Add(Interval) > DateTime.Now)
            {
                throw new InvalidOperationException($"This method can only be called once every {Interval} minutes.");
            }

            LastExecutionTime = DateTime.Now;

            var output = new StringBuilder();
            var error = new StringBuilder();

            var startInfo = new ProcessStartInfo
            {
                FileName = fullPythonPath,
                Arguments = $"-c \"{code.Replace("\"", "\\\"")}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                process.OutputDataReceived += (sender, args) => output.AppendLine(args.Data);
                process.ErrorDataReceived += (sender, args) => error.AppendLine(args.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();

                return (output.ToString(), error.ToString());
            }
        }
    }

   
}
