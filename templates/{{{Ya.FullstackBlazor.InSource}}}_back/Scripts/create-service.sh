[[ $# -lt 1 || $# -gt 5 ]] \
&& echo "Usage: $0 <model-name-singular> <model-name-plural> <model-dto> <variable-singular> <variable-plural>"\
&& exit 1

inp_singular=$1

make_camelcase() {
  echo "$1" | gsed 's/^[A-Z]/\L&/'
}

[ $# -lt 2 ] && inp_plural="$inp_singular"s || inp_plural=$2
[ $# -lt 3 ] && inp_dto="$inp_singular"Dto || inp_dto=$3
[ $# -lt 4 ] && inp_var_singular="$(make_camelcase "$inp_singular")" || inp_var_singular=$4
[ $# -lt 5 ] && inp_var_plural="$(make_camelcase "$inp_plural")" || inp_var_plural=$5

replace_all_from_to() {
  sed \
   -e "s/{{PLURAL}}/$inp_plural/g" \
   -e "s/{{SINGULAR}}/$inp_singular/g" \
   -e "s/{{DTO}}/$inp_dto/g" \
   -e "s/{{SINGULARVAR}}/$inp_var_singular/g" \
   -e "s/{{PLURALVAR}}/$inp_var_plural/g" \
   "$1" > "$2"
}

replace_all_from_to "./Templates/Services/I{{PLURAL}}Service.cs" "./Shared/ServicesInterfaces/I${inp_plural}Service.cs"
replace_all_from_to "./Templates/Services/Server{{PLURAL}}Service.cs" "./Server/Services/${inp_plural}Service.cs"