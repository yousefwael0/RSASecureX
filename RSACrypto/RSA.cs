using BigIntegerLib;

namespace RSACrypto;

public class RSA
{
    // Encrypt a message M with public key (e, n)
    public BigInteger Encrypt(BigInteger M, BigInteger e, BigInteger n)
    {
        // E(M) = M^e MOD n
        BigInteger res = new BigInteger("1");
        while (e > BigInteger.Zero){
            if (e.IsOdd()){
                res = (res * M) % n;
            }
            M = (M * M) % n;
            e /= BigInteger.Two;
        }
        return res;
    }

    // Decrypt a cipher E(M) with private key (d, n)
    public BigInteger Decrypt(BigInteger cipher, BigInteger d, BigInteger n)
    {
        // M = cipher ^ d MOD n
        BigInteger res = new BigInteger("1");
        while (d > BigInteger.Zero){
            if (d.IsOdd()){
                res = (res * cipher) % n;
            }
            cipher = (cipher * cipher) % n;
            d /= BigInteger.Two;
        }
        return res;
    }
}
