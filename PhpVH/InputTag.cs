using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH
{
    public class InputTag : HtmlTag
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }

        public int? Size { get; set; }

        public int? MaxLength { get; set; }
    }
}
