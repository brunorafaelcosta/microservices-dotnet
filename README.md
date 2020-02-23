# microservices-dotnet

docker-compose:
    - conditional services: https://github.com/docker/compose/issues/1294

ELK:
    - Disabled in Development Environment for faster startup
    - elasticsearch:
        - delete all indexes: curl -X DELETE 'http://localhost:9200/_all'
