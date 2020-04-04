FROM node:13.12.0 AS dev

# Project configuration
ENV PROJECT_ROOT_FOLDER "projects/base-admin"
ENV PROJECT_NAME "base-admin"
# ENV PROJECT_BASE_URL "/admin/"
ENV PROJECT_HTTP_PORT "4200"

# Install envsubst
RUN apt-get update && apt-get install -y gettext

# Ports listening
EXPOSE ${PROJECT_HTTP_PORT}

# Set workspace dir
WORKDIR /workspace

# Set node_modules path
ENV PATH /workspace/node_modules/.bin:$PATH

# Project build
COPY package.json package-lock.json /workspace/
RUN npm config set -g production false
RUN npm install
RUN npm install -g @angular/cli@9.0.4

# Copy workspace contents
COPY . .

# Project bootstrap
RUN cp Development.start.sh /Development.start.sh
WORKDIR /
RUN sed -i -e 's/\r$//' /Development.start.sh && chmod +x /Development.start.sh
ENTRYPOINT ["/Development.start.sh"]
