FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /App
COPY . ./

# Runtime can be e.g. linux-x64 or linux-arm64, see officially supported Runtime Identifiers
ARG RUNTIME
RUN dotnet publish Arrowgene.Ddon.Cli /p:Version=1.0.0.0 -p:PublishReadyToRun=true /p:DebugType=None /p:DebugSymbols=false -r ${RUNTIME:-linux-x64} --self-contained false -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
#RUN apt-get update && apt-get install -y apt-transport-https && rm -rf /var/lib/apt/lists/*

# Database
EXPOSE 3306/tcp
# Game server
EXPOSE 52000/tcp
# Web server
EXPOSE 52099/tcp
# Login server
EXPOSE 52100/tcp
ENV DOTNET_EnableDiagnostics=0

WORKDIR /var/ddon/server
COPY --from=build-env /App/out .
RUN adduser --disabled-password --gecos "" ddon_server
RUN chown -R ddon_server:ddon_server .
USER ddon_server

CMD ["/var/ddon/server/Arrowgene.Ddon.Cli", "server", "start"]
