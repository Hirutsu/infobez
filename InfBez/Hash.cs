using System.Collections;

namespace Infbez.Task1
{
    public static class Hash
    {
        public static long HashFile(string fileName)
        {
            byte[] bytes = File.ReadAllBytes(fileName);
            BitArray allbytes = new(bytes);
            BitArray newAllBytes;
            if (allbytes.Count % 16 == 0)
            {
                return sumXOR(allbytes);
            }
            else
            {
                newAllBytes = new(allbytes.Count + 8, false);
                for (int i = 0; i < allbytes.Count; i++)
                {
                    newAllBytes[i] = allbytes[i];
                }
                return sumXOR(newAllBytes);
            }
        }

        public static long sumXOR(BitArray bitArray)
        {
            for (int i=0; i < bitArray.Count - 16; i+=16)
            {
                bitArray[0] ^= bitArray[i + 16];
                bitArray[1] ^= bitArray[i + 16];
                bitArray[2] ^= bitArray[i + 16];
                bitArray[3] ^= bitArray[i + 16];
                bitArray[4] ^= bitArray[i + 16];
                bitArray[5] ^= bitArray[i + 16];
                bitArray[6] ^= bitArray[i + 16];
                bitArray[7] ^= bitArray[i + 16];
                bitArray[8] ^= bitArray[i + 16];
                bitArray[9] ^= bitArray[i + 16];
                bitArray[10] ^= bitArray[i + 16];
                bitArray[11] ^= bitArray[i + 16];
                bitArray[12] ^= bitArray[i + 16];
                bitArray[13] ^= bitArray[i + 16];
                bitArray[14] ^= bitArray[i + 16];
                bitArray[15] ^= bitArray[i + 16];
            }

            long sumXOR = 0;

            for (int i = 0; i < 16; i++)
            {
                if (bitArray[i])
                    sumXOR += Convert.ToInt32(Math.Pow(2, i));
            }

            return sumXOR;
        }

        public static List<KeyValuePair<string, long>> GetFiles(string directory, ref List<KeyValuePair<string, long>> files)
        {
            List<string> dir = Directory.GetDirectories(directory).ToList();
            foreach (string file in Directory.GetFiles(directory))
                files.Add(new KeyValuePair<string, long>(file.ToString(), HashFile(file)));
            foreach (string item in dir)
                GetFiles(item, ref files);        
            return files;
        }

        public static void Save(string infoFile, List<KeyValuePair<string, long>> files)
        {
            List<string> values = new();
            foreach (var item in files)
            {
                values.Add(item.Key + "-> Hash:" + item.Value);
            }
            File.WriteAllLines(infoFile, values);
        }
    }
}