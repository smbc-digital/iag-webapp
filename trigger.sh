#!/bin/bash
set -e

ensure_arg_not_empty() {
  name=$1; value=$2

  if [[ -z "$value" ]]; then
    echo "Argument '$name' is missing"
    exit 1
  fi
}

repository=$1
base_url=https://api.snap-ci.com/project/smbc-digital

ensure_arg_not_empty "API_KEY" $API_KEY
ensure_arg_not_empty "CI_USER" $CI_USER
ensure_arg_not_empty "repository" $repository

curl -u $CI_USER:$API_KEY \
-X POST \
-H 'Accept: application/vnd.snap-ci.com.v1+json' \
-H 'Content-type: application/json' \
$base_url/$repository/branch/master/trigger \
--data '{}'
