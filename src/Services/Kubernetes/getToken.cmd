kubectl create serviceaccount dashboard-admin-sa

kubectl create clusterrolebinding dashboard-admin-sa --clusterrole=cluster-admin --serviceaccount=default:dashboard-admin-sa

kubectl get secrets

start cmd.exe

pause > "Next Command kubectl describe secret dashboard-admin-sa-token-XXXXX"


# GET TOKEN COMMAND - kubectl describe secret dashboard-admin-sa-token-XXXXX
