# Devops
Automated workflows for the project.

## back-end_codeql.yml
Runs [CodeQL](https://codeql.github.com/) for the back-end project.

Generally, questionable workflow. 

## back-end_publish_admin.yml
Builds a docker image for the admin-microservice and pushes it to the [DockerHub](https://hub.docker.com/) [repository](https://hub.docker.com/r/ssproduction/pbc-admin).

Right now only release: published will trigger the action.

## back-end_publish_public.yml
Builds a docker image for the admin-microservice and pushes it to the [DockerHub](https://hub.docker.com/) [repository](https://hub.docker.com/r/ssproduction/pbc-public).

Right now only release: published will trigger the action.

## back-end_test.yml
Runs automated tests for the back-end project and publishes coverage to the [coveralls](https://coveralls.io).

Right now there are no alerts for the low coverage.

## front-end_codeql.yml
Runs [CodeQL](https://codeql.github.com/) for the front-end project.

Generally, questionable workflow. 

## front-end_publish.yml
Builds a docker image for the admin-microservice and pushes it to the [DockerHub](https://hub.docker.com/) [repository](https://hub.docker.com/r/ssproduction/pbc-front).

Right now only release: published will trigger the action.
