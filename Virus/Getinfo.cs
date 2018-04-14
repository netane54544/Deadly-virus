using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overwatch
{
    class Getinfo
    {
        public string User, Path;
        public int ID;

        public Getinfo(string i_User, int i_ID, string i_Path)
        {
            this.User = i_User;
            this.ID = i_ID;
            this.Path = i_Path;
        }

    }
}
