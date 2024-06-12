using System.Diagnostics;
using System.Threading.Tasks;

namespace PythonExecutorLibrary
{
    public class PythonPackageManager
    {
        public static async Task<bool> InstallPackageAsync(string packageName, string args = "")
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "pip",
                Arguments = $"install {packageName} {args}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                process.OutputDataReceived += (sender, eventArgs) =>
                {
                    if (!string.IsNullOrWhiteSpace(eventArgs.Data))
                    {
                        // Handle pip output, if needed
                    }
                };

                process.ErrorDataReceived += (sender, eventArgs) =>
                {
                    if (!string.IsNullOrWhiteSpace(eventArgs.Data))
                    {
                        // Handle pip error output, if needed
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();

                return process.ExitCode == 0; // Exit code 0 indicates successful installation
            }
        }
    }

   
}
