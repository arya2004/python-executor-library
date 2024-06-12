using System;
using System.Threading.Tasks;
using PythonExecutorLibrary;

namespace Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            PythonExecutor executor = new PythonExecutor();

            string code = @"
x = 2
y = 8
print(x * y)

";

            var result = await executor.ExecutePythonCodeAsync(code);

            Console.WriteLine("Output:");
            Console.WriteLine(result.output);

            //Console.WriteLine("Error:");
            //Console.WriteLine(result.error);
        }
    }
}


