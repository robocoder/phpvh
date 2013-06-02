using System;
using System.IO;
using System.Linq;
using Components;

namespace PhpVH.CodeCoverage
{
    public class CodeCoverageCalculator
    {
        public IFileReader FileReader { get; set; }

        private readonly AnnotationTable _annotationTable;

        public CodeCoverageCalculator(FileInfo annotationFile, AnnotationTable annotationTable)
        {
            FileReader = new AnnotationFileReader(annotationFile);
            _annotationTable = annotationTable;
        }

        public CodeCoverageCalculator(AnnotationTable annotationTable)
            : this (null, annotationTable)
        {
        }

#if NET35
        public CodeCoverageTable CalculateCoverage()
        {
            return CalculateCoverage(true);
        }

        public CodeCoverageTable CalculateCoverage(bool readAnnotations)
#else
        public CodeCoverageTable CalculateCoverage(bool readAnnotations = true)
#endif
        {
            var entryTable = new CodeCoverageTable();

            if (readAnnotations)
            {
                if (!FileReader.Exists())
                    return entryTable;

                var entries = FileReader.GetLines();

                foreach (var entry in entries)
                {
                    try
                    {
                        var filename = entry.RemoveAtLastIndexOf('_');
                        var id = int.Parse(entry.SubstringAtLastIndexOf('_', 1));
                        _annotationTable[filename][id].HitCount++;
                    }
#if DEBUG
                    catch (Exception ex)
                    {
                        ScannerCli.DisplayError(ex.ToString());
                    }
#else
                catch { }
#endif
                }
            }

            Func<Annotation, bool> wasHit = x => x.HitCount > 0;

            lock (_annotationTable)
            {
                var hitBlockCount = (decimal)_annotationTable
                    .Items
                    .SelectMany(x => x.Items.Where(wasHit))
                    .Count();

                var totalBlockCount = _annotationTable
                    .Items
                    .SelectMany(x => x.Items)
                    .Count();

                if (totalBlockCount != 0)
                    entryTable.Total = hitBlockCount / totalBlockCount * 100;

                _annotationTable
                    .Items
                    .Select(x => new
                    {
                        x.Filename,
                        HitBlockCount = x.Items.Count(wasHit),
                        TotalBlockCount = x.Items.Count,
                        Coverage = (decimal)x.Items.Count(wasHit) / x.Items.Count * 100,
                    })
                    .Iter(x => entryTable.Add(x.Filename, x.Coverage));
            }

            entryTable.Plugin = _annotationTable.Plugin;

            return entryTable;
        }
    }
}
