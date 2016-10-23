mode con:cols=140 lines=2500

nuget.exe update -self

CALL Build.bat

mkdir Publish
NuGet Pack Json.Comparer.nuspec -OutputDirectory Publish
Nuget push "\Publish\Json.Comparer*.nupkg"
rmdir Publish /s /q
pause