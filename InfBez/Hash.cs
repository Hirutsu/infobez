using System.Collections;

namespace Infbez.Task1
{
    public static class Hash
    {
        public static long HashFile(string fileName)
        {
            long sumHash = 0;
            byte[] bytes = File.ReadAllBytes(fileName);
            BitArray bytesB = new(bytes);
            for (int i = 0; i < bytesB.Count - 1; i += 1)
            {
                bytesB[0] = bytesB[0] ^ bytesB[1];
            }
            return sumHash;
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