using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonExecutorLibrary
{
    public static class ProcessExtensions
    {
        public static Task WaitForExitAsync(this Process process, int timeout = -1)
        {
            return Task.Run(() =>
            {
                if (timeout == -1)
                {
                    process.WaitForExit();
                }
                else
                {
                    process.WaitForExit(timeout);
                }
            });
        }
    }
}
