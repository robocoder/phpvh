using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Components.ConsolePlus;

namespace PhpVH
{
    public class ReportWriter
    {
        private List<ReportFile> _reportFiles = new List<ReportFile>();

        public List<ReportFile> ReportFiles
        {
            get { return _reportFiles; }
        }

        public DirectoryInfo ReportPath { get; private set; }

        public ReportWriter(string serverPath)
        {
            ReportPath = new DirectoryInfo(@".\Reports\" +
                serverPath.Replace('/', '_') + " Scan Reports " +
                DateTime.Now.ToString("MM-dd-yyyy-HHmmss"));
            ReportPath.Create();
        }

        public string Write(string name, string report, string extension)
        {
            var file = ReportPath + "\\" + name + "." + extension;

            _reportFiles.Add(new ReportFile(name, name + "." + extension));

            if (!string.IsNullOrEmpty(report.ToString()))
            {
                File.WriteAllText(file, report.ToString());

                Cli.WriteLine("\r\n~White~~|DarkBlue~{0} report written to {1}~R~", name, file);                

                return file;
            }

            return null;
        }

        public string Write(string name, string report)
        {
            return Write(name, report, "txt");
        }

        public string WriteFilenames()
        {
            var filename = ReportPath + "\\files.rxml";
            using (var s = File.Create(filename))
                new XmlSerializer(typeof(List<ReportFile>)).Serialize(s, _reportFiles);
            return filename;
        }
    }
}
