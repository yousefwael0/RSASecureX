using System;
using System.Diagnostics;
using System.IO;
using BigIntegerLib;
using RSACrypto;
using RSAStringCrypto;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        Console.WriteLine("===== Sample Cases =====");

        RSA rsa = new RSA();

        List<TestCase> sampleCases = new List<TestCase>();
        sampleCases = ReadInput("Sample.txt");

        List<string> sampleOutputs = new List<string>();

        foreach (var testCase in sampleCases)
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
            }

            sampleOutputs.Add(result.ToString());
        }

        WriteOutput("SampleOutput.txt", sampleOutputs);

        int systemTimeEnd = System.Environment.TickCount;
        Console.WriteLine("Execution Time: " + (systemTimeEnd - systemTimeStart) + "ms");

        Console.WriteLine("===== Complete Test Cases =====");
        List<TestCase> completeCases = new List<TestCase>();
        completeCases = ReadInput("Complete.txt");

        List<string> completeOutputs = new List<string>();

        int completeCasesStart = System.Environment.TickCount;

        int i = 0;

        foreach (var testCase in completeCases)
        {
            systemTimeStart = System.Environment.TickCount;

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
            }

            completeOutputs.Add(result.ToString());
            systemTimeEnd = System.Environment.TickCount;
            Console.WriteLine("Case " + i + ": " + (systemTimeEnd - systemTimeStart) + "ms");
            i++;
        }

        WriteOutput("CompleteOutput.txt", completeOutputs);
        int completeCasesEnd = System.Environment.TickCount;
        Console.WriteLine("Complete Test Cases Total: " + (completeCasesEnd - completeCasesStart) + "ms");
    }

    public static List<TestCase> ReadInput(string inputPath)
    {
        List<TestCase> sampleCases = new List<TestCase>();

        string[] lines = File.ReadAllLines(inputPath);
        int index = 1;
        int totalCases = int.Parse(lines[0]);

        for (int i = 0; i < totalCases; i++)
        {
            string n = lines[index++];
            string exponent = lines[index++];
            string message = lines[index++];
            int mode = int.Parse(lines[index++]);

            sampleCases.Add(new TestCase
            {
                N = n,
                Exponent = exponent,
                Message = message,
                IsDecrypt = (mode == 1)
            });
        }

        return sampleCases;
    }

    static void WriteOutput(string outputPath, List<string> results)
    {
        File.WriteAllLines(outputPath, results);
    }
}
