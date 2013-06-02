using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PhpVH
{
    public class MessageDumper
    {
        private DirectoryInfo _dumpDirectory;

        public MessageDumper(string path)
        {
            _dumpDirectory = new DirectoryInfo(path + @"\Messages");

            if (!_dumpDirectory.Exists)
                _dumpDirectory.Create();
        }

        public void Dump(string Message, int Number, MessageType Type)
        {
            var name = Number.ToString() + "_" + Type + ".txt";

            File.WriteAllText(_dumpDirectory.FullName + "\\" + name,
                Message);
        }
    }
}
