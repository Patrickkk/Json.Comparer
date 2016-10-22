mode con:cols=140 lines=2500

nuget.exe update -self

CALL Build.bat

mkdir Publish
NuGet Pack Json.Comparer -OutputDirectory "D:\Nuget"
pause