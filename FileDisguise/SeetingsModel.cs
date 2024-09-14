using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDisguise
{
    public class SeetingsModel
    {
        private string _ExperienceCode;
        public string ExperienceCode
        {
            get { return _ExperienceCode; }
            set { _ExperienceCode = value; }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
    }
}
