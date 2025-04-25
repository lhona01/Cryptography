using System;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

class RSA
{
    public static void Main(string[] args)
    {
        //BigInteger cipherText = BigInteger.Parse(args[4]);
        //BigInteger plainText = BigInteger.Parse(args[5]);
        BigInteger cipherText = BigInteger.Parse("66536047120374145538916787981868004206438539248910734713495276883724693574434582104900978079701174539167102706725422582788481727619546235440508214694579");
        BigInteger plainText = BigInteger.Parse("1756026041");

        //Dictionary<string, BigInteger[]> keys = GeneratePublicPrivateKey(
        //Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt32(args[2]), Convert.ToInt32(args[3]));

        Dictionary<string, BigInteger[]> keys = GeneratePublicPrivateKey(
        254, 1223, 251, 1339);

        BigInteger cipherText_to_plainText = RSAEncryption(plainText, keys["publicKey"]);
        BigInteger plainText_to_cipherText = RSADecryption(cipherText, keys["privateKey"]);

        Console.WriteLine("plainText: " + cipherText_to_plainText.ToString());
        Console.WriteLine("cipherText: " + plainText_to_cipherText.ToString());
    }

    static BigInteger RSAEncryption(BigInteger plainText, BigInteger[] key)
    {
        return BigInteger.ModPow(plainText, key[0], key[1]);
    }

    static BigInteger RSADecryption(BigInteger cipherText, BigInteger[] key) 
    {
        return BigInteger.ModPow(cipherText, key[0], key[1]);
    }

    static Dictionary<string, BigInteger[]> GeneratePublicPrivateKey(int p_e, int p_c, int q_e, int q_c)
    {
        Dictionary<string, BigInteger[]> keys = new Dictionary<string, BigInteger[]>();

        BigInteger p = BigInteger.Pow(2, p_e) - p_c;
        BigInteger q = BigInteger.Pow(2, q_e) - q_c;
        BigInteger n = p * q;
        BigInteger phi_n = (p - 1) * (q - 1);
        BigInteger e = 65537;

        BigInteger d = GeneratePrivateKey(e, phi_n);

        // public key
        keys.Add("publicKey", new BigInteger[] { e, n });
        // private key
        keys.Add("privateKey", new BigInteger[] { GeneratePrivateKey(e, phi_n), n });

        return keys;
    }

    static BigInteger GeneratePrivateKey(BigInteger e, BigInteger phi_n)
    {
        Stack<(BigInteger a, BigInteger b, BigInteger multiplier, BigInteger remainder)> stack = new Stack<(BigInteger a, BigInteger b, BigInteger c, BigInteger d)>();
        BigInteger a = phi_n;
        BigInteger b = e;

        // GCD Eucledian Algorithm
        do
        {
            BigInteger multiplier = BigInteger.Divide(a, b); // floor value
            BigInteger remainder = a % b;

            stack.Push((a, b, multiplier, remainder));

            if (remainder <= 1)
                break;

            a = b; b = remainder;
        } while (true);

        BigInteger x1 = -1;
        BigInteger y1 = -1;
        BigInteger x2 = -1;
        BigInteger y2 = -1;

        // Extended Eucledian Algorishm (inverse of mod)
        while (stack.Count > 0)
        {
            (BigInteger a, BigInteger b, BigInteger multiplier, BigInteger remainder) numbers = stack.Pop();
            
            if (x1 == -1)
            {
                x1 = numbers.a; y1 = 1; 
                x2 = numbers.b; y2 = -(numbers.multiplier);
                continue;
            }

            if (numbers.remainder == x1)
            {
                x1 = numbers.a;
                y2 = (1 - (x1 * y1)) / x2;
            }

            if (numbers.remainder == x2)
            {
                x2 = numbers.a;
                y1 = (1 - (x2 * y2)) / x1;
            }
        }

        return y1;
    }
}

