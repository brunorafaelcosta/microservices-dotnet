#!bin/sh

# if [[ -z "${PROJECT_ROOT_FOLDER}" ]]; then
#     echo "Must pass the project root folder to the container."
#     exit 1
# else
#     if [[ -z "${DOMAIN_PASSWORD}" || -z ${DOMAIN_PID} ]]; then
#         echo "DOMAIN_PID and DOMAIN_PASSWORD must be injected into the container."
#         exit 1
#     fi
# fi

cd workspace/${PROJECT_ROOT_FOLDER}

dotnet watch run -c debug --no-launch-profile

exec "$@"
