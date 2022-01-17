FROM node:16 AS ui-build
WORKDIR /usr/src/app
COPY . .

RUN npm install @angular/cli
RUN npm install
RUN npm run build

FROM nginx:alpine

# Set working directory to nginx asset directory
WORKDIR /usr/share/nginx/html
#Remove default nginx static assets
RUN rm -rf ./*

# Copy static assets from builder stage
COPY --from=ui-build /usr/src/app/transact-boha/ .
COPY --from=ui-build /usr/src/app/nginx-custom.conf /etc/nginx/conf.d/default.conf

EXPOSE 80 443 4200

ENTRYPOINT ["nginx", "-g", "daemon off;"]