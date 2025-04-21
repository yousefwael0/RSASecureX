using System;
using System.Collections.Generic;

namespace BigIntegerLib;

public class BigInteger
{
    private List<int> digits;

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
}
