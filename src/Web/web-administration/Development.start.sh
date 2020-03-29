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

cd workspace/

ng build shared
ng serve --project=${PROJECT_NAME} --host 0.0.0.0 #--base-href ${PROJECT_BASE_URL}

exec "$@"