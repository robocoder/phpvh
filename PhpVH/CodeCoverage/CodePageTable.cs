using Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpVH.CodeCoverage
{
    public class CodePageTable : Dictionary<string, List<CodePage>>
    {
        public string ToHtml()
        {
            var templateFilename = PathHelper.GetEntryPath("CodeCoverage", "IndexTemplate.html");
            var template = File.ReadAllText(templateFilename);
            var fileList = this
                .Select(x => string.Format(
                    "<li>{0} <ul>{1}</ul></li>",
                    x.Key,
                    x.Value
                        .Select(y => string.Format(@"
                            <li>
                                <a href=""#"" onclick=""setViewerPage('{1}')"">{0}</a>
                            </li>
                        ",
                            y.Name,
                            y.Filename.Replace("\\", "\\\\")))
                        .DefaultIfEmpty()
                        .Aggregate()))
                .Aggregate();

            fileList = "<ul>" + fileList + "</ul>";

            return string.Format(template, fileList);
        }
    }
}
