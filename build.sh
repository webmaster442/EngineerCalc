#!/bin/bash

dotnet restore
dotnet build --no-restore -c Release --self-contained true -r linux-x64 -o ./publish