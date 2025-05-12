using BigIntegerLib;
using System.Text;

using RSACrypto;

namespace RSAStringCrypto;

public class StringCrypto
{
    static BigInteger StringToBigInteger(string text)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in text)
        {
            sb.Append(((int)c).ToString("D3"));
        }
        return new BigInteger(sb.ToString());
    }

    static string BigIntegerToString(BigInteger number)
    {
        string numStr = number.ToString();

        while (numStr.Length % 3 != 0)
            numStr = "0" + numStr;

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < numStr.Length; i += 3)
        {
            int ascii = int.Parse(numStr.Substring(i, 3));
            sb.Append((char)ascii);
        }
        return sb.ToString();
    }

    public static List<BigInteger> EncryptString(string text, BigInteger e, BigInteger n)
    {
        List<BigInteger> encryptedChunks = new List<BigInteger>();
        RSA rsa = new RSA();

        // Step 1: Determine maximum safe encoded byte length (as decimal digits) that fits in n
        int maxDigits = 0;
        string testString = "";

        while (true)
        {
            testString += "255"; // maximum value of a byte (3 digits)
            if (new BigInteger(testString) >= n)
                break;
            maxDigits += 3;
        }

        if (maxDigits == 0)
            throw new ArgumentException("Modulus too small to encode even one byte.");

        // Step 2: Encrypt chunks
        int i = 0;
        while (i < text.Length)
        {
            string encodedChunk = "";
            int charCount = 0;

            // Add characters one by one until the encoded number would exceed n
            while (i + charCount < text.Length && encodedChunk.Length + 3 <= maxDigits)
            {
                byte b = (byte)text[i + charCount];
                encodedChunk += b.ToString("D3"); // always 3 digits
                charCount++;
            }

            BigInteger m = new BigInteger(encodedChunk);
            BigInteger c = rsa.Encrypt(m, e, n);
            encryptedChunks.Add(c);
            i += charCount;
        }

        return encryptedChunks;
    }


    public static string DecryptChunks(List<BigInteger> encryptedChunks, BigInteger d, BigInteger n)
    {
        StringBuilder result = new StringBuilder();
        RSA rsa = new RSA();

        foreach (var chunk in encryptedChunks)
        {
            BigInteger m = rsa.Decrypt(chunk, d, n);
            string part = BigIntegerToString(m);
            result.Append(part);
        }

        return result.ToString();
    }
}
