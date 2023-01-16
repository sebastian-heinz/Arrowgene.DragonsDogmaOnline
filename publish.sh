#!/usr/bin/env bash
# https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore2x
read -r VERSION<ddon.version
mkdir ./release
for RUNTIME in linux-x64 osx-x64; do
    # Server
    dotnet publish Arrowgene.Ddon.Cli/Arrowgene.Ddon.Cli.csproj /p:Version=$VERSION --runtime $RUNTIME --self-contained --configuration Release --output ./publish/$RUNTIME-$VERSION/Server
    # ReleaseFiles
    cp -r ./ReleaseFiles/. ./publish/$RUNTIME-$VERSION/
    # Pack
    tar cjf ./release/$RUNTIME-$VERSION.tar.gz ./publish/$RUNTIME-$VERSION
done 