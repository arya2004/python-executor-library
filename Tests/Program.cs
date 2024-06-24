using System;
using System.Diagnostics;
using System.Threading.Tasks;
using PythonExecutorLibrary;

namespace PythonExecutorLibraryExample
{
    class Program
    {
        /// <summary>
        /// Demonstrates how to execute Python code asynchronously and synchronously using PythonExecutor.
        /// </summary>
        static async Task ExecCode()
        {
            string code = @"
import sys
print('Starting a task that will consume resources...')
try:
    while True:
        pass 
except KeyboardInterrupt:
    print('Code terminated by user')
except MemoryError:
    print('Code terminated due to memory limit reached')
except Exception as e:
    print(f'Exception occurred: {e}')
finally:
    print('Finished task')
";

            PythonExecutor pythonExecutor = new PythonExecutor(2);

            // Execute Python code asynchronously
            var (output, error) = await pythonExecutor.ExecutePythonCodeAsync("python", code);
            Console.WriteLine("Async Execution Output:");
            Console.WriteLine(output);
            Console.WriteLine("Async Execution Error:");
            Console.WriteLine(error);

            // Execute Python code synchronously
            (output, error) = pythonExecutor.ExecutePythonCode("python", code);
            Console.WriteLine("Sync Execution Output:");
            Console.WriteLine(output);
            Console.WriteLine("Sync Execution Error:");
            Console.WriteLine(error);
        }

        /// <summary>
        /// Demonstrates how to execute multiple Python code snippets asynchronously using PythonExecutor.
        /// </summary>
        static async Task ExecMultipleCode()
        {
            string code = @"
print('Hello')
";

            PythonExecutor pythonExecutor = new PythonExecutor(2);

            // Execute multiple Python code snippets asynchronously
            var (outputs, error) = await pythonExecutor.ExecuteMultiplePythonCodeAsync("python", new string[] { code });
            Console.WriteLine("Async Multiple Execution Output:");
            Console.WriteLine(outputs[0]);
            Console.WriteLine("Async Multiple Execution Error:");
            Console.WriteLine(error);
        }

        /// <summary>
        /// Demonstrates how to install a Python package using PythonPackageManager.
        /// </summary>
        static async Task InstallPackage()
        {
            string packageName = "numpy";
            bool success = await PythonPackageManager.InstallPackageAsync("python", packageName);

            if (success)
            {
                Console.WriteLine($"{packageName} installed successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to install {packageName}.");
            }
        }

        /// <summary>
        /// Demonstrates various package management functionalities using PythonPackageManager.
        /// </summary>
        static async Task PackageMics()
        {
            Console.WriteLine(await PythonPackageManager.GetPackageLocationAsync("python", "pandas"));
            Console.WriteLine(await PythonPackageManager.IsPackageHealthyAsync("python", "pandas"));
            Console.WriteLine(await PythonPackageManager.PackageExistsAsync("python", "pandas"));
        }

        /// <summary>
        /// Demonstrates how to install pip using PythonPackageManager.
        /// </summary>
        static async Task InstallPip()
        {
            Console.WriteLine("Before installing pip");
            Console.WriteLine(await PythonPackageManager.InstallPipAsync("python"));
            Console.WriteLine("After installing pip");
            Console.WriteLine(await PythonPackageManager.GetPackageLocationAsync("python", "pip"));
            Console.WriteLine(await PythonPackageManager.IsPackageHealthyAsync("python", "pip"));
            Console.WriteLine(await PythonPackageManager.PackageExistsAsync("python", "pip"));
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static async Task Main(string[] args)
        {
            // Call method to install a package
            await InstallPackage();

            // Call method to execute Python code asynchronously and synchronously
            await ExecCode();

            // Measure execution time for executing multiple Python code snippets
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await ExecMultipleCode();
            stopwatch.Stop();
            Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");

            // Validate Python code
            string pythonCode1 = "import os\nprint('Hello, World!')";
            string pythonCode2 = "fprintf('Hello, World!')";

            bool result1 = PythonCodeValidator.ContainsImportOrPrint(pythonCode1);
            bool result2 = PythonCodeValidator.ContainsImportOrPrint(pythonCode2);

            Console.WriteLine($"Code 1 contains 'import' or 'print': {result1}");
            Console.WriteLine($"Code 2 contains 'import' or 'print': {result2}");

            // Call method to demonstrate various package management functionalities
            await PackageMics();

            // Call method to install pip and verify its installation
            await InstallPip();
        }
    }
}
