using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarcraftImageLab.Model;
using WarcraftImageLab.Utility;

namespace WarcraftImageLab.Export
{
    internal class ExportWindowViewModel : ViewModelBase
    {
        private ObservableCollection<ListItemError> _fileItems = new ObservableCollection<ListItemError>();

        #region Public propterties
        public ObservableCollection<ListItemError> FileItems
        {
            get { return _fileItems; }
        }
        #endregion

        public void AddErrorToList(ListItemError error)
        {
            _fileItems.Add(error);
        }
    }
}
