set CONFIG=Release
::::::::::::::::::::::::::::::::
:: .NET 4.5
::::::::::::::::::::::::::::::::
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild PhpVH.sln /p:DefineConstants="TRACE" /t:PhpVH:rebuild;PhpVHGui:rebuild;PhpVHReportViewer:rebuild /p:Configuration=%CONFIG% /p:Platform="Any CPU" /p:OutDir="%cd%\\bin\\.NET 4.5\\"

COPY "PhpVHGui\bin\%CONFIG%\PhpVH-GUI.exe" "PhpVH\bin\%CONFIG%\PhpVH-GUI.exe"
COPY "PhpVHReportViewer\bin\%CONFIG%\PhpVHReportViewer.exe" "PhpVH\bin\%CONFIG%\PhpVHReportViewer.exe"

::::::::::::::::::::::::::::::::
:: .NET 3.5
::::::::::::::::::::::::::::::::
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild PhpVH.sln /tv:3.5 /p:DefineConstants="TRACE;NET35" /t:PhpVH:rebuild /p:Configuration=%CONFIG% /p:Platform="Any CPU" /p:OutDir="%cd%\\bin\\.NET 3.5 (no GUI)\\"

DEL /s "%cd%\bin\*.pdb"