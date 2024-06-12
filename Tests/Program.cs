using System;
using System.Threading.Tasks;
using PythonExecutorLibrary;

namespace Tests
{
    class Program
    {
        static async Task ExecCode()
        {
           // PythonExecutor executor = new PythonExecutor();

            string code = @"
import numpy as np

arr = np.array([1, 2, 3, 4, 5])

print(arr)

print(type(arr))
    

";

            var (output, error) = await PythonExecutor.ExecutePythonCodeAsync(code);

            Console.WriteLine("Output:");
            Console.WriteLine(output);

            Console.WriteLine("Error:");
            Console.WriteLine(error);
        }


        static async Task InstallPackage()
        {
            string packageName = "pandas"; 
            string arguments = "--user"; 

            bool success = await PythonPackageManager.InstallPackageAsync(packageName);

            if (success)
            {
                Console.WriteLine($"{packageName} installed successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to install {packageName}.");
            }
        }

        static async Task Main(string[] args)
        {
            await InstallPackage();

            await ExecCode();
        }
    }
}


