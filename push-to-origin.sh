#!/bin/bash

if [ -z "$1" ]; then
  echo "Usage: $0 \"commit message\" <branch>"
  exit 1
fi

if [ -z "$2" ]; then
  echo "Error: No branch specified."
  echo "Usage: $0 \"commit message\" <branch>"
  exit 1
fi

msg="$1"
branch="$2"

git add .
git commit -m "$msg"
git push origin "$branch"

