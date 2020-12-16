#Run Command Bellow for setting execution policy
#Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

kubectl proxy

#http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/login