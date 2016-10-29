#!/bin/bash

docker build -t dadstorm .
docker run -it --rm dadstorm
