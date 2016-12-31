call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat" x86_amd64

msbuild /p:Configuration=Release /p:Platform="Any CPU"

cd src\MessageServer

nuget pack MessageServer.csproj

nuget push *.nupkg -Source https://www.nuget.org/api/v2/package