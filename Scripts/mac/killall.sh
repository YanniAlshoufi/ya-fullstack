#!/bin/zsh

ya_fullstack_killport() {
  	port=$(sudo lsof -nP | rg LISTEN | rg :$1 | awk '{ print $2 }')
  	echo $port | xargs -I {} kill -9 {}
}

ya_fullstack_killport 4080
ya_fullstack_killport 4443
ya_fullstack_killport 5080
ya_fullstack_killport 5443

ps ax | grep 'dotnet' | grep -v 'grep' | awk '{print $1}' | xargs -I {} kill -9 {}

