using System;
using System.Threading.Tasks;
using PythonExecutorLibrary;

namespace Tests
{
    class Program
    {
        static async Task Function(string[] args)
        {
            PythonExecutor executor = new PythonExecutor();

            string code = @"
import numpy as np

arr = np.array([1, 2, 3, 4, 5])

print(arr)

print(type(arr))
    

";

            var (output, error) = await executor.ExecutePythonCodeAsync(code);

            Console.WriteLine("Output:");
            Console.WriteLine(output);

            Console.WriteLine("Error:");
            Console.WriteLine(error);
        }


        static async Task Main(string[] args)
        {
            PythonPackageManager packageManager = new PythonPackageManager();
            string packageName = "pandas"; // Example package name
            string arg = "--user"; // Example arguments for pip

            bool success = await packageManager.InstallPackageAsync(packageName);

            if (success)
            {
                Console.WriteLine($"{packageName} installed successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to install {packageName}.");
            }
        }
    }
}


