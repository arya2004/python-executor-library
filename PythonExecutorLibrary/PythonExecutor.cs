using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace PythonExecutorLibrary
{
    public class PythonExecutor
    {
        public static async Task<(string output, string error)> ExecutePythonCodeAsync(string code)
        {
            var output = new StringBuilder();
            var error = new StringBuilder();

            var startInfo = new ProcessStartInfo
            {
                FileName = "python",
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
