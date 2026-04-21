#!/bin/bash
docker build -t webmaster442/engineeringcalc .
docker run -it -e TERM=xterm-256color -e LANG=C.UTF-8 webmaster442/engineeringcalc:latest