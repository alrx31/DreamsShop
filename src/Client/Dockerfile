FROM node:20 AS builder

WORKDIR /app
COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build --configuration=production

FROM nginx:alpine AS deliver
VOLUME /output

COPY --from=builder /app/dist/Client/browser /output
