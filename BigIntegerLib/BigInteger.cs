using System;
using System.Collections.Generic;
using System.Text;

namespace BigIntegerLib;

public class BigInteger
{
    private List<int> digits;
    int Base = 1000000000; // 10^9
    int DigitLength = 9;

    // 0000000 1234567 890123456 

    // digits = { "890123456 ", "1234567 "}
    // Constructor
    public BigInteger()
    {
        digits = new List<int> { 0 }; // Initialize with zero
    }

    public BigInteger(string number)
    {

        if (string.IsNullOrEmpty(number))
        {
            throw new ArgumentException("Enter a Number");
        }

        digits = new List<int>();

        for (int i = number.Length; i > 0; i -= DigitLength)
        {
            int start = Math.Max(0, i - DigitLength);
            string chunk = number.Substring(start, i - start);

            if (!int.TryParse(chunk, out int digit))
            {
                throw new ArgumentException("Invalid");
            }

            digits.Add(digit);

        }

        RemoveLeadingZeros();

    }

    void RemoveLeadingZeros()
    {

        while (digits.Count > 1 && digits[^1] == 0)
        {
            digits.RemoveAt(digits.Count - 1);
        }
    }

    public BigInteger(int value) : this(value.ToString()) { }
    public BigInteger(long value) : this(value.ToString()) { }

    // Convert back to string
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(digits[^1].ToString()); // Add highest digit without padding

        // Add remaining digits with leading zeros
        for (int i = digits.Count - 2; i >= 0; i--)
        {
            sb.Append(digits[i].ToString().PadLeft(DigitLength, '0'));
        }

