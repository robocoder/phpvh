using Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace PhpVH
{
    public static class PhpResource
    {
        private const string ResourcePath = "PhpResources";

        public static string Load(string name)
        {
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            var dir = Directory.GetCurrentDirectory();
            var filename = string.Format("{0}.php", name);
            var searchPaths = new[] 
            {
                PathHelper.GetExecutingPath(ResourcePath, filename),
                Path.Combine(Directory.GetCurrentDirectory(), ResourcePath, filename)
            };

            foreach (var p in searchPaths)
            {
                if (File.Exists(p))
                {
                    return File.ReadAllText(p);
                }
            }

            throw new FileNotFoundException(string.Format("Could not find PHP resource {0}.", filename));
        }
    }
}
