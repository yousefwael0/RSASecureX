using System;
using System.Diagnostics;
using System.IO;
using BigIntegerLib;
using RSACrypto;

namespace RSASecureX;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("RSA SecureX - Program Started.");

        // TODO: Read input file
        // TODO: Choose encryption or decryption
        // TODO: Measure execution time
        // TODO: Write result to output file
    }

    // Read input from file
    static void ReadInput(string inputPath)
    {
        throw new NotImplementedException();
    }

    // Write output to file
    static void WriteOutput(string outputPath, string result)
    {
        throw new NotImplementedException();
    }

    // Measure execution time helper
    static TimeSpan MeasureExecutionTime(Action operation)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        operation();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }
}
