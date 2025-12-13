#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
COMPOSE_FILE="${COMPOSE_FILE:-$ROOT_DIR/src/docker-compose.yml}"
COMPOSE="docker compose -f $COMPOSE_FILE"
DOMAIN="${1:-${LETSENCRYPT_DOMAIN:-localhost}}"

echo "Checking public endpoint https://$DOMAIN"
curl -k --silent --output /dev/null "https://$DOMAIN" || {
  echo "Public HTTPS check failed" >&2
  exit 1
}

echo "Checking internal Identity HTTPS endpoint"
$COMPOSE exec -T nginx /bin/sh -c "\
  if ! command -v curl >/dev/null 2>&1; then \
    apt-get update >/dev/null && apt-get install -y curl >/dev/null; \
  fi && \
  curl -k --silent --output /dev/null https://identity:5001/\
" && echo "Identity OK"

echo "Checking internal Business HTTPS endpoint"
$COMPOSE exec -T nginx /bin/sh -c "\
  if ! command -v curl >/dev/null 2>&1; then \
    apt-get update >/dev/null && apt-get install -y curl >/dev/null; \
  fi && \
  curl -k --silent --output /dev/null https://business:8082/\
" && echo "Business OK"

echo "All TLS checks passed"
