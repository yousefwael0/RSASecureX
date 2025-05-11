using BigIntegerLib;
using System;

namespace RSACrypto;

public class RSA
{
    // Encrypt a message M with public key (e, n)
    public BigInteger Encrypt(BigInteger M, BigInteger e, BigInteger n)
    {
        throw new NotImplementedException();
    }

    // Decrypt a cipher E(M) with private key (d, n)
    public BigInteger Decrypt(BigInteger cipher, BigInteger d, BigInteger n)
    {
        throw new NotImplementedException();
    }
}
