cd ../../src/Services/

docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" --env-file=".env" down
