FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS dev

# Project configuration
ENV PROJECT_ROOT_FOLDER "Identity/Services.Identity.API"
ENV PROJECT_HTTP_PORT "8080"

# VSCode Remote Debugger
RUN apt-get update
RUN apt-get install procps -y
# uncomment if vsdbg volume not mounted 
# RUN apt-get install curl -y
# RUN apt-get install unzip -y
# RUN curl -sSL https://aka.ms/getvsdbgsh | sh /dev/stdin -v latest -l /vsdbg

# Copy workspace contents
RUN mkdir /workspace
COPY . ./workspace

# Ports listening
ENV ASPNETCORE_URLS http://*:${PROJECT_HTTP_PORT}
EXPOSE ${PROJECT_HTTP_PORT}

# Project build
WORKDIR /workspace/${PROJECT_ROOT_FOLDER}
RUN dotnet restore
RUN dotnet build -c debug

# Project bootstrap
RUN cp start.Development.sh /start.Development.sh
WORKDIR /
RUN sed -i -e 's/\r$//' /start.Development.sh && chmod +x /start.Development.sh
ENTRYPOINT ["/start.Development.sh"]
