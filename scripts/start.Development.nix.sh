#!/bin/bash

docker-compose -f "docker-compose.Development.yml" --env-file="Development.env" up -d --build