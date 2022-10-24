using System.Text;

namespace Infbez
{
    [Serializable]
    public class Folder
    {
        public List<string> Folders;
        public Dictionary<string, string> Files;
        static readonly Encoding enc = Encoding.GetEncoding(1251);

        public Folder(List<string> folders, Dictionary<string, string> files)
        {
            Folders = folders;
            Files = files;
        }

        public Folder(string path)
        {
            Folders = GetFoldersList(path, path.Length);
            Files = GetFilesList(path, path.Length);
        }

        public void DeployFolder(string rootPath)
        {
            foreach (string folder in Folders)
            {
                Directory.CreateDirectory(rootPath + folder);
            }

            foreach (var item in Files)
            {
                using FileStream fs = new(rootPath + item.Key, FileMode.Create);
                byte[] bytes = enc.GetBytes(item.Value);
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        static List<string> GetFoldersList(string path, int initPathLength)
        {
            List<string> foldersList = new();
            string[] dirs = Directory.GetDirectories(path);
            foldersList.AddRange(dirs.Select(x => x.Remove(0, initPathLength)));

            foreach (string subdirectory in dirs)
            {
                try
                {
                    foldersList.AddRange(GetFoldersList(subdirectory, initPathLength));
                }
                catch { }
            }
            return foldersList;
        }

        static Dictionary<string, string> GetFilesList(string path, int initPathLength)
        {
            Dictionary<string, string> files = new();
            string[] dirs = Directory.GetDirectories(path);

            foreach (string filename in Directory.GetFiles(path))
            {
                files.Add(filename.Remove(0, initPathLength), enc.GetString(File.ReadAllBytes(filename)));
            }

            foreach (string subdirectory in dirs)
            {
                try
                {
                    GetFilesList(subdirectory, initPathLength).ToList().ForEach(x => files.Add(x.Key, x.Value));
                }
                catch { }
            }
            return files;
        }
    }
}
