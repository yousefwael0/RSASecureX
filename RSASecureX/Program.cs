using System;
using System.Diagnostics;
using System.IO;
using BigIntegerLib;
using RSACrypto;

namespace RSASecureX;

public class TestCase
{
    public string N { get; set; }
    public string Exponent { get; set; }  // e or d
    public string Message { get; set; }   // M or E(M)
    public bool IsDecrypt { get; set; }   // True If Decrypting
}

class Program
{
    static void Main(string[] args)
    {
        int systemTimeStart = System.Environment.TickCount;
        Console.WriteLine("RSA SecureX - Program Started.");

        BigInteger a = new BigInteger("10000000000");
        BigInteger b = new BigInteger("50000");

        Console.WriteLine("Multiplication: " + a * b);

        RSA rsa = new RSA();

        // TODO: Read input file
        List<TestCase> testCases = new List<TestCase>();
        testCases = ReadInput("Test.txt");

        System.Console.WriteLine(testCases.Count);
        // TODO: Choose encryption or decryption
        // TODO: Measure execution time
        // TODO: Write result to output file
        List<string> outputs = new List<string>();

        foreach (var testCase in testCases)
        {
            BigInteger n = new BigInteger(testCase.N);
            BigInteger exp = new BigInteger(testCase.Exponent);
            BigInteger message = new BigInteger(testCase.Message);

            BigInteger result;

            if (testCase.IsDecrypt)
            {
                result = rsa.Decrypt(message, exp, n);
            }
            else
            {
                result = rsa.Encrypt(message, exp, n);
                Console.WriteLine("RES: " + result);
            }

            outputs.Add(result.ToString());
        }

        WriteOutput("Output.txt", outputs);

        int systemTimeEnd = System.Environment.TickCount;
        Console.WriteLine("Execution Time: " + (systemTimeEnd - systemTimeStart) + "ms");
    }

    // Read input from file
    public static List<TestCase> ReadInput(string inputPath)
    {
        List<TestCase> testCases = new List<TestCase>();

        string[] lines = File.ReadAllLines(inputPath);
        int index = 1;
        int totalCases = int.Parse(lines[0]);

        for (int i = 0; i < totalCases; i++)
        {
            string n = lines[index++];
            string exponent = lines[index++];
            string message = lines[index++];
            int mode = int.Parse(lines[index++]);

            testCases.Add(new TestCase
            {
                N = n,
                Exponent = exponent,
                Message = message,
                IsDecrypt = (mode == 1)
            });
        }

        return testCases;
    }


    // Write output to file
    static void WriteOutput(string outputPath, List<string> results)
    {
        File.WriteAllLines(outputPath, results);
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
