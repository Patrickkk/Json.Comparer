mode con:cols=140 lines=2500

nuget.exe update -self

CALL Build.bat

mkdir Publish
NuGet Pack Newtonsoft.Json.Comparer.nuspec -OutputDirectory Publish
Nuget push "\Publish\Newtonsoft.Json.Comparer*.nupkg"
rmdir Publish /s /q
pause