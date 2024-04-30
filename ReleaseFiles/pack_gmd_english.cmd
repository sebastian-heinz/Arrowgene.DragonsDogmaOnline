pushd "%~dp0 "
cd ./Server.
Arrowgene.Ddon.Cli.exe client packGmd romDir=%1 gmdCsv="%~dp0/Server/Files/Client/gmd.csv" romLang="English"
cmd