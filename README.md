# Python Executor Library

## Overview

The `PythonExecutorLibrary` is a .NET library that provides functionalities to execute Python scripts, manage Python virtual environments, handle processes, and manage Python packages. This document provides definitions and usage examples for the classes and methods in the library.

___


## Classes and Methods

### PythonExecutor

#### `RunPythonScript`

```csharp
public static string RunPythonScript(string pythonExecutablePath, string scriptPath, string arguments)
```

Executes a Python script with the specified arguments.

- **Parameters**:
  - `pythonExecutablePath` (string): The path to the Python executable.
  - `scriptPath` (string): The path to the Python script.
  - `arguments` (string): The arguments to pass to the script.

- **Returns**: The output from the script execution.

___

### PythonPackageManager

#### `InstallPackageAsync`

```csharp
public static async Task<bool> InstallPackageAsync(string fullPythonPath, string packageName, string args = "")
```

Installs a Python package asynchronously.

- **Parameters**:
  - `fullPythonPath` (string): The full path to the Python executable.
  - `packageName` (string): The name of the package to install.
  - `args` (string): Additional arguments for the pip install command.

- **Returns**: A task that represents the asynchronous operation. The task result contains a boolean indicating whether the installation was successful.

#### `PackageExistsAsync`

```csharp
public static async Task<bool> PackageExistsAsync(string fullPythonPath, string packageName)
```

Checks if a Python package is installed asynchronously.

- **Parameters**:
  - `fullPythonPath` (string): The full path to the Python executable.
  - `packageName` (string): The name of the package to check.

- **Returns**: A task that represents the asynchronous operation. The task result contains a boolean indicating whether the package is installed.

#### `IsPackageHealthyAsync`

```csharp
public static async Task<bool> IsPackageHealthyAsync(string fullPythonPath, string packageName)
```

Checks if a Python package is healthy (i.e., can be imported without error) asynchronously.

- **Parameters**:
  - `fullPythonPath` (string): The full path to the Python executable.
  - `packageName` (string): The name of the package to check.

- **Returns**: A task that represents the asynchronous operation. The task result contains a boolean indicating whether the package is healthy.

#### `GetPackageLocationAsync`

```csharp
public static async Task<string> GetPackageLocationAsync(string fullPythonPath, string packageName)
```

Gets the location of a Python package asynchronously.

- **Parameters**:
  - `fullPythonPath` (string): The full path to the Python executable.
  - `packageName` (string): The name of the package to locate.

- **Returns**: A task that represents the asynchronous operation. The task result contains the location of the package if found, otherwise null.

#### `InstallPipAsync`

```csharp
public static async Task<bool> InstallPipAsync(string fullPythonPath)
```

Installs pip asynchronously.

- **Parameters**:
  - `fullPythonPath` (string): The full path to the Python executable.

- **Returns**: A task that represents the asynchronous operation. The task result contains a boolean indicating whether the installation was successful.

#### `InstallPackage`

```csharp
public static bool InstallPackage(string fullPythonPath, string packageName, string args = "")
```

Installs a Python package synchronously.

- **Parameters**:
  - `fullPythonPath` (string): The full path to the Python executable.
  - `packageName` (string): The name of the package to install.
  - `args` (string): Additional arguments for the pip install command.

- **Returns**: A boolean indicating whether the installation was successful.

#### `PackageExists`

```csharp
public static bool PackageExists(string fullPythonPath, string packageName)
```

Checks if a Python package is installed synchronously.

- **Parameters**:
  - `fullPythonPath` (string): The full path to the Python executable.
  - `packageName` (string): The name of the package to check.

- **Returns**: A boolean indicating whether the package is installed.

#### `IsPackageHealthy`

```csharp
public static bool IsPackageHealthy(string fullPythonPath, string packageName)
```

Checks if a Python package is healthy (i.e., can be imported without error) synchronously.

- **Parameters**:
  - `fullPythonPath` (string): The full path to the Python executable.
  - `packageName` (string): The name of the package to check.

- **Returns**: A boolean indicating whether the package is healthy.

#### `GetPackageLocation`

```csharp
public static string GetPackageLocation(string fullPythonPath, string packageName)
```

Gets the location of a Python package synchronously.

- **Parameters**:
  - `fullPythonPath` (string): The full path to the Python executable.
  - `packageName` (string): The name of the package to locate.

- **Returns**: The location of the package if found, otherwise null.

#### `InstallPip`

```csharp
public static bool InstallPip(string fullPythonPath)
```

