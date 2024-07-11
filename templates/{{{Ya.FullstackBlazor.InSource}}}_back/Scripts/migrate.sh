[ $# -lt 1 ] && echo 'Please provide the name of the migration!' && exit;
[ $# -gt 1 ] && echo 'Please provide only one migration name!' && exit;

dotnet ef migrations add "$1" --startup-project Server --project Data