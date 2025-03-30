using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

class Hash
{
    private static readonly Random _random = new Random();

    public static void Main(string[] args)
    {
        Dictionary<string, string> md5Hash = new Dictionary<string, string>();
        bool collision = false;

        if (args.Length != 1)
        {
            Console.WriteLine("Usage: dotnet run <salt_in_hex>");
            return;
        }

        string saltHex = args[0];
        byte saltByte = Convert.ToByte(saltHex, 16);

        while (!collision)
        {
            string randomString = GetRandomString(10);
            byte[] stringBytes = Encoding.UTF8.GetBytes(randomString);
            byte[] bytesToHash = new byte[stringBytes.Length + 1];
            Array.Copy(stringBytes, bytesToHash, stringBytes.Length);
            bytesToHash[bytesToHash.Length - 1] = saltByte;

            byte[] hashBytes = MD5.Create().ComputeHash(bytesToHash);

            byte[] first5Bytes = new byte[5];
            Array.Copy(hashBytes, 0, first5Bytes, 0, 5);

            string hashString = BitConverter.ToString(first5Bytes).Replace("-", "");
            if (md5Hash.ContainsKey(hashString))
            {
                if (md5Hash[hashString] != randomString)
                {
                    Console.WriteLine($"{md5Hash[hashString]},{randomString}");
                    collision = true;
                    break;
                }
            }
            else
            {
                md5Hash.Add(hashString, randomString);
            }
        }
    }

    public static string GetRandomString(int length)
    {
        StringBuilder sb = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            sb.Append(GetRandomCharacter());
        }
        return sb.ToString();
    }

    public static char GetRandomCharacter()
    {
        int choice = _random.Next(3);

        if (choice == 0) // Digits 0-9
        {
            return (char)_random.Next(48, 58); // ASCII 48-57
        }
        else if (choice == 1) // Lowercase a-z
        {
            return (char)_random.Next(97, 123); // ASCII 97-122
        }
        else // Uppercase A-Z
        {
            return (char)_random.Next(65, 91); // ASCII 65-90
        }
    }
}