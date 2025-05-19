using BigIntegerLib;
using System.Text;

using RSACrypto;

namespace RSAStringCrypto;

public class StringCrypto
{
    // Overall Complexity: O(n)
    static string BigIntegerToString(BigInteger number)
    {
        string numStr = number.ToString(); // O(n), n = number of digits

        while (numStr.Length % 3 != 0) // At most 2 iterations
            numStr = "0" + numStr; // O(n) for string concatenation

        StringBuilder sb = new StringBuilder(); // O(1)

        for (int i = 0; i < numStr.Length; i += 3) // O(n)
        {
            int ascii = int.Parse(numStr.Substring(i, 3)); // O(1)
            sb.Append((char)ascii); // O(1)
        }
        return sb.ToString(); // O(n)
    }

    // Overall Complexity: O(k * log(e) *  m^1.585) where e is the exponent, k number of chunks and m number of digits of n
    public static List<BigInteger> EncryptString(string text, BigInteger e, BigInteger n)
    {
        List<BigInteger> encryptedChunks = new List<BigInteger>(); // O(1)
        RSA rsa = new RSA(); // O(1)

        int maxDigits = 0; // O(1)
        string testString = ""; // O(1)

        while (true)
        {
            testString += "255"; // O(m), m is current length in the loop
            if (new BigInteger(testString) >= n) // O(k), k is the length of testString
                break; // O(1)
            maxDigits += 3; // O(1)
        }

        if (maxDigits == 0) // O(1)
            throw new ArgumentException("Modulus too small to encode even one byte."); // O(1)

        int i = 0; // O(1)
        while (i < text.Length)
        {
            string encodedChunk = ""; // O(1)
            int charCount = 0; // O(1)

            while (i + charCount < text.Length && encodedChunk.Length + 3 <= maxDigits) // O(n), n is text length
            {
                byte b = (byte)text[i + charCount]; // O(1)
                encodedChunk += b.ToString("D3"); // O(1)
                charCount++; // O(1)
            }

            BigInteger m = new BigInteger(encodedChunk); // O(b), b is number of digits in the chunk
            BigInteger c = rsa.Encrypt(m, e, n); // O(log(e) x k^1.585)
            encryptedChunks.Add(c); // O(1)
            i += charCount; // O(1)
        }

        return encryptedChunks; // O(1)
    }


    // Overall Complexity: O(k * log(d) * m^1.585) where d is the private exponent, k number of encrypted chunks and m is number of digits of n

    public static string DecryptChunks(List<BigInteger> encryptedChunks, BigInteger d, BigInteger n)
    {
        StringBuilder result = new StringBuilder(); // O(1)
        RSA rsa = new RSA(); // O(1)

        foreach (var chunk in encryptedChunks)
        {
            BigInteger m = rsa.Decrypt(chunk, d, n); // O(log(e) x k^1.585)
            string part = BigIntegerToString(m); // O(n)
            result.Append(part); //O(n)
        }

        return result.ToString(); // O(n)
    }
}