Installs pip synchronously.

- **Parameters**:
  - `fullPythonPath` (string): The full path to the Python executable.

- **Returns**: A boolean indicating whether the installation was successful.

### Usage Examples

#### Running a Python Script

```csharp
string pythonPath = @"C:\Python39\python.exe";
string scriptPath = @"C:\Scripts\my_script.py";
string arguments = "arg1 arg2";

string output = PythonExecutor.RunPythonScript(pythonPath, scriptPath, arguments);
Console.WriteLine(output);
```



#### Installing a Python Package Asynchronously

```csharp
string pythonPath = @"C:\Python39\python.exe";
string packageName = "requests";

bool isInstalled = await PythonPackageManager.InstallPackageAsync(pythonPath, packageName);
Console.WriteLine("Package Installed: " + isInstalled);
```

#### Checking if a Package is Installed

```csharp
string pythonPath = @"C:\Python39\python.exe";
string packageName = "requests";

bool exists = PythonPackageManager.PackageExists(pythonPath, packageName);
Console.WriteLine("Package Exists: " + exists);
```

#### Checking Package Health

```csharp
string pythonPath = @"C:\Python39\python.exe";
string packageName = "requests";

bool isHealthy = PythonPackageManager.IsPackageHealthy(pythonPath, packageName);
Console.WriteLine("Package Healthy: " + isHealthy);
```

#### Installing pip Asynchronously

```csharp
string pythonPath = @"C:\Python39\python.exe";

bool isPipInstalled

 = await PythonPackageManager.InstallPipAsync(pythonPath);
Console.WriteLine("pip Installed: " + isPipInstalled);
```


___

### PythonCodeValidator

The `PythonCodeValidator` class contains methods to validate Python code. This class is static and does not require instantiation.

#### Methods

##### ContainsImportOrPrint

```csharp
public static bool ContainsImportOrPrint(string pythonCode)
```

**Description:**

The `ContainsImportOrPrint` method checks if the given Python code contains 'import' or 'print' statements. It uses regular expressions to search for these keywords in a case-insensitive manner.

**Parameters:**

- `pythonCode` (`string`): The Python code to validate. It can be a single line or multiple lines of code.

**Returns:**

- `bool`: Returns `true` if the code contains 'import' or 'print' statements; otherwise, returns `false`.

**Usage:**

```csharp
using System;
using PythonExecutorLibrary;

class Program
{
    static void Main()
    {
        string code1 = "import os\nprint('Hello, world!')";
        string code2 = "def add(a, b):\n    return a + b";

        bool result1 = PythonCodeValidator.ContainsImportOrPrint(code1); // Returns true
        bool result2 = PythonCodeValidator.ContainsImportOrPrint(code2); // Returns false

        Console.WriteLine($"Code 1 contains 'import' or 'print': {result1}");
        Console.WriteLine($"Code 2 contains 'import' or 'print': {result2}");
    }
}
```

___

### PythonExecutor

The `PythonExecutor` class provides methods to execute Python code. This class supports both synchronous and asynchronous execution.

#### Properties

##### Interval

```csharp
public TimeSpan Interval { get; set; }
```

**Description:**

Gets or sets the interval between executions.

##### LastExecutionTime

```csharp
public DateTime LastExecutionTime { get; set; }
```

**Description:**

Gets or sets the last execution time.

##### TimeLimitInSeconds

```csharp
public int TimeLimitInSeconds { get; set; }
```

**Description:**

Gets or sets the time limit for executing Python code in seconds.

#### Constructors

##### PythonExecutor(TimeSpan _dateTimeOffset, int processTimeLimitInSeconds)

```csharp
public PythonExecutor(TimeSpan _dateTimeOffset, int processTimeLimitInSeconds)
```

**Description:**

Initializes a new instance of the `PythonExecutor` class with a specified interval and time limit.

**Parameters:**

- `_dateTimeOffset` (`TimeSpan`): The interval between executions.
- `processTimeLimitInSeconds` (`int`): The time limit for executing Python code in seconds.

##### PythonExecutor(int processTimeLimitInSeconds)

```csharp
public PythonExecutor(int processTimeLimitInSeconds)
```

**Description:**

Initializes a new instance of the `PythonExecutor` class with a specified time limit.

**Parameters:**

- `processTimeLimitInSeconds` (`int`): The time limit for executing Python code in seconds.

#### Methods

##### ExecutePythonCodeAsync

