using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public static class DirectoryInfoExtension
    {
        public static DirectoryInfo Combine(this DirectoryInfo dir, params string[] paths)
        {
            return new DirectoryInfo(Path.Combine(new[] { dir.FullName }.Concat(paths).ToArray()));
        }

        public static void CopyTo(this DirectoryInfo sourceDir, string destinationDir, bool overwrite)
        {
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            foreach (var f in sourceDir.GetFiles())
            {
                f.CopyTo(Path.Combine(destinationDir, f.Name), overwrite);
            }

            foreach (var subDir in sourceDir.GetDirectories())
            {
                subDir.CopyTo(Path.Combine(destinationDir, subDir.Name), overwrite);
            }
        }

        public static void CopyTo(this DirectoryInfo sourceDir, string destinationDir)
        {
            CopyTo(sourceDir, destinationDir, false);
        }
    }
}
