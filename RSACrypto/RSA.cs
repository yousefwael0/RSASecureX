using BigIntegerLib;
using System;
using System.Security.Cryptography;

namespace RSACrypto;

public class RSA
{
    private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

    // Generate public key: (e, n)
    public (BigInteger e, BigInteger n) GeneratePublicKey(int primeDigits = 64)
    {
        BigInteger p = GenerateRandomPrime(primeDigits);
        BigInteger q;

        do q = GenerateRandomPrime(primeDigits);
        while (q == p);

        BigInteger n = p * q;
        BigInteger phi = (p - BigInteger.One) * (q - BigInteger.One);

        BigInteger e = new BigInteger(65537);
        if (BigInteger.GCD(e, phi) != BigInteger.One)
        {
            e = new BigInteger(3);
            while (e < phi && BigInteger.GCD(e, phi) != BigInteger.One)
                e = e + BigInteger.Two;
        }

        return (e, n);
    }

    public BigInteger Encrypt(BigInteger M, BigInteger e, BigInteger n)
        => ModPow(M, e, n);

    public BigInteger Decrypt(BigInteger cipher, BigInteger d, BigInteger n)
        => ModPow(cipher, d, n);

    private BigInteger ModPow(BigInteger baseVal, BigInteger exp, BigInteger mod)
    {
        if (mod == BigInteger.One) return BigInteger.Zero;
        BigInteger result = BigInteger.One;
        baseVal %= mod;

        while (!exp.IsZero())
        {
            if (exp.IsOdd())
                result = (result * baseVal) % mod;

            exp = exp / BigInteger.Two;
            baseVal = (baseVal * baseVal) % mod;
        }

        return result;
    }

    public BigInteger GenerateRandomPrime(int digits)
    {
        while (true)
        {
            BigInteger candidate = RandomBigIntegerWithDigits(digits);
            if (IsProbablyPrime(candidate, 5))
                return candidate;
        }
    }

    // Replaced Fermat with Miller–Rabin for better performance
    private bool IsProbablyPrime(BigInteger n, int rounds)
    {
        if (n <= BigInteger.One) return false;
        if (n == BigInteger.Two || n == new BigInteger(3)) return true;
        if (n.IsEven()) return false;

        BigInteger d = n - BigInteger.One;
        int r = 0;

        while (d.IsEven())
        {
            d = d / BigInteger.Two;
            r++;
        }

        for (int i = 0; i < rounds; i++)
        {
            BigInteger a = RandomBigInteger(BigInteger.Two, n - BigInteger.Two);
            BigInteger x = ModPow(a, d, n);

            if (x == BigInteger.One || x == n - BigInteger.One)
                continue;

            bool passed = false;
            for (int j = 0; j < r - 1; j++)
            {
                x = ModPow(x, BigInteger.Two, n);
                if (x == n - BigInteger.One)
                {
                    passed = true;
                    break;
                }
            }

            if (!passed) return false;
        }

        return true;
    }

    private BigInteger RandomBigInteger(BigInteger min, BigInteger max)
    {
        int digits = max.ToString().Length;
        BigInteger result;

        do
        {
            result = RandomBigIntegerWithDigits(digits);
        } while (result < min || result >= max);

        return result;
    }

    private BigInteger RandomBigIntegerWithDigits(int digits)
    {
        string number = RandomDigitChar(1, 9).ToString();
        for (int i = 1; i < digits; i++)
            number += RandomDigitChar(0, 9);

        return new BigInteger(number);
    }

    private int RandomDigitChar(int min, int max)
    {
        byte[] b = new byte[1];
        rng.GetBytes(b);
        return (b[0] % (max - min + 1)) + min;
    }
}