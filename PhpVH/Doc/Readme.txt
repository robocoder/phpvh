PHP Vulnerability Hunter

== Description

PHP Vulnerability Hunter is a PHP web application fuzz tool that scans for several 
different vulnerabilities by performing dynamic program analysis. 

It can detect the following vulnerabilities:

Arbitrary Command Execution
Arbitrary File Write/Change/Rename/Delete
Local File Inclusion/Arbitrary File Read
Arbitrary PHP Execution
SQL Injection
Dynamic Function Call/Class Instantiation
Reflected Cross-site Scripting (XSS)
Open Redirect
Full Path Disclosure


== Before You Start

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

-h            Hook superglobals
              Alternative to the superglobal override class
              Use to test different code paths or if 
              superglobal override causes errors, although  
              hooking frequently causes errors itself

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
Runs all scans on every folder in the webroot


== Change Log
1.3.87.0
Several improvements to SQL injection scanning
Added static analysis based vulnerability detection
Multiple static analysis improvements
Updated launcher
Optimized code coverage memory usage
Several improvements and fixes to code coverage
Several lexer fixes and optimizations
Improved spidering
Overhauled hooking
Several CLI enhancements
Misc error handling fixes
Improved arbitrary upload scan
Command scan now uses probe exe rather than calc, no longer blocking responses
Added input map and code coverage views to report viewer
Added annotation report
Added code coverage message
Added plugin config files
Added new fuzz strings to command injection plugin
Added console logging
Added automatic repair
Added unit tests
Added integration tests
Added code coverage commenting
Fixed variable discovery infinite loop bug
Fixed several crashing bugs
Fixed file deletion false positives
Fixed multiple local file inclusion scan bugs
Fixed arbitrary PHP execution scan bug
Fixed bug that caused phpb files to be scanned on windows 7 machines
Fixed hooking include bug
Fixed race condition in init

1.2.0.2
Fixed crash caused by space in path name

1.2.0.1
Added tooltips to GUI
Added input map report
Added automatic error reporting
Added connection timeout setting
Added port setting
Added code coverage accuracy options
Added static analysis phase
Added dynamic function call/class instantiation scan
Added superglobal hook option
Added repair mode
Minor CLI tweaks
Changed default timeout to 60 seconds
Scan mode and input count now shown with each response
Several improvements to code annotation
Updated help menu shortcut to point to local copy of guide
Several launcher improvements
Improved XSS scan
Reports and dumps are now written to subdirectory
Alert messages are now sanitized to remove beep chars
Fixed GUI window size
Fixed client connection error handling
Fixed multiple http implementation bugs

1.1.4.6
Added code coverage report
Updated GUI validation
Several instrumentation fixes
Fixed lingering connection issue
Fixed GUI and report viewer crashes related to working directory

1.1.3.1
Improved arbitrary upload scan
Improved local file inclusion scan
Improved input discovery
Updated crawler
Added report viewer captured data filter
Added option to dump all HTTP messages
Added option to toggle alert beep
Added help option to GUI
Several report viewer UI tweaks
Minor CLI tweaks
Several preloading fixes
Several file scan fixes
Http implementation fixes
Fixed endless loop in input discovery
Fixed GUI crash bug
Fixed unhook crash bug
Fixed connection error handling

1.1.0.6
Http client fix
Several improvements to local file inclusion/arbitrary read scan
Several preloading fixes
Fixed bug in command scan
Added run assistant
Renamed PHPVHLauncher to PHPVH-GUI
Fixed minor interface bugs
Skinned and tweaked interface of report viewer

1.0.9.1
Hooking algorithm fix
File scan improvements
Local file inclusion scan improvements
Fixed post scan cleanup fixes
Fixed bug caused by capitlizing PHP in opening tags
Application now pauses after displaying instructions
XSS scan algorithm fixes
Open redirect scan algorithm added
Fixed bug with -s option
Added GUI launcher
Http implementation fixes

1.0.6.7
Several improvements to file scan algorithm
Several improvements to XSS scan algorithm
Added full path disclosure algorithm
Updated help
Http client fixes
Updated interface

1.0.5.7
Fixed issue file scan caused by document root being on a drive other than C
Fixed issue with XSS anchors not properly incrementing
Several improvements made to SQL injection monitoring
Added cookie value fuzzing
* option added for application path
Added log viewer
Added -v command line option to open viewer upon scan completion
Added new fuzz strings to arbitrary PHP execution scan
Improved arbitrary PHP execution scan algorithm
Fixed bug that caused false positives in arbitary PHP execution scan
Made discovery report optional with -d argument
Several updates to the command scan
Updated hook file
Hook algorithm updates
Several hooking bug fixes
Local file inclusion scan updates

1.0.2.2
Scan algorithm updates that improve the effectiveness of every type of scan
Updated report naming scheme to improve readability
Updated directions
Added user friendly error messages
Minor interface updates
Removed SQL error warnings
Fixed several crash bugs
Miscellaneous bug fixes

1.0.0.9
Added scan overview report
Updated SQL injection scan
Updated command scan
Minor interface updates
Fixed bug caused by use of <? open tag

1.0.0.0
Initial release