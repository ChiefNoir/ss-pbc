# Deployment guide

## Environment
### Docker
> https://docs.docker.com/engine/install/

### Docker/Volume
````
docker volume create pbc-data-files
````
Volume for files

````
docker volume create ss-data-redis
````
Volume for Redis

````
docker volume create ss-data-postgres
````
Volume for Postgresql

### PostgreSQL
````
docker run -d --name ss-postgres -e POSTGRES_PASSWORD= --mount source=ss-data-postgres,destination=/var/lib/postgresql/data -p 5432:5432 postgres
````
> POSTGRES_PASSWORD=

Must be specified. 

### Redis
````
docker run -d --name ss-redis -e REDIS_PASSWORD= -v ss-data-redis:/data -p 6379:6379 --restart always redis /bin/sh -c 'redis-server --appendonly yes --requirepass ${REDIS_PASSWORD}'
````
> REDIS_PASSWORD=

Must be specified. 

### Nginx
> https://github.com/nginx-proxy/acme-companion

### Filesystem
Lookup configs (appsettings.json) for services. Create and edit:
````
  /conf/pbc-admin-appsettings.json | for SSPBC
  /conf/pbc-appsettings.json | for SSPBC.Admin
````

## App
### SSPBC.Admin
````
docker run -d -it --name pbc-admin --mount type=bind,source=/conf/pbc-admin-appsettings.json,target=/app/appsettings.json --mount source=ss-pbc-files,destination=/files --env "VIRTUAL_HOST=" --env "VIRTUAL_PORT=80" --env "LETSENCRYPT_HOST=" --env "LETSENCRYPT_EMAIL=" ssproduction/pbc-admin 
````
> VIRTUAL_HOST=

Must be specified. Virtual host for the reverse-proxy.

> LETSENCRYPT_HOST=

Must be specified. Host for the reverse-proxy.

> LETSENCRYPT_EMAIL=

Better to specify.

### SSPBC
````
docker run -d -it --name pbc-public --mount type=bind,source=/conf/pbc-appsettings.json,target=/app/appsettings.json --mount source=ss-pbc-files,destination=/files --env "VIRTUAL_HOST=" --env "VIRTUAL_PORT=80" --env "LETSENCRYPT_HOST=" --env "LETSENCRYPT_EMAIL=" ssproduction/pbc-public 
````
> VIRTUAL_HOST=

Must be specified. Virtual host for the reverse-proxy.

> LETSENCRYPT_HOST=

Must be specified. Host for the reverse-proxy.

> LETSENCRYPT_EMAIL=

Better to specify.

### SSPBC.Front
````
docker run -d -it --name pbc-front --env "VIRTUAL_HOST=" --env "VIRTUAL_PORT=80" --env "LETSENCRYPT_HOST=" --env "LETSENCRYPT_EMAIL=" --env="REACT_APP_API_PUBLIC_ENDPOINT=" --env="REACT_APP_API_PRIVATE_ENDPOINT=" ssproduction/pbc-front 
````
> VIRTUAL_HOST=

Must be specified. Virtual host for the reverse-proxy.

> LETSENCRYPT_HOST=

Must be specified. Host for the reverse-proxy.

> LETSENCRYPT_EMAIL=

Better to specify.

> REACT_APP_API_PUBLIC_ENDPOINT=

Address of the SSPBC.

> REACT_APP_API_PRIVATE_ENDPOINT=

Address of the SSPBC.Admin