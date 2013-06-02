using System;

namespace PhpVH.Tests
{
    public abstract class HtmlTest : ExternalResourceTest
    {
        protected override string GetExtension()
        {
            return "Html";
        }
    }
}
