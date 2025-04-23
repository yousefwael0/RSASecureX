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

    // Required matching > operator
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

    public BigInteger Multiply(BigInteger other)
    {
        throw new NotImplementedException();
    }

    public (BigInteger Quotient, BigInteger Remainder) Divide(BigInteger other)
    {
        throw new NotImplementedException();
    }
}
