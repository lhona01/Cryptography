
class Program
{
    public static void Main(string[] args)
    {
        //string[] inputBytes = args[0].Split(' ');
        string[] inputBytes = ["B1", "FF", "FF", "CC", "98", "80", "09", "EA", "04", "48", "7E", "C9"];
        string dataToHide = "";

        // Convert each hex byte to binary string
        for (int i = 0; i < inputBytes.Length; i++)
        {
            byte byteValue = Convert.ToByte(inputBytes[i], 16);
            dataToHide += Convert.ToString(byteValue, 2).PadLeft(8, '0'); // Ensure 8 bits for each byte
        }

        // Original bitmap bytes (example)
        byte[] bmpBytes = new byte[] {
                    0x42,0x4D,0x4C,0x00,0x00,0x00,0x00,0x00,
                    0x00,0x00,0x1A,0x00,0x00,0x00,0x0C,0x00,
                    0x00,0x00,0x04,0x00,0x04,0x00,0x01,0x00,
                    0x18,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,
                    0x00,0x00,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                    0xFF,0x00,0x00,0x00,0xFF,0xFF,0xFF,0x00,
                    0x00,0x00,0xFF,0x00,0x00,0xFF,0xFF,0xFF,
                    0xFF,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0xFF,
                    0xFF,0x00,0x00,0x00,0xFF,0xFF,0xFF,0x00,
                    0x00,0x00
        };


        // Hide 2 bits per byte
        int hideBitIndex = 0;
        for (int i = 26; i < bmpBytes.Length; i++)
        {
            if (hideBitIndex + 2 <= dataToHide.Length)
            {
                // Hide bits
                string binary = Convert.ToString(bmpBytes[i], 2).PadLeft(8, '0');
                char[] binaryChars = binary.ToCharArray();


                binaryChars[6] = XOR(binaryChars[6], dataToHide[hideBitIndex]);
                binaryChars[7] = XOR(binaryChars[7], dataToHide[hideBitIndex + 1]);

                //Convert modified binary back to byte
                byte modifiedByte = Convert.ToByte(new string(binaryChars), 2);
                bmpBytes[i] = modifiedByte;

                hideBitIndex += 2;
            }
            else
            {
                Console.WriteLine("Error: Not enough data to hide.");
                break;
            }
        }

        // Print modified bitmap bytes
        for (int i = 0; i < bmpBytes.Length; i++)
        {
            Console.Write(bmpBytes[i].ToString("X2") + " ");
        }

        Console.WriteLine(); // Ensure there's a line break after printing
    }
    public static char XOR(char a, char b)
    {
        if ((a == '0' && b == '0') || (a == '1' && b == '1'))
            return '0';
        else
            return '1';
    }
}