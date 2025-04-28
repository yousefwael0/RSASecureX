using BigIntegerLib;
using RSACrypto;
using System.Text;

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

    static List<BigInteger> EncryptString(string text, BigInteger e, BigInteger n)
    {
        List<BigInteger> encryptedChunks = new List<BigInteger>();
        RSA rsa = new RSA();

        int chunkSize = 0;
        
        while (true)
        {
            string temp = "";
            for (int i = 0; i < chunkSize + 1; i++) temp += "255";
            BigInteger test = new BigInteger(temp);

            if (test >= n)
                break;
            chunkSize++;
        }

        chunkSize--;

        for (int i = 0; i < text.Length; i += chunkSize)
        {
            string part = text.Substring(i, Math.Min(chunkSize, text.Length - i));
            BigInteger m = StringToBigInteger(part);
            BigInteger c = rsa.Encrypt(m, e, n);
            encryptedChunks.Add(c);
        }

        return encryptedChunks;
    }

    static string DecryptChunks(List<BigInteger> encryptedChunks, BigInteger d, BigInteger n)
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
