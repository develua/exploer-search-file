using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_work_2_1
{
    class FilePath
    {
        public string Name { get; private set; }
        public string PathFile { get; private set; }

        public FilePath(string n, string pf)
        {
            Name = n;
            PathFile = pf;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
