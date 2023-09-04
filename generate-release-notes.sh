#!/bin/bash

# Obtém a tag mais recente
latest_tag=$(git describe --tags --abbrev=0 2>/dev/null || echo "")

if [ -z "$latest_tag" ]; then
  range="HEAD"
else
  range="$latest_tag..HEAD"
fi

# Gera notas de lançamento com base nos commits
release_notes=$(git log --oneline --no-merges --pretty=format:"%h %s (%an)" $range)

echo "$release_notes"
