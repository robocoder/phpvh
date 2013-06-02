using System.Collections.Generic;
using System.IO;

namespace PhpVH.CodeCoverage
{
    public class AnnotationFileReader : IFileReader
    {
        private readonly FileInfo _annotationFile;

        public AnnotationFileReader(FileInfo annotationFile)
        {
            _annotationFile = annotationFile;
        }

        public IEnumerable<string> GetLines()
        {
            string entry;
            using (var reader = new StreamReader(_annotationFile.OpenRead()))
                while ((entry = reader.ReadLine()) != null)
                    yield return entry;
        }

        public bool Exists()
        {
            return _annotationFile.Exists;
        }
    }
}
