using Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleasePackager
{
    public static class FileSystemInfoHelper
    {
        public static bool TryDelete(FileSystemInfo info)
        {
            if (!info.Exists)
            {
                return false;
            }

            var success = true;
            var dir = info as DirectoryInfo;

            if (dir != null)
            {
                var children = dir
                    .GetFiles()
                    .OfType<FileSystemInfo>()
                    .Concat(dir.GetDirectories());

                foreach (var f in children)
                {
                    if (!TryDelete(f))
                    {
                        success = false;
                    }
                }                
            }

            info.Delete();

            return success;
        }
    }
}
