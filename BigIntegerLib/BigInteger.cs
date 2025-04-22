using System;
using System.Collections.Generic;

namespace BigIntegerLib;

public class BigInteger
{
    private List<int> digits;
    public static readonly BigInteger Zero = new BigInteger("0");
    public static readonly BigInteger One = new BigInteger("1");
    public static readonly BigInteger Two = new BigInteger("2");

    // Constructor
    public BigInteger(string number)
    {
        // TODO: Parse the string into digits
        throw new NotImplementedException();
    }

    // Convert back to string
    public override string ToString()
    {
        throw new NotImplementedException();
    }

    // ========== Basic Operations ==========

    public BigInteger Add(BigInteger other)
    {
        throw new NotImplementedException();
    }

    public BigInteger Subtract(BigInteger other)
    {
        throw new NotImplementedException();
    }

    public bool IsOdd()
    {
        throw new NotImplementedException();
    }

    public bool IsEven()
    {
        throw new NotImplementedException();
    }

    // ========== Advanced Operations ==========

    public BigInteger Multiply(BigInteger other)
    {
        throw new NotImplementedException();
    }

    public (BigInteger Quotient, BigInteger Remainder) Divide(BigInteger other)
    {
        throw new NotImplementedException();
    }
    
    // ========= Operator Overloading =============

    public static BigInteger operator +(BigInteger a, BigInteger b)
    {
        return a.Add(b);
    }

    public static BigInteger operator -(BigInteger a, BigInteger b)
    {
        return a.Subtract(b);
    }

    public static BigInteger operator *(BigInteger a, BigInteger b)
    {
        return a.Multiply(b);
    }

    public static BigInteger operator /(BigInteger a, BigInteger b)
    {
        return a.Divide(b).Quotient;
    }

    public static BigInteger operator %(BigInteger a, BigInteger b)
    {
        return a.Divide(b).Remainder;
    }
    
    public static bool operator ==(BigInteger a, BigInteger b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        if (a.digits.Count != b.digits.Count) return false;
        for (int i = 0; i < a.digits.Count; i++)
        {
            if (a.digits[i] != b.digits[i]) return false;
        }
        return true;
    }

    public static bool operator !=(BigInteger a, BigInteger b) => !(a == b);

    public static bool operator <(BigInteger a, BigInteger b)
    {
        if (a.digits.Count != b.digits.Count)
            return a.digits.Count < b.digits.Count;
        for (int i = a.digits.Count - 1; i >= 0; i--)
        {
            if (a.digits[i] != b.digits[i])
                return a.digits[i] < b.digits[i];
        }
        return false;
    }

    public static bool operator >(BigInteger a, BigInteger b) => b < a;

    public static bool operator <=(BigInteger a, BigInteger b) => (a < b) || (a == b);

    public static bool operator >=(BigInteger a, BigInteger b) => (a > b) || (a == b);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        if (obj is null || obj.GetType() != this.GetType())
            return false;

        BigInteger other = (BigInteger)obj;

        if (digits.Count != other.digits.Count)
            return false;

        // Compare from most significant digit first
        for (int i = digits.Count - 1; i >= 0; i--)
        {
            if (digits[i] != other.digits[i])
                return false;
        }
        return true;
    }

    public override int GetHashCode()
    {
        // If the number is huge, don't hash all digits — sample a few
        int hash = 17;
        int step = Math.Max(1, digits.Count / 10); // Sample 10 digits max

        for (int i = 0; i < digits.Count; i += step)
        {
            hash = hash * 31 + digits[i];
        }
        return hash;
    }
}
