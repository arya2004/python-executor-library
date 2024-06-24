using System;
using System.Text.RegularExpressions;

namespace PythonExecutorLibrary
{
    /// <summary>
    /// Provides methods to validate Python code.
    /// </summary>
    public class PythonCodeValidator
    {
        /// <summary>
        /// Checks if the given Python code contains 'import' or 'print' statements.
        /// </summary>
        /// <param name="pythonCode">The Python code to validate.</param>
        /// <returns>True if the code contains 'import' or 'print'; otherwise, false.</returns>
        public static bool ContainsImportOrPrint(string pythonCode)
        {
            if (string.IsNullOrEmpty(pythonCode))
            {
                return false;
            }

            // Define the regex pattern
            string pattern = @"\b(import|print)\b";

            // Use Regex to match the pattern
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // Check if the pattern is found in the pythonCode
            return regex.IsMatch(pythonCode);
        }
    }
}
