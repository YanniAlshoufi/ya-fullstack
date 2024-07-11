dotnet watch run --project Server --launch-profile https --non-interactive &
dotnet watch run --project Client --launch-profile https --non-interactive &
(cd Client; npx tailwindcss -i ./Styles/app.css -o ./wwwroot/css/app.css --watch)
