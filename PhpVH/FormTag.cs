using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH
{
    public class FormTag : HtmlTag
    {
        public string Action { get; set; }

        public string Method { get; set; }

        public InputTag[] Inputs { get; set; }
    }
}
