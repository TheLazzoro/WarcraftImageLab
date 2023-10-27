using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarcraftImageLab.Model;
using WarcraftImageLab.Utility;

namespace WarcraftImageLab.Content
{
    internal class MainControlViewModel : ViewModelBase
    {
        private ObservableCollection<FileItem> _fileItems = new ObservableCollection<FileItem>();

        #region Public propterties
        public ObservableCollection<FileItem> FileItems
        {
            get { return _fileItems; }
        }
        #endregion

        public event Action OnFileAdded;

        public void AddFileToList(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                var item = new FileItem(fullPath);
                _fileItems.Add(item);
                OnFileAdded?.Invoke();
            }
        }

        public void AddFilesToList(string[] listFullPath)
        {
            for (int i = 0; i < listFullPath.Length; i++)
            {
                string fullPath = listFullPath[i];
                AddFileToList(fullPath);
            }
        }

        public void RemoveFileAt(int index)
        {
            _fileItems.RemoveAt(index);
        }
    }
}
