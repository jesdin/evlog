## Setting up MongoDB 

### Pull the latest mongo image:

```bash
docker pull mongo
```

### Create a new container

(Adjust the local port and container name as needed.)

```bash
docker run --name mongo \
    -e MONGO_INITDB_ROOT_USERNAME=root \
    -e MONGO_INITDB_ROOT_PASSWORD=amDbDZ3v \
    -p 27017:27017 \
    -d mongo
```

The newly created `mongo` container should be running. You may verify it using `docker ps`. You may use `docker start mongo` start the container if it's not already running.

### Seed some data

Run the seed script from the project root using:

```bash
mongo -u root -p amDbDZ3v localhost/admin ./scripts/seed.js
```