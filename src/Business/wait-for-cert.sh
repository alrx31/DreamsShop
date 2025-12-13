#!/bin/sh
set -e

CERT_PATH="${ASPNETCORE_Kestrel__Certificates__Default__Path:-/etc/letsencrypt/live/app/aspnet.pfx}"
TIMEOUT_SECONDS="${CERT_WAIT_TIMEOUT:-60}"
COUNTER=0

while [ ! -f "$CERT_PATH" ]; do
  if [ "$COUNTER" -ge "$TIMEOUT_SECONDS" ]; then
    echo "Certificate $CERT_PATH not found after $TIMEOUT_SECONDS seconds." >&2
    exit 1
  fi
  echo "Waiting for certificate at $CERT_PATH... ($COUNTER s)"
  COUNTER=$((COUNTER + 1))
  sleep 1
done

exec dotnet Presentation.dll
