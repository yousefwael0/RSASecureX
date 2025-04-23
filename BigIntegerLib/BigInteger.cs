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

    // digits = {  "1234567 ", "890123456 ", " "}
    // Constructor
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
}
