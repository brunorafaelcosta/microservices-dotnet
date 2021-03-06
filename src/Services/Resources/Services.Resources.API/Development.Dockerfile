FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS dev

ARG HTTP_PORT

# Project configuration
ENV PROJECT_ROOT_FOLDER "Resources/Services.Resources.API"
ENV PROJECT_FILE_NAME "Services.Resources.API.csproj"
ENV PROJECT_HTTP_PORT ${HTTP_PORT}

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=1

# Dependencies
RUN apt-get update
RUN apt-get install procps -y
# RUN apt-get install curl -y (uncomment if vsdbg volume not mounted)
# RUN apt-get install unzip -y (uncomment if vsdbg volume not mounted)

# VSCode Remote Debugger (uncomment if vsdbg volume not mounted)
# RUN curl -sSL https://aka.ms/getvsdbgsh | sh /dev/stdin -v latest -l /vsdbg

WORKDIR /workspace

# Projects packages restore
COPY dotnet-restore.Development.Dockerfile.tar .
RUN tar -xvf dotnet-restore.Development.Dockerfile.tar && rm dotnet-restore.Development.Dockerfile.tar
RUN dotnet restore ${PROJECT_ROOT_FOLDER}/${PROJECT_FILE_NAME} -nowarn:msb3202,nu1503

# Copy workspace contents
COPY . .

# Ports listening
ENV ASPNETCORE_URLS http://*:${PROJECT_HTTP_PORT}
EXPOSE ${PROJECT_HTTP_PORT}

# Project build
WORKDIR /workspace/${PROJECT_ROOT_FOLDER}
RUN dotnet build --no-restore -c debug

# Project bootstrap
RUN cp Development.start.sh /Development.start.sh
WORKDIR /
RUN sed -i -e 's/\r$//' /Development.start.sh && chmod +x /Development.start.sh
ENTRYPOINT ["/Development.start.sh"]
