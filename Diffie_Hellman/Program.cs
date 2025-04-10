using System.Collections;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;


class DiffieHellman
{
    public static void Main(string[] args)
    {
        int g_e = int.Parse(args[1]);
        int g_c = int.Parse(args[2]);
        int N_e = int.Parse(args[3]);
        int N_c = int.Parse(args[4]);

        byte[] initializationVector = hexStringToByteArray(args[0]);
        byte[] cipherText = hexStringToByteArray(args[7]);
        string plainText = args[8];

        // public values
        BigInteger g = BigInteger.Pow(2, g_e) - g_c;
        BigInteger N = BigInteger.Pow(2, N_e) - N_c;

        // sent by bob
        BigInteger partialKey = BigInteger.Parse(args[6]);


        // Alice private num
        BigInteger keyFinder = BigInteger.Parse(args[5]);

        byte[] key = BigInteger.ModPow(partialKey, keyFinder, N).ToByteArray();

        string decryptedText = DecryptStringFromBytes_Aes(cipherText, key, initializationVector);
        string encryptedText = byteArrayToHexString(EncryptStringToBytes_Aes(plainText, key, initializationVector));
         
        Console.WriteLine(decryptedText + "," + encryptedText);
    }

    static byte[] hexStringToByteArray(string hexString)
    {
        return hexString.Split(' ').Select(hex => Convert.ToByte(hex, 16)).ToArray();
    }

    static string byteArrayToHexString(byte[] byteArray)
    {
        return BitConverter.ToString(byteArray).Replace("-", " ");
    }

    static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        byte[] encrypted;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                }

                encrypted = msEncrypt.ToArray();
            }
        }

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext = null;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }
}