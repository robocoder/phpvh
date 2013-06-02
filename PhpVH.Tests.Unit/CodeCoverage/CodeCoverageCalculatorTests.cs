using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PhpVH.CodeCoverage;

namespace PhpVH.Tests.Unit.CodeCoverage
{
    [TestFixture, Category("CodeCoverage")]
    public class CodeCoverageCalculatorTests
    {
        [Test]
        public void CalculateCoverage_FileNotExists_ReturnsDefault()
        {
            var calculator = new CodeCoverageCalculator(null, new AnnotationTable())
            {
                FileReader = new StubFileReader
                {
                    ExistsValue = false
                }
            };

            var coverageTable = calculator.CalculateCoverage();

            Assert.AreEqual(new CodeCoverageTable(), coverageTable);
        }

        [Test]
        public void CalculateCoverage_FileExists_WithoutLines_ReturnsDefault()
        {
            var calculator = new CodeCoverageCalculator(null, new AnnotationTable())
            {
                FileReader = new StubFileReader
                {
                    ExistsValue = false
                }
            };

            var coverageTable = calculator.CalculateCoverage();

            Assert.AreEqual(new CodeCoverageTable(), coverageTable);
        }

        [Test]
        public void CalculateCoverage_FileExists_WithLines_ReturnsCodeCoverageTable()
        {
            var annotations = new[]
            {
                new { fileName = "first", id = 1, index = 2, hitCount = 1, coverage = 100.0m, line = "first_1" },
                new { fileName = "second", id = 2, index = 8, hitCount = 3, coverage = 100.0m, line = "second_2" },
                new { fileName = "third", id = 3, index = 13, hitCount = 0, coverage = 0.0m, line = string.Empty }
            };

            var annotationTable = new AnnotationTable();
            foreach (var a in annotations)
            {
                annotationTable.Add(a.fileName);
                annotationTable[a.fileName].Add(new Annotation(a.id, a.index, a.hitCount));
            }

            var calculator = new CodeCoverageCalculator(null, annotationTable)
            {
                FileReader = new StubFileReader
                {
                    ExistsValue = true,
                    GetLinesValue = annotations.Select(a => a.line)
                }
            };

            var actualCoverage = calculator.CalculateCoverage();

            var expectedCoverage = new CodeCoverageTable
            {
                Total = (decimal)2 / 3 * 100
            };

            foreach (var a in annotations)
                expectedCoverage.Add(a.fileName, a.coverage);

            Assert.AreEqual(expectedCoverage.Plugin, actualCoverage.Plugin);
            Assert.AreEqual(expectedCoverage.Total, actualCoverage.Total);
            Assert.AreEqual(expectedCoverage.ToString(), actualCoverage.ToString());
            CollectionAssert.AreEqual(expectedCoverage, actualCoverage);
        }

        [Test]
        public void CalculateCoverage_DoNotReadAnnotations_ReturnsCodeCoverageTable()
        {
            var annotations = new[]
            {
                new { fileName = "first", hitCount = 3, totalCount = 3, coverage = 100.0m, line = "first_1" },
                new { fileName = "second", hitCount = 4, totalCount = 5, coverage = 80.0m, line = "second_2" },
                new { fileName = "third", hitCount = 8, totalCount = 10, coverage = 80.0m, line = "third_3" },
                new { fileName = "fourth", hitCount = 1, totalCount = 5, coverage = 20.0m, line = "fourth_4" },
                new { fileName = "fifth", hitCount = 3, totalCount = 30, coverage = 10.0m, line = "fifth_5" }
            };

            var annotationTable = new AnnotationTable();
            int id = 0;
            foreach (var a in annotations)
            {
                annotationTable.Add(a.fileName);
                int hitCount = a.hitCount;
                for (int i = 0; i < a.totalCount; i++)
                    annotationTable[a.fileName].Add(new Annotation(id++, id, hitCount-- > 0 ? 1 : 0));
            }

            var calculator = new CodeCoverageCalculator(annotationTable);

            var actualCoverage = calculator.CalculateCoverage(false);

            var expectedCoverage = new CodeCoverageTable
            {
                Total = 35.84905660377358490566037736m
            };

            foreach (var a in annotations)
                expectedCoverage.Add(a.fileName, a.coverage);

            Assert.AreEqual(expectedCoverage.Plugin, actualCoverage.Plugin);
            Assert.AreEqual(expectedCoverage.Total, actualCoverage.Total);
            Assert.AreEqual(expectedCoverage.ToString(), actualCoverage.ToString());
            CollectionAssert.AreEqual(expectedCoverage, actualCoverage);
        }
    }

    internal class StubFileReader : IFileReader
    {
        public IEnumerable<string> GetLinesValue { get; set; }

        public bool ExistsValue { get; set; }

        public StubFileReader()
        {
            GetLinesValue = new List<string>();
        }

        public IEnumerable<string> GetLines()
        {
            return GetLinesValue;
        }

        public bool Exists()
        {
            return ExistsValue;
        }
    }
}
