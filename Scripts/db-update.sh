#!/bin/bash

dotnet ef database update --startup-project './Code/Server/Src.Server.Web' --project './Code/Data/Src.Data.ClassLib'