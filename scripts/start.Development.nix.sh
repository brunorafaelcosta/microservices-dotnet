#!/bin/bash

cd ../src/

docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" -f "docker-compose.linux.override.yml" --env-file=".env" up -d --build
