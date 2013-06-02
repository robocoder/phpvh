using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if !MONO && !NET35
using System.Windows.Forms;
#endif
using System.Diagnostics;
using PhpVH.ScanPlugins;
using System.Text.RegularExpressions;
using Components.ConsolePlus;

namespace PhpVH
{
    public class ScannerCli
    {
        public static void DisplayAppInfo()
        {
            var appName = "PHP Vulnerability Hunter";
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Cli.WriteLine(
                "~{0}~~|{1}~[~{2}~+~{0}~ {3} {4} ~{2}~+~{0}~]~R~\r\n",
                ConsoleColor.White,
                ConsoleColor.DarkGreen,
                ConsoleColor.Cyan,
                appName,
                version);            
        }

        public static void DisplayInstructions()
        {
            Cli.WriteLine(
@"== Before You Start

For PHP Vulnerability Hunter to successfully run the following 
conditions must be met: 

1) PHP Vulnerability Hunter must be run as administrator 

2) The targeted web application must not be accessed while PHP 
Vulnerability Hunter is running 

3) Only one instance of PHP Vulnerability Hunter per webroot can
be running at any time 

                 
== Arguments

phpvh [-s] [-p] [-m] [-t] [-c] [-c2] [-d] [-v] [-b] [-h] [-r]
    [-dump] [-log] [-static] webroot apps

webroot       Absolute web root (required)

apps          Application paths (comma delimited, required)
              Use * to scan every application in the webroot

-s            Server (default localhost)

-p            Port (default 80)

-m            Scan modes (default CFLPSXRI)
              C - Arbitrary Command Execution 
              F - Arbitrary File Write/Change/Rename/Delete 
              L - Local File Inclusion/Arbitrary File Read 
              P - Arbitrary PHP Execution 
              S - SQL Injection 
              D - Dynamic Function Call/Class Instantiation
              X - Reflected Cross-site Scripting (XSS)
              R - Open Redirect
              I - Full Path Disclosure

-t timeout    Timeout in milliseconds (default 60000)

-c            Code coverage report

-c2           High accuracy code coverage report 
              WARNING: may cause slowdown and timeouts 

-d            Scan overview report

-v            Open vulnerability report viewer

-b            Beep on vulnerability alert

-r            Repair mode
              Use to repair an application if PHP 
              Vulnerability Hunter crashes during a scan

-dump         Dump all http messages

-log          Log console output

-static       Static analysis only

== Examples

phpvh c:\xampp\htdocs MyApp 
Runs all scans on app located at c:\xampp\htdocs\MyApp

phpvh -m CX c:\xampp\htdocs MyApp1,MyApp2 
Runs Command Execution and Reflected XSS scans on applications 
located at c:\xampp\htdocs\MyApp1 and c:\xampp\htdocs\MyApp2

phpvh c:\xampp\htdocs * 
Runs all scans on every folder in the webroot");


        }

        public static void DisplayCriticalMessage(string Message, params object[] Arguments)
        {
            Cli.WriteLine(Message, Arguments);            
        }

        public static void DisplayCriticalMessageAndExit(string Message, params object[] Arguments)
        {
            DisplayCriticalMessage(Message, Arguments);

            Environment.Exit(0);
        }

        public static void DisplayScanPlugin(ScanPluginBase Plugin)
        {
            Cli.WriteLine("~Cyan~{0}~R~", Plugin.ToString());
        }

        public static void DisplayResourcePath(string Path)
        {
            Cli.WriteLine("~White~Target: {0}~R~", Path);
        }

        public static void DisplayResponse(HttpResponse Response, 
            int Mode, int InputCount, long Milliseconds, int respLength)
        {
            var resp = !string.IsNullOrEmpty(Response.Header) ?
                    Response.Header.Split('\r', '\n')[0] :
                    "[No Header]";

            var inputColor = 
                InputCount >= 20 ? ConsoleColor.Red :
                InputCount > 0 ? ConsoleColor.Yellow :
                ConsoleColor.Gray;

            var timeColor = 
                Milliseconds >= 20000 ? ConsoleColor.Red :
                Milliseconds >= 10000 ? ConsoleColor.Yellow :
                ConsoleColor.Green;

            var respLengthColor =
                respLength >= 1000000 ? ConsoleColor.Red :
                respLength >= 100000 ? ConsoleColor.Yellow :
                ConsoleColor.Green;

            var respColor =
               Regex.IsMatch(resp, @"\s[45]\d{2}($|[^\d])") ? ConsoleColor.Red :
               Regex.IsMatch(resp, @"\s2\d{2}($|[^\d])") ? ConsoleColor.Green :
               ConsoleColor.DarkCyan;           

            Cli.WriteLine(
                "{{Mode: {0}, Input Count: ~{1}~{2}~R~}} -> ~{3}~{4}~R~ (~{5}~{6:n0} bytes~R~ in ~{7}~{8}ms~R~)",
                Mode, 
                inputColor, 
                InputCount,
                respColor,
                resp,
                respLengthColor,
                respLength,
                timeColor,
                Milliseconds);            
        }

        public static void DisplayResponseError(string Response)
        {
            Cli.WriteLine("~Red~Error reading response~R~");
        }

        public static void DisplayPhaseName(string name)
        {
            Cli.WriteLine(
                "~{0}~~|{1}~{2}~R~",
                ConsoleColor.Blue,
                ConsoleColor.White,
                name);            
        }

        public static void DisplayAlert(ScanAlert Alert)
        {
            var color = Alert.AlertType == ScanAlertOptions.Vulnerability ? 
                ConsoleColor.Red : ConsoleColor.Yellow;

            var msg = 
                Alert.AlertType == ScanAlertOptions.Vulnerability ?  "Potential Vulnerability" :
                Alert.AlertType == ScanAlertOptions.Warning ?  "Warning" :
                "Request Data";

            Cli.WriteLine("~{0}~{1}: {2}~R~", color, msg, Alert.AlertName);
        }

        public static void DisplayError(string Error)
        {
            Cli.WriteLine("~Red~{0}~R~", Error);
        }

        public static void DisplayAppPath(string Path)
        {
            Cli.WriteLine("Scanning ~Cyan~{0}~R~", Path);
        }

        public static void DisplayDiscoveredUrl(string url)
        {
            Cli.WriteLine("[~Green~+~R~] {0}", url);     
        }

        public static void DisplayScrapedUrl(string url, IEnumerable<FormTag> forms)
        {
            var count = forms.SelectMany(x => x.Inputs).Count();

            var countColor =
                count > 10 ? ConsoleColor.Red :
                count > 0 ? ConsoleColor.Yellow :
                ConsoleColor.Green;

            Cli.WriteLine(
                "{0} -> ~{1}~{2}~R~", 
                url,
                countColor,
                count);           
        }

        public static void RunAssistant()
        {
#if !MONO && !NET35
            var file = @".\hr";

            var showDialog = 
                !Environment.GetCommandLineArgs().Any(x => x == "-l") &&
                !File.Exists(file);

            File.Create(file).Close();

            if (showDialog)
            {
                if (MessageBox.Show(@"It seems this is the first time PHP " +
                    "Vulnerability Hunter has been run on this system. " + 
                    "Would you like to use the launcher instead of the command line " + 
                    "interface?", "Run Launcher",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var guiFile = "phpvh-gui.exe";

                    if (!File.Exists(guiFile))
                    {
                        ScannerCli.DisplayError(string.Format("Could not find launcher {0}", guiFile));
                        Environment.Exit(6);
                    }

                    Process.Start(guiFile);
                    Environment.Exit(0);
                }
            }
#endif            
        }
    }
}
