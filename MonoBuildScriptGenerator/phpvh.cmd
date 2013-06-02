:: SourceDir must have trailing slash!
SET SourceDir=C:\source\PhpVH\
MonoBuildScriptGenerator "%SourceDir%PhpVH\PhpVH.csproj,%SourceDir%Components\Components.csproj" "%SourceDir%PhpVH\bin\debug\phpvhm.exe" "%SourceDir%PhpVH\b.bat"
CALL "%SourceDir%PhpVH\b.bat"