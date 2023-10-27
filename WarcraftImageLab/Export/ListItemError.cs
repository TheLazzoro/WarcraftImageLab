using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarcraftImageLab.Export
{
    public class ListItemError
    {
        public string FullPath { get; }
        public string ErrorMsg { get; }

        public ListItemError(string fullPath, string errorMsg)
        {
            FullPath = fullPath;
            ErrorMsg = errorMsg;
        }
    }
}
