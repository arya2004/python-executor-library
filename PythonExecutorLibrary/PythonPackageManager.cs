using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PythonExecutorLibrary
{
    /// <summary>
    /// Provides methods to manage Python packages using pip.
    /// </summary>
    public class PythonPackageManager
    {
        /// <summary>
        /// Installs a Python package asynchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <param name="packageName">The name of the package to install.</param>
        /// <param name="args">Additional arguments for the pip install command.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the installation was successful.</returns>
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

        /// <summary>
        /// Checks if a Python package is installed asynchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <param name="packageName">The name of the package to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the package is installed.</returns>
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

        /// <summary>
        /// Checks if a Python package is healthy (i.e., can be imported without error) asynchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <param name="packageName">The name of the package to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the package is healthy.</returns>
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

                return process.ExitCode == 0;
            }
        }

        /// <summary>
        /// Gets the location of a Python package asynchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <param name="packageName">The name of the package to locate.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the location of the package if found, otherwise null.</returns>
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

        /// <summary>
        /// Installs pip asynchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the installation was successful.</returns>
        public static async Task<bool> InstallPipAsync(string fullPythonPath)
        {
            const string getPipUrl = "https://bootstrap.pypa.io/get-pip.py";
            const string tempFilePath = "get-pip.py";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Download get-pip.py script
                    var response = await httpClient.GetAsync(getPipUrl);
                    response.EnsureSuccessStatusCode();
                    var scriptContent = await response.Content.ReadAsStringAsync();

                    // Save the script to a temporary file
                    await File.WriteAllTextAsync(tempFilePath, scriptContent);

                    var startInfo = new ProcessStartInfo
                    {
                        FileName = fullPythonPath,
                        Arguments = tempFilePath,
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
                                // Handle script output, if needed
                                // Console.WriteLine(eventArgs.Data);
                            }
                        };

                        process.ErrorDataReceived += (sender, eventArgs) =>
                        {
                            if (!string.IsNullOrWhiteSpace(eventArgs.Data))
                            {
                                // Handle script error output, if needed
                                // Console.WriteLine(eventArgs.Data);
                            }
                        };

                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        await process.WaitForExitAsync();

                        return process.ExitCode == 0; // Exit code 0 indicates successful pip installation
                    }
                }
                finally
                {
                    // Clean up the temporary file
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }
                }
            }
        }

        /// <summary>
        /// Installs a Python package synchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <param name="packageName">The name of the package to install.</param>
        /// <param name="args">Additional arguments for the pip install command.</param>
        /// <returns>A boolean indicating whether the installation was successful.</returns>
        public static bool InstallPackage(string fullPythonPath, string packageName, string args = "")
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

                process.WaitForExit();

                return process.ExitCode == 0; // Exit code 0 indicates successful installation
            }
        }

        /// <summary>
        /// Checks if a Python package is installed synchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <param name="packageName">The name of the package to check.</param>
        /// <returns>A boolean indicating whether the package is installed.</returns>
        public static bool PackageExists(string fullPythonPath, string packageName)
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

                process.WaitForExit();

                return !string.IsNullOrEmpty(output); // Non-empty output indicates the package exists
            }
        }

        /// <summary>
        /// Checks if a Python package is healthy (i.e., can be imported without error) synchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <param name="packageName">The name of the package to check.</param>
        /// <returns>A boolean indicating whether the package is healthy.</returns>
        public static bool IsPackageHealthy(string fullPythonPath, string packageName)
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
                process.WaitForExit();

                return process.ExitCode == 0; // Exit code 0 indicates the package can be imported without error
            }
        }

        /// <summary>
        /// Gets the location of a Python package synchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <param name="packageName">The name of the package to locate.</param>
        /// <returns>The location of the package if found, otherwise null.</returns>
        public static string GetPackageLocation(string fullPythonPath, string packageName)
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

                process.WaitForExit();

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

        /// <summary>
        /// Installs pip synchronously.
        /// </summary>
        /// <param name="fullPythonPath">The full path to the Python executable.</param>
        /// <returns>A boolean indicating whether the installation was successful.</returns>
        public static bool InstallPip(string fullPythonPath)
        {
            const string getPipUrl = "https://bootstrap.pypa.io/get-pip.py";
            const string tempFilePath = "get-pip.py";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Download get-pip.py script
                    var response = httpClient.GetAsync(getPipUrl).Result;
                    response.EnsureSuccessStatusCode();
                    var scriptContent = response.Content.ReadAsStringAsync().Result;

                    // Save the script to a temporary file
                    File.WriteAllText(tempFilePath, scriptContent);

                    var startInfo = new ProcessStartInfo
                    {
                        FileName = fullPythonPath,
                        Arguments = tempFilePath,
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
                                // Handle script output, if needed
                                // Console.WriteLine(eventArgs.Data);
                            }
                        };

                        process.ErrorDataReceived += (sender, eventArgs) =>
                        {
                            if (!string.IsNullOrWhiteSpace(eventArgs.Data))
                            {
                                // Handle script error output, if needed
                                // Console.WriteLine(eventArgs.Data);
                            }
                        };

                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        process.WaitForExit();

                        return process.ExitCode == 0; // Exit code 0 indicates successful pip installation
                    }
                }
                finally
                {
                    // Clean up the temporary file
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }
                }
            }
        }
    }
}
