# LINQ Performance benchmarks

Is it really so slow to use LINQ for video games?
And what if we compile our unity code with IL2CPP?

Using [Unity's performance benchmark](https://docs.unity3d.com/Packages/com.unity.test-framework.performance@1.1) package, here are some tests to run.

## Results

LINQ is still remarkably slower than writing the code yourself.
For critical performance code. Don't use LINQ.

| Function                             | Deviation | Standard deviation | Median |   GC |
| :----------------------------------- | --------: | -----------------: | -----: | ---: |
| Where - Divide by 3 No LINQ          |      0.25 |               0.03 |   0.12 |   12 |
| Where - Divide by 3 LINQ Query       |      0.18 |               0.02 |   0.11 |   13 |
| Where - Divide by 3 LINQ             |      0.08 |               0.01 |   0.11 |   13 |
| Sum - No LINQ                        |      0.07 |               0.01 |   0.11 |    0 |
| Sum - LINQ                           |      0.30 |               0.06 |   0.19 |    1 |
| Select - Subtype No LINQ             |      0.25 |               0.05 |   0.19 |    2 |
| Select - Subtype LINQ Query          |      0.05 |               0.01 |   0.23 |    3 |
| Select - Subtype LINQ                |      0.16 |               0.04 |   0.23 |    3 |
| Select - Add 1 No LINQ               |      0.18 |               0.03 |   0.16 |    0 |
| Select - Add 1 LINQ Query            |      0.06 |               0.01 |   0.22 |    3 |
| Select - Add 1 LINQ                  |      0.07 |               0.01 |   0.21 |    3 |
| OfType - Complex No LINQ             |      0.07 |               0.03 |   0.37 |    0 |
| OfType - Complex No LINQ - Generic   |      0.15 |               0.07 |   0.51 |    0 |
| OfType - Complex LINQ                |       007 |               0.21 |   3.08 |    0 |
| OfType - Simple No LINQ              |      0.14 |               0.04 |   0.26 |   12 |
| OfType - Simple No LINQ Query        |      0.08 |               0.03 |   0.33 |   12 |
| OfType - Simple LINQ                 |      0.13 |               0.18 |   1.35 |   14 |
| Distinct - No LINQ Naive Prealloc    |      0.02 |              17.96 | 830.90 |    2 |
| Distinct - No LINQ Naive No Prealloc |      0.02 |              20.24 | 830.24 |   14 |
| Distinct - No LINQ HashSet           |      0.34 |               0.47 |   1.39 |    6 |
| Distinct - LINQ                      |      0.33 |               0.29 |   0.88 |   29 |

## Contributing

### Getting started

Open Unity, Open Window > General > Test Runner and Window > Analysis > Performance Test Report

In the test runner window, run all tests
In the performance test report window, press refresh and investigate.

### Adding your own tests

Simply create a new script and run it with the test runner.

### Using the Performance Benchmark Reporter

Download the tool here https://github.com/Unity-Technologies/PerformanceBenchmarkReporter

Syntax:

```
dotnet .\UnityPerformanceBenchmarkReporter.dll --results=C:\Projects\IL2CPP-LINQ\Builds\results\results.xml  --reportdirpath=C:\Projects\IL2CPP-LINQ\report
```

Note that the results.xml is not generated when running tests from the editor.
To generate the results.xml file, run the tests from unity command line as follows:

```
"C:\Program Files\Unity\Hub\Editor\2019.1.14f1\Editor\Unity.exe" -runTests -batchMode -projectPath C:\Projects\IL2CPP-LINQ\ -scriptingbackend=il2cpp -testPlatform playmode -buildTarget Win -testResults C:\Projects\IL2CPP-LINQ\Builds\results\results.xml -logFile C:\Projects\IL2CPP-LINQ\Log.txt
```

