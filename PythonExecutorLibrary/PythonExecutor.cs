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

        public int TimeLimitInSeconds { get; set; }

        public PythonExecutor(TimeSpan _dateTimeOffset, int processTimeLimitInSeconds)
        {
            LastExecutionTime = DateTime.MinValue;
            Interval = _dateTimeOffset;
            TimeLimitInSeconds = processTimeLimitInSeconds;

        }

        public PythonExecutor(int processTimeLimitInSeconds)
        {
            LastExecutionTime = DateTime.MinValue;
            Interval = TimeSpan.Zero;
            TimeLimitInSeconds = processTimeLimitInSeconds;
        }


        public async Task<(string output, string error)> ExecutePythonCodeAsync(string code, string fullPythonPath)
        {
            if (LastExecutionTime.Add(Interval) > DateTime.Now)
            {
                throw new InvalidOperationException($"This method can only be called once every {Interval} minutes.");
            }

            LastExecutionTime = DateTime.Now;

            var output = new StringBuilder();
            var error = new StringBuilder();

            var startInfo = new ProcessStartInfo
            {
                FileName = fullPythonPath,
                Arguments = $"-c \"{code}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                process.OutputDataReceived += (sender, args) => output.AppendLine(args.Data);
                process.ErrorDataReceived += (sender, args) => error.AppendLine(args.Data);

                var cts = new CancellationTokenSource();
                var killTask = Task.Delay(this.TimeLimitInSeconds * 1000);

                try
                {
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    var processTask = Task.Run(() => process.WaitForExitAsync(cts.Token));

                    if (await Task.WhenAny(processTask, killTask) == killTask)
                    {
                        process.Kill();
                        return (output.ToString(), "Error: process took a lot of time");
                    }

                    await processTask;
                }
                finally
                {
                    cts.Cancel();
                }

                return (output.ToString(), error.ToString());
            }
        }


        public (string output, string error) ExecutePythonCode(string code, string fullPythonPath)
        {
            if (LastExecutionTime.Add(Interval) > DateTime.Now)
            {
                throw new InvalidOperationException($"This method can only be called once every {Interval} minutes.");
            }

            LastExecutionTime = DateTime.Now;

            var output = new StringBuilder();
            var error = new StringBuilder();
             
            var startInfo = new ProcessStartInfo
            {
                FileName = fullPythonPath,
                Arguments = $"-c \"{code}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                process.OutputDataReceived += (sender, args) => output.AppendLine(args.Data);
                process.ErrorDataReceived += (sender, args) => error.AppendLine(args.Data);

                var cts = new System.Threading.CancellationTokenSource();
                var killTask = Task.Delay(this.TimeLimitInSeconds * 1000);

                try
                {
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    if (!process.WaitForExit(this.TimeLimitInSeconds * 1000))
                    {
                        process.Kill();
                        return (output.ToString(), "Error: process took a lot of time");
                    }
                }
                finally
                {
                    cts.Cancel();
                }

                return (output.ToString(), error.ToString());
            }
        }   

    }


}
