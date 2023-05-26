# SleekFlow


## Local Development

#### Required

- Visual Studio 2022
- Docker Desktop or MongoDB Server
- Any MongoDB Client (Robo3T, Compass and etc)

#### How to start

**NOTE:** Skip the rest if using MongoDB Server locally (Not docker)

- Run Docker Desktop
- Go the working folder **mapshare-backend/src**
- Run the following command in Git Bash with current directory as above
> sh start-docker.sh
- This will pickup docker-compose.yml and start a MongoDB instance in your local machine, it will take some times
- Add the following to **C:\Windows\System32\drivers\etc\hosts**, this is required to use connection string for replicaset
> 127.0.0.1 mapsharemongo
- Open Visual Studio 2022 and start the development

**NOTE:** Use the Connection Information below for MongoDB Server in Docker container

> ##### Connection information
> - Server: mapsharemongo
> - Port: 27017
> - username: user
> - password: password
> - Authentication Mode: Basic(SCRAM-SHA-1)
> - AuthenticationDB: mapshare


**NOTE:** Default username and password are set in **mapshare-backend/src/init-local-mongo.js**

**NOTE:** Please use the default username and password for MongoDB
