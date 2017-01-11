using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_work_2_1
{
    class SearchData
    {
        public string PathFolder { get; set; }
        public string Text { get; set; }
        public Action<FilePath> AddNewFileToList { get; set; }
        public Action<int> PogressBarMax { get; set; }
        public Action PogressBarIncement { get; set; }
    }
}
