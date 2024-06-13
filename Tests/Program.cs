using System;
using System.Threading.Tasks;
using PythonExecutorLibrary;

namespace Tests
{
    class Program
    {
        static async Task ExecCode()
        {
          

            string code = @"

import sys


print(""Starting a task that will consume resources..."")

try:
    while True:
        pass 
except KeyboardInterrupt:
    print(""Code terminated by user"")
except MemoryError:
    print(""Code terminated due to memory limit reached"")
except Exception as e:
    print(f""Exception occurred: {e}"")
finally:
    print(""Finished task"")
    

";

            //called every time specified by ctor
            PythonExecutor pythonExecutor = new PythonExecutor(TimeSpan.FromSeconds(5), 1);

            //Can be called as many times
            PythonExecutor pythonExecutor1 = new PythonExecutor(1);

            

            var (output, error) = await pythonExecutor1.ExecutePythonCodeAsync(code, "python");

            Console.WriteLine("Output:");
            Console.WriteLine(output);

            Console.WriteLine("Error:");
            Console.WriteLine(error);

            //Thread.Sleep(TimeSpan.FromSeconds(6));

            //(output, error) = await pythonExecutor1.ExecutePythonCodeAsync(code, "python", TimeSpan.FromSeconds(1), 1 * 1024 * 1024, 50);

            //Console.WriteLine("Output:");
            //Console.WriteLine(output);

            //Console.WriteLine("Error:");
            //Console.WriteLine(error);
        }


        static async Task InstallPackage()
        {
            string packageName = "numpy";
            string arguments = "--user";

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

        static async Task PackageMics()
        {
            

            Console.WriteLine(await PythonPackageManager.GetPackageLocationAsync("python", "pandas"));
            Console.WriteLine(await PythonPackageManager.IsPackageHealthyAsync("python", "pandas"));
            Console.WriteLine(await PythonPackageManager.PackageExistsAsync("python", "pandas"));


        }

        static async Task InstallPip()
        {
            

            Console.WriteLine(await PythonPackageManager.GetPackageLocationAsync("python", "pip"));
            Console.WriteLine(await PythonPackageManager.IsPackageHealthyAsync("python", "pip"));
            Console.WriteLine(await PythonPackageManager.PackageExistsAsync("python", "pip"));

            Console.WriteLine("Before installing pip");

            Console.WriteLine(await PythonPackageManager.InstallPipAsync("python"));

            Console.WriteLine("After installing pip");

            Console.WriteLine(await PythonPackageManager.GetPackageLocationAsync("python", "pip"));
            Console.WriteLine(await PythonPackageManager.IsPackageHealthyAsync("python", "pip"));
            Console.WriteLine(await PythonPackageManager.PackageExistsAsync("python", "pip"));
        }


        static async Task Main(string[] args)
        {
            //await InstallPackage();

            await ExecCode();
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


