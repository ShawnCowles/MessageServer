call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat" x86_amd64

cd src\MessageServer

nuget pack MessageServer.csproj -Symbols -Build

nuget push *.nupkg -Source https://www.nuget.org/api/v2/package

del *.nupkg

pause