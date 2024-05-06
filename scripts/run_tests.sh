#!/usr/bin/bash

if [ -z "$GODOT" ]; then
  echo "GODOT environment variable not set"
  exit 1
else
  echo "$GODOT"
fi

if ! [ -f "$GODOT" ]; then
  echo "Godot executable not found at $GODOT"
  exit 1
fi

$GODOT ./ --headless -- --confirma-run --confirma-quit "$@"
