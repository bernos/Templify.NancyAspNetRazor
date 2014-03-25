@echo off

%systemroot%\Microsoft.Net\Framework64\v4.0.30319\MSBuild.exe build.proj /t:Templify /p:Configuration=Release /p:VisualStudioVersion=12.0


pause
exit
