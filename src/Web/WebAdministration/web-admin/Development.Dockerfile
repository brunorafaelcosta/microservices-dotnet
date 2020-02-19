FROM node:13.8.0-alpine AS dev

# Project configuration
ENV PROJECT_ROOT_FOLDER "WebAdministration/web-admin"
ENV PROJECT_HTTP_PORT "3000"

# VSCode Remote Debugger
RUN apk update
RUN apk --no-cache add procps
# uncomment if vsdbg volume not mounted 
# RUN apt-get install curl -y
# RUN apt-get install unzip -y
# RUN curl -sSL https://aka.ms/getvsdbgsh | sh /dev/stdin -v latest -l /vsdbg

# Ports listening
EXPOSE ${PROJECT_HTTP_PORT}

# Project build
RUN mkdir -p /workspace/${PROJECT_ROOT_FOLDER}
WORKDIR /workspace/${PROJECT_ROOT_FOLDER}
COPY ${PROJECT_ROOT_FOLDER}/package.json ./
# COPY ${PROJECT_ROOT_FOLDER}/package-lock.json ./
RUN npm install --silent

# Copy workspace contents
WORKDIR /workspace
COPY . .

# NPM links - project dependencies
RUN cd "/workspace/web-common" && npm link
RUN cd "/workspace/WebAdministration/web-admin-common" && npm link
RUN cd "/workspace/${PROJECT_ROOT_FOLDER}" && npm link web-common && npm link web-admin-common

# Project bootstrap
WORKDIR /
RUN cp /workspace/${PROJECT_ROOT_FOLDER}/start.Development.sh /start.Development.sh
RUN sed -i -e 's/\r$//' /start.Development.sh && chmod +x /start.Development.sh
ENTRYPOINT ["/start.Development.sh"]
