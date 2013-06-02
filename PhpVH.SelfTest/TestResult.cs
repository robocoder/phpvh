using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpVH.SelfTest
{
    public class TestResult
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }

        public bool Succeeded { get; set; }

        public bool Skipped { get; set; }

        public int Passed { get; set; }

        public int Total { get; set; }

        public TestResult()
        {
        }

        public TestResult(string name, string message, string details, bool succeeded, int passed, int total)
        {
            Name = name;
            Message = message;
            Details = details;
            Succeeded = succeeded;
            Passed = passed;
            Total = total;
        }
    }
}
