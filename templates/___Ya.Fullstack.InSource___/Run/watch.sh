#!/bin/sh
(cd Client && npm run dev) &
(cd Server && dotnet watch run)
