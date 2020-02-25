# microservices-dotnet

VSCode:

    Debug docker containers:
        Add the following to .vscode/launch.json
            "configurations": [
                {
                    "name": "Docker Attach - {CONTAINER_NAME}",
                    "type": "coreclr",
                    "request": "attach",
                    "processId": "${command:pickRemoteProcess}",
                    // "processName": "{CONTAINER_NAME}",
                    "sourceFileMap": {
                        "/workspace": "${workspaceFolder}/src"
                    },
                    "pipeTransport": {
                        "pipeProgram": "docker",
                        "pipeArgs": [ "exec", "-i", "{CONTAINER_NAME}" ],
                        "debuggerPath": "/vsdbg/vsdbg",
                        "pipeCwd": "${workspaceRoot}",
                        "quoteArgs": false
                    }
                }
            ]

docker-compose:
    - conditional services: https://github.com/docker/compose/issues/1294

ELK:
    - Disabled in Development Environment for faster startup
    - elasticsearch:
        - delete all indexes: curl -X DELETE 'http://localhost:9200/_all'

Authentication:
    - https://github.com/benscabbia/identityserver4-dockersample-dotnetcore-nginx
    - https://damienbod.com/2017/04/14/asp-net-core-identityserver4-resource-owner-password-flow-with-custom-userrepository
    - https://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity


Architecture:

    Services:
        Identity
            Services.Identity.STS (https://identityserver4.readthedocs.io/en/latest/intro/terminology.html)
    Web:
        shared
            web-common
        Administration
            shared
                web-admininistration-common
            web-administration
            web-tenancy-administration
        Portal
            web-portal
    Mobile
        Portal
            Mobile.Portal.Droid
            Mobile.Portal.iOS