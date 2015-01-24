using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PhpVH
{
    public static class Php
    {
        public const string NonNameStartRegex = @"([^a-zA-Z_\x7f-\xff])";

        public const string NonNameEndRegex = @"([^a-zA-Z0-9_\x7f-\xff])";

        public const string ValidNameRegex = @"[a-zA-Z_\x7f-\xff][a-zA-Z0-9_\x7f-\xff]*";

        public const string ValidNameEndRegex = @"[a-zA-Z0-9_\x7f-\xff]";        
    }
}
