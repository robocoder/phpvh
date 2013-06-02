using System.Collections.Generic;

namespace PhpVH.CodeCoverage
{
    public interface IFileReader
    {
        IEnumerable<string> GetLines();

        bool Exists();
    }
}
