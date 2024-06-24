using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PythonExecutorLibrary
{
    /// <summary>
    /// Provides extension methods for the <see cref="Process"/> class.
    /// </summary>
    public static class ProcessExtensions
    {
        /// <summary>
        /// Asynchronously waits for the associated process to exit.
        /// </summary>
        /// <param name="process">The process to wait for.</param>
        /// <param name="timeout">The amount of time, in milliseconds, to wait for the process to exit. 
        /// The default is -1, which means an infinite time-out period.</param>
        /// <returns>A task that represents the asynchronous wait operation.</returns>
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
