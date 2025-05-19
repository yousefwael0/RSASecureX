using BigIntegerLib;
using System;
using System.Security.Cryptography;

namespace RSACrypto;

public class RSA
{
    private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

    // Generate public key: (e, n)
    public (BigInteger e, BigInteger n) GeneratePublicKey(int primeDigits = 617)
    {
        BigInteger p = GenerateRandomPrime(primeDigits);
        BigInteger q;
        do
        {
            q = GenerateRandomPrime(primeDigits);
        } while (q == p); // ensure p != q

        BigInteger n = p * q;
        BigInteger phi = (p - BigInteger.One) * (q - BigInteger.One);

        BigInteger e = new BigInteger(65537); // common public exponent

        if (BigInteger.GCD(e, phi) != BigInteger.One)
        {
            int maxTries = 10;
            do
            {
                e = GenerateRandomPrime(3);
                if (--maxTries == 0)
                    throw new Exception("Failed to find a valid public exponent e.");
            } while (BigInteger.GCD(e, phi) != BigInteger.One);
        }

        return (e, n);
    }

    public BigInteger Encrypt(BigInteger M, BigInteger e, BigInteger n)
    {
        return ModPow(M, e, n);
    }

    public BigInteger Decrypt(BigInteger cipher, BigInteger d, BigInteger n)
    {
        return ModPow(cipher, d, n);
    }

    private BigInteger ModPow(BigInteger baseVal, BigInteger exp, BigInteger mod)
    {
        if (mod == BigInteger.One) return BigInteger.Zero;
        if (exp.IsZero()) return BigInteger.One;

        baseVal %= mod;
        BigInteger result = BigInteger.One;

        while (!exp.IsZero())
        {
            if (exp.IsOdd())
                result = (result * baseVal) % mod;

            exp = exp / BigInteger.Two;
            baseVal = (baseVal * baseVal) % mod;
        }

        return result;
    }

    private bool IsProbablyPrime(BigInteger n, int rounds = 5)
    {
        if (n <= BigInteger.One) return false;
        if (n == BigInteger.Two) return true;

        for (int i = 0; i < rounds; i++)
        {
            BigInteger a = RandomBigInteger(2, 1000);

            if (BigInteger.GCD(a, n) != BigInteger.One)
                return false;

            if (ModPow(a, n - BigInteger.One, n) != BigInteger.One)
                return false;
        }

        return true;
    }

    public BigInteger GenerateRandomPrime(int numDigits)
    {
        while (true)
        {
            BigInteger candidate = RandomBigIntegerWithDigits(numDigits);

            if (IsProbablyPrime(candidate))
                return candidate;
        }
    }

    private BigInteger RandomBigIntegerWithDigits(int digits)
    {
        string number = RandomDigitChar(1, 9).ToString();
        for (int i = 1; i < digits; i++)
            number += RandomDigitChar(0, 9);

        return new BigInteger(number);
    }

    private BigInteger RandomBigInteger(int min, int max)
    {
        byte[] bytes = new byte[4];
        rng.GetBytes(bytes);
        int value = BitConverter.ToInt32(bytes, 0);
        value = Math.Abs(value % (max - min + 1)) + min;
        return new BigInteger(value);
    }

    private int RandomDigitChar(int min, int max)
    {
        byte[] b = new byte[1];
        rng.GetBytes(b);
        return (b[0] % (max - min + 1)) + min;
    }
}