        return sb.ToString();
    }

    //operators
    public static bool operator <(BigInteger a, BigInteger b)
    {
        if (a.digits.Count != b.digits.Count)
            return a.digits.Count < b.digits.Count;

        for (int i = a.digits.Count - 1; i >= 0; i--)
        {
            if (a.digits[i] != b.digits[i])
                return a.digits[i] < b.digits[i];
        }

        return false; // equal
    }

    public static bool operator >(BigInteger a, BigInteger b)
    {
        if (a.digits.Count != b.digits.Count)
            return a.digits.Count > b.digits.Count;

        for (int i = a.digits.Count - 1; i >= 0; i--)
        {
            if (a.digits[i] != b.digits[i])
                return a.digits[i] > b.digits[i];
        }

        return false; // equal
    }
    //elgded
    public static bool operator <=(BigInteger a, BigInteger b)
    {
        return (a < b) || (a == b);
    }

    public static bool operator >=(BigInteger a, BigInteger b)
    {
        return (a > b) || (a == b);
    }

    public static bool operator ==(BigInteger a, BigInteger b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

        if (a.digits.Count != b.digits.Count)
            return false;

        for (int i = 0; i < a.digits.Count; i++)
        {
            if (a.digits[i] != b.digits[i])
                return false;
        }

        return true;
    }
    //
    public static bool operator !=(BigInteger a, BigInteger b)
    {
        return !(a == b);
    }
 public override bool Equals(object obj)
    {
        if (obj is not BigInteger) return false;
        return this == (BigInteger)obj;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (int digit in digits)
        {
            hash = hash * 31 + digit;
        }
        return hash;
    }
    // ========== Basic Operations ==========

    public BigInteger Add(BigInteger a, BigInteger b)
    {
        BigInteger result = new BigInteger();
        result.digits.Clear();

        int carry = 0;
        int length = Math.Max(a.digits.Count, b.digits.Count);

        for(int i = 0; i < length || carry > 0; i++)
        {
            int sum = carry;
            if (i < a.digits.Count) sum += a.digits[i];
            if (i < b.digits.Count) sum += b.digits[i];

            carry = sum / Base;
            result.digits.Add(sum % Base);
        }

        return result;

    }

   
   
    public BigInteger Subtract(BigInteger a, BigInteger b)
    {

        if (a < b) throw new ArgumentException("a must be greater than or equal to b");

        BigInteger result = new BigInteger();
        result.digits.Clear();


        int borrow = 0;
        for (int i = 0; i < a.digits.Count; i++)
        {
            int diff = a.digits[i] - borrow;
            if (i < b.digits.Count) diff -= b.digits[i];

            if (diff < 0)
            {
                diff += Base;
                borrow = 1;
            }
            else
            {
                borrow = 0;
            }

            result.digits.Add(diff);
        }

        result.RemoveLeadingZeros();
        return result;

    }

    public bool IsOdd()
    {
        return !IsEven();
    }

    public bool IsEven()
    {

        return digits.Count == 0 || digits[0] % 2 == 0;
    }

    // ========== Advanced Operations ==========

    public BigInteger Multiply(BigInteger x, BigInteger y)
    {
        {
            x.RemoveLeadingZeros();
            y.RemoveLeadingZeros();


            if (x.digits.Count == 1 && y.digits.Count == 1)
            {
                long product = (long)x.digits[0] * y.digits[0];
                return new BigInteger(product);
            }

            int n = Math.Max(x.digits.Count, y.digits.Count);
            int m = n / 2;


            BigInteger low1 = x.Split(0, m);
            BigInteger high1 = x.Split(m, x.digits.Count - m);
            BigInteger low2 = y.Split(0, m);
            BigInteger high2 = y.Split(m, y.digits.Count - m);

            BigInteger z0 = Karatsuba(low1, low2);
            BigInteger z1 = Karatsuba(low1 + high1, low2 + high2);
            BigInteger z2 = Karatsuba(high1, high2);

            BigInteger result = ShiftLeft(z2, 2 * m) + ShiftLeft(z1 - z2 - z0, m) + z0;
            result.RemoveLeadingZeros();
            return result;
        }


        private static BigInteger ShiftLeft(BigInteger a, int m)
        {
            if (a == new BigInteger("0")) return new BigInteger("0");

            BigInteger result = new BigInteger();
            result.digits = new List<int>(new int[m]);
            result.digits.AddRange(a.digits);
            return result;
        }

        private BigInteger Split(int start, int count)
        {
            BigInteger result = new BigInteger();
            result.digits.Clear();

            for (int i = start; i < start + count && i < digits.Count; i++)
                result.digits.Add(digits[i]);

            result.RemoveLeadingZeros();
            return result;
        }
       
    }

    public (BigInteger Quotient, BigInteger Remainder) Divide(BigInteger other)
    {
        public (BigInteger Quotient, BigInteger Remainder) Divide(BigInteger divisor)
        {
            if (divisor.IsZero())
                throw new DivideByZeroException("Cannot divide by zero.");

            if (this.IsZero())
                return (new BigInteger(0), new BigInteger(0));

            BigInteger dividend = new BigInteger();
            dividend.digits = new List<int>(this.digits); 

            BigInteger quotient = new BigInteger();
            quotient.digits.Clear();

            BigInteger remainder = new BigInteger();
            remainder.digits.Clear();

            for (int i = dividend.digits.Count - 1; i >= 0; i--)
            {
                remainder.digits.Insert(0, dividend.digits[i]);
                remainder.RemoveLeadingZeros();

                int low = 0, high = Base - 1, best = 0;
                while (low <= high)
                {
                    int mid = (low + high) / 2;
                    BigInteger trial = divisor * new BigInteger(mid);
                    if (trial <= remainder)
                    {
                        best = mid;
                        low = mid + 1;
                    }
                    else
                    {
                        high = mid - 1;
                    }
                }

                quotient.digits.Insert(0, best);
                remainder = remainder - (divisor * new BigInteger(best));
            }

            quotient.RemoveLeadingZeros();
            remainder.RemoveLeadingZeros();

            return (quotient, remainder);
        }
    }
}


