#!/bin/bash

ya_fullstack_kill_all() {
  ./Scripts/mac/killall.sh
}

trap "ya_fullstack_kill_all" SIGINT

dotnet watch --project 'Code/Server/Src.Server.Web' --launch-profile https --non-interactive &
dotnet watch --project 'Code/Client/Src.Client.BlazorWasm' --launch-profile https --non-interactive & 
(cd 'Code/Client/Src.Client.BlazorWasm'; npx tailwindcss -i ./Styles/app.css -o ./wwwroot/css/app.css --watch)

