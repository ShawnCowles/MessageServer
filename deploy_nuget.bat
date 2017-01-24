call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat" x86_amd64

cd src\MessageServer
nuget pack MessageServer.csproj -Symbols -Build -IncludeReferencedProjects -Properties Configuration=Release;Platform=AnyCpu
nuget push *.nupkg -Source https://www.nuget.org/api/v2/package
del *.nupkg

cd ../..

cd src\MessageServer.Contracts
nuget pack MessageServer.Contracts.csproj -Symbols -Build -IncludeReferencedProjects -Properties Configuration=Release;Platform=AnyCpu
nuget push *.nupkg -Source https://www.nuget.org/api/v2/package
del *.nupkg

echo "Deployment Complete"