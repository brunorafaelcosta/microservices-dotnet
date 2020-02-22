# microservices-dotnet

docker-compose:
    - conditional services: https://github.com/docker/compose/issues/1294

ELK:
    elasticsearch:
        - delete all indexes: curl -X DELETE 'http://localhost:9200/_all'
        
    - https://www.humankode.com/asp-net-core/logging-with-elasticsearch-kibana-asp-net-core-and-docker
    - https://github.com/revocengiz/Elk/tree/master/Sample/HttpSinkWebApp
    - https://github.com/revocengiz/Elk/blob/master/Elk/logstash/Dockerfile