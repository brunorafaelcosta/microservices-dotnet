#!/bin/bash

docker-compose -f "docker-compose.Development.yml" -f "docker-compose.Development.override.yml" --env-file="Development.env" up -d --build