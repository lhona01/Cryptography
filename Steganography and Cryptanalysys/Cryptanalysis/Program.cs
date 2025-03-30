

using System;
using System.Security;
using System.Security.Cryptography;


public class Cryptanalysis
{
    public static void Main(string[] args)
    {
        string secretString = args[0];
        string cipherString = args[1];
        bool seedFound = false;
        
        DateTime startDt = new DateTime(2020, 7, 3, 11, 0, 0);
        DateTime endDt = new DateTime(2020, 7, 4, 11, 0, 0);

        for (DateTime currDt = startDt; startDt < endDt; currDt = currDt.AddMinutes(1))
        {
            TimeSpan ts = currDt.Subtract(new DateTime(1970, 1, 1));
            
            Random rng = new Random((int)ts.TotalMinutes);
            byte[] key = BitConverter.GetBytes(rng.NextDouble());

            if (cipherString == Encrypt(key, secretString))
            {
                Console.WriteLine(ts.TotalMinutes);
                seedFound = true;
                break;
            }
        }

        if (seedFound == false)
        {
            Console.WriteLine("seed not found!");
        }
    }
    private static string Encrypt(byte[] key, string secretString)
    {
        DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms,
        csp.CreateEncryptor(key, key), CryptoStreamMode.Write);
        StreamWriter sw = new StreamWriter(cs);
        sw.Write(secretString);
        sw.Flush();
        cs.FlushFinalBlock();
        sw.Flush();
        return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
    }
}