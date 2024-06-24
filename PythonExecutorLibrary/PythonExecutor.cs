
using System.Diagnostics;
using System.Text;

namespace PythonExecutorLibrary
{
    /// <summary>
    /// Provides methods to execute Python code.
    /// </summary>
    public class PythonExecutor
    {
        /// <summary>
        /// Gets or sets the interval between executions.
        /// </summary>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// Gets or sets the last execution time.
        /// </summary>
        public DateTime LastExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the time limit for executing Python code in seconds.
        /// </summary>
        public int TimeLimitInSeconds { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PythonExecutor"/> class with a specified interval and time limit.
        /// </summary>
        /// <param name="_dateTimeOffset">The interval between executions.</param>
        /// <param name="processTimeLimitInSeconds">The time limit for executing Python code in seconds.</param>
        public PythonExecutor(TimeSpan _dateTimeOffset, int processTimeLimitInSeconds)
        {
            LastExecutionTime = DateTime.MinValue;
            Interval = _dateTimeOffset;
            TimeLimitInSeconds = processTimeLimitInSeconds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PythonExecutor"/> class with a specified time limit.
        /// </summary>
        /// <param name="processTimeLimitInSeconds">The time limit for executing Python code in seconds.</param>
        public PythonExecutor(int processTimeLimitInSeconds)
        {
            LastExecutionTime = DateTime.MinValue;
            Interval = TimeSpan.Zero;
            TimeLimitInSeconds = processTimeLimitInSeconds;
        }

        /// <summary>
        /// Executes Python code asynchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <param name="code">The Python code to execute.</param>
        /// <returns>A tuple containing the standard output and standard error from the Python process.</returns>
        public async Task<(string output, string error)> ExecutePythonCodeAsync(string fullPythonPath, string code)
        {
            if (LastExecutionTime.Add(Interval) > DateTime.Now)
            {
                return (string.Empty, $"This method can only be called once every {Interval} minutes.");
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
                        return (output.ToString(), "Error: process took too much time");
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

        /// <summary>
        /// Executes multiple Python code snippets asynchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <param name="args">An array of Python code snippets to execute.</param>
        /// <returns>A tuple containing an array of standard outputs and the standard error from the Python process.</returns>
        public async Task<(string[] output, string error)> ExecuteMultiplePythonCodeAsync(string fullPythonPath, string[] args)
        {
            if (LastExecutionTime.Add(Interval) > DateTime.Now)
            {
                return (Array.Empty<string>(), $"This method can only be called once every {Interval} minutes.");
            }

            LastExecutionTime = DateTime.Now;

            var output = new StringBuilder();
            var error = new StringBuilder();

            var finalCode = new StringBuilder();
            string guid = Guid.NewGuid().ToString();

            finalCode.Append(args[0]);
            for (int i = 1; i < args.Length; i++)
            {
                finalCode.Append($"\nprint('{guid}')\n" + args[i]);
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = fullPythonPath,
                Arguments = $"-c \"{finalCode}\"",
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
                        return (Array.Empty<string>(), "Error: process took too much time");
                    }

                    await processTask;
                }
                finally
                {
                    cts.Cancel();
                }

                string[] splitAnswer = output.ToString().Split($"{guid}\r\n");

                return (splitAnswer, error.ToString());
            }
        }

        /// <summary>
        /// Executes Python code synchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <param name="code">The Python code to execute.</param>
        /// <returns>A tuple containing the standard output and standard error from the Python process.</returns>
        public (string output, string error) ExecutePythonCode(string fullPythonPath, string code)
        {
            if (LastExecutionTime.Add(Interval) > DateTime.Now)
            {
                return (string.Empty, $"This method can only be called once every {Interval} minutes.");
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

                    if (!process.WaitForExit(this.TimeLimitInSeconds * 1000))
                    {
                        process.Kill();
                        return (output.ToString(), "Error: process took too much time");
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
