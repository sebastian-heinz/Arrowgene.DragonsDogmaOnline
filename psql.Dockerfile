FROM mcr.microsoft.com/dotnet/sdk:9.0-noble AS build-env
# Runtime can be e.g. win-x64, linux-x64 or linux-arm64, see officially supported Runtime Identifiers
ARG RUNTIME
ENV DOTNET_EnableDiagnostics=0
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
WORKDIR /App
COPY . ./
RUN dotnet publish Arrowgene.Ddon.Cli -p:Version=1.0.0.0 -p:EnableSdkContainerSupport=true -p:StripSymbols=true -p:DebugSymbols=false -p:DebugType=None -p:PublishReadyToRun=true --runtime ${RUNTIME:-linux-x64} -p:SelfContained=true -p:PublishSingleFile=false -p:TargetFramework=net9.0 -p:PublishTrimmed=false -p:IsAotCompatible=false -p:IsTrimmable=false -p:PublishAot=false -c Release -o out
RUN rm -rf out/Files/Client

# -extra is required due to timezone looks up, e.g. for 'Tokyo Standard Time' in Arrowgene.Ddon.GameServer.StampManager.RelativeSpanToReset(DateTime lastStamp)
FROM mcr.microsoft.com/dotnet/runtime-deps:9.0-noble-chiseled-extra
ENV DOTNET_EnableDiagnostics=0
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

WORKDIR /var/ddon/server
COPY --from=build-env /App/out .
# Required due to dynamic script file compilation
USER root

# Database
EXPOSE 3306/tcp
# Game server
EXPOSE 52000/tcp
# Web server
EXPOSE 52099/tcp
# Login server
EXPOSE 52100/tcp

CMD ["/var/ddon/server/Arrowgene.Ddon.Cli", "server", "start"]
