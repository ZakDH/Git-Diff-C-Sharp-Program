using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment
{
    public class namingfile
    {
        protected string FileName;

        public void SetFilename(String pName)
        {
            FileName = pName;
        }
    }
    public class MissingData : namingfile
    {
        public void RenameFile(String pNewName)
        {
            FileName = pNewName;
        }
    }
}
