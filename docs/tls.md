# TLS & Certificate Workflow

This project now issues and stores a shared Let's Encrypt certificate that Nginx, Business, and Identity consume through the `letsencrypt-data` volume. The steps below assume you run Docker commands from the repository root.

> **Local fallback**  
> The `cert-init` helper container runs automatically during `docker compose up`. If no ACME certificate is present it generates a short-lived, self-signed certificate and bundles it into `/etc/letsencrypt/live/app/aspnet.pfx` so the stack can boot locally. Replace this temporary cert with a real Let's Encrypt one as soon as possible.

## 1. Configure environment
Create a `.env` file (never commit it) with the variables required by the `certbot` profile (the same values are reused by `cert-init`). If you skip this step while developing locally, the defaults (`localhost` + password `development`) still let the stack start with the self-signed bundle:

```
LETSENCRYPT_DOMAIN=api.example.com
LETSENCRYPT_EMAIL=ops@example.com
LETSENCRYPT_PFX_PASSWORD=strong-password
```

## 2. Request/renew certificates
Run the one-off certbot container whenever you need to issue or renew the certificate:

```
docker compose -f src/docker-compose.yml --profile certbot run --rm certbot
```

The container performs these steps:
- Solves the HTTP-01 challenge through the shared `/var/www/certbot` folder served by Nginx on port 80
- Symlinks the issued certificate to `/etc/letsencrypt/live/app`
- Converts the PEM bundle into `/etc/letsencrypt/live/app/aspnet.pfx` using `LETSENCRYPT_PFX_PASSWORD`

For automated renewals you can schedule (cron/systemd) the following command and restart the dependent services when it succeeds:

```
0 3 * * * docker compose -f /home/alrx/PROGRAM/DreamsShops/src/docker-compose.yml --profile certbot run --rm certbot sh -c "certbot renew && ln -sfn /etc/letsencrypt/live/$LETSENCRYPT_DOMAIN /etc/letsencrypt/live/app && openssl pkcs12 -export -out /etc/letsencrypt/live/app/aspnet.pfx -inkey /etc/letsencrypt/live/app/privkey.pem -in /etc/letsencrypt/live/app/fullchain.pem -passout pass:$LETSENCRYPT_PFX_PASSWORD && docker compose -f /home/alrx/PROGRAM/DreamsShops/src/docker-compose.yml restart nginx business identity"
```

Adjust the absolute path to match your environment.

To regenerate the fallback certificate manually (e.g., after deleting the volume), run:

```
docker compose -f src/docker-compose.yml up cert-init
```

## 3. Validate HTTPS
Use the helper script to ping the external domain (via Nginx) and the internal HTTPS endpoints (via the shared Docker network):

```
./scripts/check-tls.sh api.example.com
```

### Notes
- Certificates persist because the `letsencrypt-data` volume is declared in `docker-compose.yml`
- `cert-init` only creates certificates if the target files are missing, so running the certbot profile automatically replaces the self-signed bundle without further changes
- If you ever change the domain name, re-run the certbot profile to create a new cert and regenerate the PFX before restarting the stack
- The ASP.NET containers read `/etc/letsencrypt/live/app/aspnet.pfx`, while Nginx references the same directory for `fullchain.pem` and `privkey.pem`
