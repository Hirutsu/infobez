namespace Infbez.Task1
{
    public static class Hash
    {
        public static int HashFile(string fileName)
        {
            int sumHash = 0;
            byte[] bytes = File.ReadAllBytes(fileName);
            for (int i = 0; i < bytes.Length - 2; i += 2)
                sumHash += bytes[i] ^ bytes[i + 1];
            return sumHash;
        }

        public static List<KeyValuePair<string, int>> GetFiles(string directory, ref List<KeyValuePair<string, int>> files)
        {
            List<string> dir = Directory.GetDirectories(directory).ToList();
            foreach (string file in Directory.GetFiles(directory))
                files.Add(new KeyValuePair<string, int>(file.ToString(), HashFile(file)));
            foreach (string item in dir)
                GetFiles(item, ref files);        
            return files;
        }

        public static void Save(string infoFile, List<KeyValuePair<string, int>> files)
        {
            File.WriteAllText(infoFile, files.ToString());
        }
    }
}