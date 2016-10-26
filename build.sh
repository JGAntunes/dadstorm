#!/bin/bash

docker build -t dadstorm .
docker run -it --rm --name dadstorm dadstorm
