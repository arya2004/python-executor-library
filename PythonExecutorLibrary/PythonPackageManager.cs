using System.Diagnostics;
using System.Threading.Tasks;

namespace PythonExecutorLibrary
{
    public class PythonPackageManager
    {
        public static async Task<bool> InstallPackageAsync(string fullPythonPath, string packageName, string args = "")
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fullPythonPath,
                Arguments = $" -m pip install {packageName} {args}",
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
                        //Console.WriteLine(eventArgs.Data);
                    }
                };

                process.ErrorDataReceived += (sender, eventArgs) =>
                {
                    if (!string.IsNullOrWhiteSpace(eventArgs.Data))
                    {
                        // Handle pip error output, if needed
                       // Console.WriteLine(eventArgs.Data);

                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();

                return process.ExitCode == 0; // Exit code 0 indicates successful installation
            }
        }

        public static async Task<bool> PackageExistsAsync(string fullPythonPath, string packageName)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fullPythonPath,
                Arguments = $" -m pip show {packageName}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                string output = string.Empty;

                process.OutputDataReceived += (sender, eventArgs) =>
                {
                    if (!string.IsNullOrWhiteSpace(eventArgs.Data))
                    {
                        output += eventArgs.Data;
                    }
                };

                process.Start();
                process.BeginOutputReadLine();

                await process.WaitForExitAsync();

                return !string.IsNullOrEmpty(output); // Non-empty output indicates the package exists
            }
        }

        public static async Task<bool> IsPackageHealthyAsync(string fullPythonPath, string packageName)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fullPythonPath,
                Arguments = $" -c \"import {packageName}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();
                await process.WaitForExitAsync();

                return process.ExitCode == 0; // Exit code 0 indicates the package can be imported without error
            }
        }

        public static async Task<string> GetPackageLocationAsync(string fullPythonPath, string packageName)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fullPythonPath,
                Arguments = $" -m pip show {packageName}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                string output = string.Empty;

                process.OutputDataReceived += (sender, eventArgs) =>
                {
                    if (!string.IsNullOrWhiteSpace(eventArgs.Data))
                    {
                        output += eventArgs.Data + Environment.NewLine;
                    }
                };

                process.Start();
                process.BeginOutputReadLine();

                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    return null; // Exit code other than 0 indicates an error
                }

                // Parse the output to find the location
                var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (var line in lines)
                {
                    if (line.StartsWith("Location:"))
                    {
                        return line.Substring("Location:".Length).Trim();
                    }
                }

                return null; // Location not found
            }
        }
    }

   
}
