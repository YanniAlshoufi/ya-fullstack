#!/bin/bash

[[ $# -lt 1 || $# -gt 4 ]] \
&& echo "Usage: $0 <model-name-singular> <model-name-plural> <variable-singular> <variable-plural>"\
&& exit 1

inp_singular=$1

make_camelcase() {
  echo "$1" | gsed 's/^[A-Z]/\L&/'
}

[ $# -lt 2 ] && inp_plural="$inp_singular"s || inp_plural=$2
[ $# -lt 3 ] && inp_var_singular="$(make_camelcase "$inp_singular")" || inp_var_singular=$4
[ $# -lt 4 ] && inp_var_plural="$(make_camelcase "$inp_plural")" || inp_var_plural=$5

replace_all_from_to() {
  sed \
   -e "s/{{PLURAL}}/$inp_plural/g" \
   -e "s/{{SINGULAR}}/$inp_singular/g" \
   -e "s/{{SINGULARVAR}}/$inp_var_singular/g" \
   -e "s/{{PLURALVAR}}/$inp_var_plural/g" \
   "$1" > "$2"
}

replace_all_from_to "./Templates/Services/I{{PLURAL}}Service.cs" "./Code/Shared/Src.Shared.ClassLib/ServicesInterfaces/I${inp_plural}Service.cs"
replace_all_from_to "./Templates/Services/Server{{PLURAL}}Service.cs" "./Code/Server/Src.Server.Web/Services/${inp_plural}Service.cs"
replace_all_from_to "./Templates/Services/Client{{PLURAL}}Service.cs" "./Code/Client/Src.Client.BlazorWasm/Services/${inp_plural}Service.cs"

add_service_dependency_injection_to() {
  tmpfile=$(mktemp)
  sed '/\/\/ Auto-added services, DO NOT REMOVE THIS LINE/a\
        builder.Services.AddScoped<I'"$inp_plural"'Service, '"$inp_plural"'Service>();\
'\
  "$1" > "${tmpfile}"
  cat "${tmpfile}" > "$1"
  /bin/rm -f "${tmpfile}"
}

add_service_dependency_injection_to "./Code/Server/Src.Server.Web/Helpers/ServiceGeneration.cs"
add_service_dependency_injection_to "./Code/Client/Src.Client.BlazorWasm/Helpers/ServiceGeneration.cs"
