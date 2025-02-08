#!/usr/bin/env bash

# Check if GODOT environment variable is set
if [[ -z "$GODOT" ]]; then
  echo "Error: GODOT environment variable not set" >&2
  exit 1
fi

# Check if Godot executable exists
if ! [[ -f "$GODOT" ]]; then
  echo "Error: Godot executable not found at $GODOT" >&2
  exit 1
fi

run_target="--confirma-run"
args=()

# Parse options
opts=$(getopt -o r:m:c:vsedhjg:o:p: --long run:,method:,category:,verbose,sequential,exit,disable-orphans,disable-cs,disable-gd,gd-path:,output:,output-path: -- "$@")

if [ $? -ne 0 ]; then
  exit 1
fi

eval set -- "$opts"

while true; do
  case "$1" in
    -r|--run)             run_target="--confirma-run=$2"; shift 2;;
    -m|--method)          args+=("--confirma-method=$2"); shift 2;;
    -c|--category)        args+=("--confirma-category=$2"); shift 2;;
    -v|--verbose)         args+=("--confirma-verbose"); shift;;
    -s|--sequential)      args+=("--confirma-sequential"); shift;;
    -e|--exit)            args+=("--confirma-exit-on-failure"); shift;;
    -d|--disable-orphans) args+=("--confirma-disable-orphans-monitor"); shift;;
    -h|--disable-cs)      args+=("--confirma-disable-cs"); shift;;
    -j|--disable-gd)      args+=("--confirma-disable-gd"); shift;;
    -g|--gd-path)         args+=("--confirma-gd-path=$2"); shift 2;;
    -o|--output)          args+=("--confirma-output=$2"); shift 2;;
    -p|--output-path)     args+=("--confirma-output-path=$2"); shift 2;;
    --)                   shift; break;;
  esac
done

# Add remaining arguments
args+=("$@")

$GODOT ./ --headless -- "$run_target" "${args[@]}"