```csharp
public async Task<(string output, string error)> ExecutePythonCodeAsync(string fullPythonPath, string code)
```

**Description:**

Executes Python code asynchronously.

**Parameters:**

- `fullPythonPath` (`string`): The full path to the Python executable.
- `code` (`string`): The Python code to execute.

**Returns:**

- `Task<(string output, string error)>`: A tuple containing the standard output and standard error from the Python process.

**Usage:**

```csharp
using System;
using System.Threading.Tasks;
using PythonExecutorLibrary;

class Program
{
    static async Task Main()
    {
        var executor = new PythonExecutor(5);
        var result = await executor.ExecutePythonCodeAsync("/usr/bin/python3", "print('Hello, world!')");
        Console.WriteLine($"Output: {result.output}");
        Console.WriteLine($"Error: {result.error}");
    }
}
```

##### ExecuteMultiplePythonCodeAsync

```csharp
public async Task<(string[] output, string error)> ExecuteMultiplePythonCodeAsync(string fullPythonPath, string[] args)
```

**Description:**

Executes multiple Python code snippets asynchronously.

**Parameters:**

- `fullPythonPath` (`string`): The full path to the Python executable.
- `args` (`string[]`): An array of Python code snippets to execute.

**Returns:**

- `Task<(string[] output, string error)>`: A tuple containing an array of standard outputs and the standard error from the Python process.

**Usage:**

```csharp
using System;
using System.Threading.Tasks;
using PythonExecutorLibrary;

class Program
{
    static async Task Main()
    {
        var executor = new PythonExecutor(5);
        string[] scripts = { "print('Script 1')", "print('Script 2')" };
        var result = await executor.ExecuteMultiplePythonCodeAsync("/usr/bin/python3", scripts);
        foreach (var output in result.output)
        {
            Console.WriteLine($"Output: {output}");
        }
        Console.WriteLine($"Error: {result.error}");
    }
}
```

##### ExecutePythonCode

```csharp
public (string output, string error) ExecutePythonCode(string fullPythonPath, string code)
```

**Description:**

Executes Python code synchronously.

**Parameters:**

- `fullPythonPath` (`string`): The full path to the Python executable.
- `code` (`string`): The Python code to execute.

**Returns:**

- `(string output, string error)`: A tuple containing the standard output and standard error from the Python process.

**Usage:**

```csharp
using System;
using PythonExecutorLibrary;

class Program
{
    static void Main()
    {
        var executor = new PythonExecutor(5);
        var result = executor.ExecutePythonCode("/usr/bin/python3", "print('Hello, world!')");
        Console.WriteLine($"Output: {result.output}");
        Console.WriteLine($"Error: {result.error}");
    }
}
```

## Example

Here is a complete example demonstrating how to use both the `PythonCodeValidator` and `PythonExecutor` classes:

```csharp
using System;
using System.Threading.Tasks;
using PythonExecutorLibrary;

class Program
{
    static async Task Main()
    {
        // Validate Python code
        string code1 = "import os\nprint('Hello, world!')";
        string code2 = "def add(a, b):\n    return a + b";

        bool result1 = PythonCodeValidator.ContainsImportOrPrint(code1); // Returns true
        bool result2 = PythonCodeValidator.ContainsImportOrPrint(code2); // Returns false

        Console.WriteLine($"Code 1 contains 'import' or 'print': {result1}");
        Console.WriteLine($"Code 2 contains 'import' or 'print': {result2}");

        // Execute Python code asynchronously
        var executor = new PythonExecutor(5);
        var asyncResult = await executor.ExecutePythonCodeAsync("/usr/bin/python3", "print('Hello, async world!')");
        Console.WriteLine($"Async Output: {asyncResult.output}");
        Console.WriteLine($"Async Error: {asyncResult.error}");

        // Execute multiple Python code snippets asynchronously
        string[] scripts = { "print('Script 1')", "print('Script 2')" };
        var multiResult = await executor.ExecuteMultiplePythonCodeAsync("/usr/bin/python3", scripts);
        foreach (var output in multiResult.output)
        {
            Console.WriteLine($"Multi Output: {output}");
        }
        Console.WriteLine($"Multi Error: {multiResult.error}");

        // Execute Python code synchronously
        var syncResult = executor.ExecutePythonCode("/usr/bin/python3", "print('Hello, sync world!')");
        Console.WriteLine($"Sync Output: {syncResult.output}");
        Console.WriteLine($"Sync Error: {syncResult.error}");
    }
}
```
