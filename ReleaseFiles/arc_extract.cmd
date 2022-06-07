pushd "%~dp0"
set CUR_DIR=%CD%
cd ./Server.
Arrowgene.Ddon.Cli.exe client "%~1" extract="%CUR_DIR%"
cmd