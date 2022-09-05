REM https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore2x
SET /p VERSION=<ddon.version
SET RUNTIMES=win-x64
SET ZIP="C:\Program Files\7-Zip\7z.exe"
mkdir .\release
(for %%x in (%RUNTIMES%) do ( 
REM Clean
if exist .\publish\%%x-%VERSION%\ RMDIR /S /Q .\publish\%%x-%VERSION%\
REM Server
dotnet publish Arrowgene.Ddon.Cli\Arrowgene.Ddon.Cli.csproj /p:Version=%VERSION% --runtime %%x --self-contained --configuration Release --output ./publish/%%x-%VERSION%/Server
REM ReleaseFiles
xcopy .\ReleaseFiles .\publish\%%x-%VERSION%\
REM PACK
REM if exist %ZIP% %ZIP% -ttar a dummy .\publish\%%x-%VERSION%\* -so | %ZIP% -si -tgzip a .\release\%%x-%VERSION%.tar.gz
))
REM keep console open
cmd