#!/bin/bash
build() {
  image=$1; tag=$2; dockerfile=$3; use_proxy=$4;

  if [[ $use_proxy = true ]]; then
    echo Running with proxy...
    docker build \
    --build-arg HTTP_PROXY=$HTTP_PROXY \
    --build-arg http_proxy=$HTTP_PROXY \
    --build-arg HTTPS_PROXY=$HTTPS_PROXY \
    --build-arg https_proxy=$HTTPS_PROXY \
    --build-arg NO_PROXY=$NO_PROXY \
    -t $image:$tag --file $dockerfile .
  else
    echo Running without proxy...
    docker build -t $image:$tag --file $dockerfile .
  fi
}

run() {
  container=$1; image=$2; tag=$3; aspnetcore_env=$4; use_proxy=$5;

  rm $container
  if [[ $use_proxy = true ]]; then
    echo Running with proxy...
  	docker run -d --name $container -p 5000:5000 \
  		-e HTTP_PROXY=$HTTP_PROXY \
  		-e HTTPS_PROXY=$HTTPS_PROXY \
  		-e NO_PROXY=$NO_PROXY \
  		-e ASPNETCORE_ENVIRONMENT=$aspnetcore_env $image:$tag
  else
    echo Running without proxy...
    docker run -d --name $container -p 5000:5000 $image:$tag
  fi
}

rm() {
  name=$1
  containers=$(containers_to_remove $name)

  if [[ -n "$containers" ]]; then
    echo "Removing container(s) [$containers]"
    docker rm -f $containers
  else
    echo "No container(s) to remove"
  fi
}

rmi() {
  name=$1
  images=$(images_to_remove $name)

  if [[ -n "$images" ]]; then
    echo "Removing image(s) [$images]"
    docker rmi -f $images
  else
    echo "No image(s) to remove"
  fi
}

containers_to_remove() {
  name=$1
  if [[ $name = "all" ]]; then
    echo $(docker ps -aq)
  else
    echo $(docker ps -aq -f name=$name)
  fi
}

images_to_remove() {
  name=$1
  if [[ $name = "all" ]]; then
    echo $(docker images -aq)
  elif [[ $name = "dangling" ]]; then
    echo $(docker images -aq -f dangling=true)
  else
    echo $name
  fi
}

running_locally() {
  last_arg=$1
  if [[ $last_arg != true ]] && [[ $last_arg != false ]]; then
    echo true
  else
    echo $last_arg
  fi
}

handle_command() {
  validate_dockersetup $(running_locally ${@: -1}) $MACHINE_NAME
  case "$1" in
    build)
      build $2 $3 $4 $5
      ;;
    run)
      run $2 $3 $4 $5 $6
      ;;
    rm)
      rm $2
      ;;
    rmi)
      rmi $2
      ;;
  *)
    echo Invalid Option "'$1'"!
    echo "Available options are: <build/run/rm/rmi>"
    exit 1
  esac
}

validate_dockermachine() {
  machine=$1
  ensure_arg_not_empty "MACHINE_NAME" $machine

  echo "Validating machine '$machine'"
  status=$(docker-machine status $machine)

  if [[ $status = '' ]]; then
    echo "Machine '$machine' does not exist. Creating '$machine'..."
    docker-machine create --driver virtualbox $machine
  elif [[ $status != "Running" ]]; then
    echo "Machine '$machine' is not running, status is '$status'"
    docker-machine start $machine
  fi
  ip=$(docker-machine ip $machine)
  echo "Machine '$machine' is running on host $ip"
  eval $(docker-machine env $machine)
}

validate_dockerdaemon() {
  docker ps -q >/dev/null 2>&1
  if [ $? -ne 0 ]; then
    printf "Error connecting to docker daemon (does docker ps work?)\n"
    exit 1
  fi
}

validate_dockersetup() {
  islocal=$1; machine=$2
  if [[ $islocal = true ]]; then
    validate_dockermachine $machine
  fi
  validate_dockerdaemon
}

ensure_arg_not_empty() {
  name=$1; value=$2

  if [[ -z "$value" ]]; then
    echo "Argument '$name' is missing"
    exit 1
  fi
}

handle_command "$@"
