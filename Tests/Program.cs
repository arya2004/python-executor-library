﻿using System;
using System.Threading.Tasks;
using PythonExecutorLibrary;

namespace Tests
{
    class Program
    {
        static async Task ExecCode()
        {
          

            string code = @"
import numpy as np

arr = np.array([1, 2, 3, 4, 5])

print(arr)

print(type(arr))

print(x)
    

";

            //called every time specified by ctor
            PythonExecutor pythonExecutor = new PythonExecutor(TimeSpan.FromSeconds(5));

            //Can be called as many times
            PythonExecutor pythonExecutor1 = new PythonExecutor();

            var (output, error) = await pythonExecutor1.ExecutePythonCodeAsync(code, "python");

            Console.WriteLine("Output:");
            Console.WriteLine(output);

            Console.WriteLine("Error:");
            Console.WriteLine(error);

            Thread.Sleep(TimeSpan.FromSeconds(6));

            (output, error) = await pythonExecutor1.ExecutePythonCodeAsync(code, "python");

            Console.WriteLine("Output:");
            Console.WriteLine(output);

            Console.WriteLine("Error:");
            Console.WriteLine(error);
        }


        static async Task InstallPackage()
        {
            //string packageName = "numpy"; 
            //string arguments = "--user"; 

            //bool success = await PythonPackageManager.InstallPackageAsync("python",packageName);

            //if (success)
            //{
            //    Console.WriteLine($"{packageName} installed successfully.");
            //}
            //else
            //{
            //    Console.WriteLine($"Failed to install {packageName}.");
            //}

            Console.WriteLine(await PythonPackageManager.GetPackageLocationAsync("python", "pandas"));
            Console.WriteLine(await PythonPackageManager.IsPackageHealthyAsync("python", "pandas"));
            Console.WriteLine(await PythonPackageManager.PackageExistsAsync("python", "pandas"));
        }

        static async Task Main(string[] args)
        {
            await InstallPackage();

          //  await ExecCode();
        }

        //https://chatgpt.com/share/9a1697a6-5975-4a76-ba25-47b49e52c1e1
        //https://chatgpt.com/share/e417debb-3950-417a-99bc-8ad645d60a74

        //pip install
        //https://bootstrap.pypa.io/get-pip.py


        //pip with python
        //process.StartInfo.FileName = fullPythonPath;
        //process.StartInfo.Arguments = $"-m pip install {packageName}";

    }
}


