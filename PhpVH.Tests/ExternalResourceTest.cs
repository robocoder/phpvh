using System;
using System.IO;

namespace PhpVH.Tests
{
    public abstract class ExternalResourceTest
    {
        protected abstract string GetExtension();

        protected virtual string GetFolder()
        {
            return GetExtension();
        }

        protected string LoadResource(string name)
        {
            return File.ReadAllText(string.Format(
                "{0}\\{1}.{2}", 
                GetFolder(),
                name,
                GetExtension()));
        }
    }
}
