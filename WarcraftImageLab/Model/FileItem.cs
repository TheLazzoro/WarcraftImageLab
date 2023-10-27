using System.IO;

namespace WarcraftImageLab.Model
{
    internal class FileItem
    {
        public string FileName
        {
            get
            {
                return Path.GetFileName(FullPath);
            }
        }
        public string FullPath { get; set; }
        public string Size { get; set; }

        public FileItem(string fullPath)
        {
            FullPath = fullPath;
            FileInfo info = new(fullPath);

            if(info.Length < 1024)
            {
                Size = $"{info.Length} bytes";
            }
            else if(info.Length < 1024 * 1000)
            {
                Size = $"{info.Length / 1024} KB";
            }
            else
            {
                Size = $"{info.Length / (1024 * 1000)} MB";
            }
        }
    }
}
