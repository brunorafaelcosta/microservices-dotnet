#!/bin/bash

cd $(dirname $0)

cd ./../../src/Services/

docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" -f "docker-compose.linux.override.yml" --env-file=".env" down
