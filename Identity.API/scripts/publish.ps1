param(
    [string] $ImageTag = "latest"
)

docker push "eszopregistry.azurecr.io/eszop-identity-api:$ImageTag"