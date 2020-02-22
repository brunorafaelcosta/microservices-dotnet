# microservices-dotnet

docker-compose:
    - conditional services: https://github.com/docker/compose/issues/1294

ELK:
    elasticsearch:
        - delete all indexes: curl -X DELETE 'http://localhost:9200/_all'
