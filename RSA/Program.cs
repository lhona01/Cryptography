using System;
using System.Numerics;
using System.Collections.Generic;

class RSA
{
    public static void Main(string[] args)
    {
        // Simulate input for demonstration
        BigInteger cipherText = BigInteger.Parse("66536047120374145538916787981868004206438539248910734713495276883724693574434582104900978079701174539167102706725422582788481727619546235440508214694579");
        BigInteger plainText = BigInteger.Parse("1756026041");

        Dictionary<string, BigInteger[]> keys = GenerateKeys(254, 1223, 251, 1339);

        BigInteger encryptedText = RSAEncryption(plainText, keys["publicKey"]);
        BigInteger decryptedText = RSADecryption(cipherText, keys["privateKey"]);

        Console.WriteLine("Encrypted Text: " + encryptedText);
        Console.WriteLine("Decrypted Text: " + decryptedText);
    }

    // Encryption: c = m^e mod n
    static BigInteger RSAEncryption(BigInteger plainText, BigInteger[] publicKey)
    {
        return BigInteger.ModPow(plainText, publicKey[0], publicKey[1]);
    }

    // Decryption: m = c^d mod n
    static BigInteger RSADecryption(BigInteger cipherText, BigInteger[] privateKey)
    {
        return BigInteger.ModPow(cipherText, privateKey[0], privateKey[1]);
    }

    // Generate RSA keys
    static Dictionary<string, BigInteger[]> GenerateKeys(int p_e, int p_c, int q_e, int q_c)
    {
        BigInteger p = BigInteger.Pow(2, p_e) - p_c;
        BigInteger q = BigInteger.Pow(2, q_e) - q_c;
        BigInteger n = p * q;
        BigInteger phi_n = (p - 1) * (q - 1);
        BigInteger e = 65537; // Commonly used public exponent

        BigInteger d = GeneratePrivateKey(e, phi_n);

        Dictionary<string, BigInteger[]> keys = new Dictionary<string, BigInteger[]>();
        keys.Add("publicKey", new BigInteger[] { e, n });
        keys.Add("privateKey", new BigInteger[] { d, n });

        return keys;
    }

    // Compute modular inverse using Extended Euclidean Algorithm
    static BigInteger GeneratePrivateKey(BigInteger e, BigInteger phi_n)
    {
        BigInteger x = 0, y = 1, u = 1, v = 0, a = phi_n, b = e;

        while (b != 0)
        {
            BigInteger q = a / b;
            BigInteger r = a % b;
            BigInteger m = x - u * q;
            BigInteger n = y - v * q;

            a = b;
            b = r;
            x = u;
            y = v;
            u = m;
            v = n;
        }

        if (x < 0) x += phi_n;
        return x;
    }
}
