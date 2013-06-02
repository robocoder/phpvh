using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PhpVH
{
    [Serializable, XmlType("ScanAlert")]
    public class ScanAlert
    {
        private ScanAlertOptions _alertType;

        [XmlAttribute]
        public ScanAlertOptions AlertType
        {
            get { return _alertType; }
            set { _alertType = value; }
        }

        private string _alertName;

        [XmlAttribute]
        public string AlertName
        {
            get { return _alertName; }
            set { _alertName = value; }
        }

        private FileTrace _trace;

        [XmlElement]
        public FileTrace Trace
        {
            get { return _trace; }
            set { _trace = value; }
        }

        public ScanAlert()
        {
        }

        public ScanAlert(ScanAlertOptions AlertType, string AlertName, FileTrace Trace)
        {
            _alertType = AlertType;
            _alertName = AlertName;
            _trace = Trace;
        }

        public override string ToString()
        {
            return "Alert Name: " + AlertName + "\r\n" +
                StringSanitizer.RemoveBeeps(Trace.Request) + "\r\n" +
                StringSanitizer.RemoveBeeps(Trace.Response) + "\r\n" +
                new string('-', 64) + "\r\n\r\n";
        }
    }
}
