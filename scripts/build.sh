#!/bin/bash

# This script builds docker images for the application.
# NOTE: The script must be run from the project root directory.


docker build \
    -t gldraphael/evlog \
    -f ./src/Evlog.Web/Dockerfile .

docker build \
    -t gldraphael/evlog-self-contained \
    -f ./src/Evlog.Web/Dockerfile \
    --target self-contained .
