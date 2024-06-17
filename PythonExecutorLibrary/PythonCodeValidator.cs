using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PythonExecutorLibrary
{
    public class PythonCodeValidator
    {
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
