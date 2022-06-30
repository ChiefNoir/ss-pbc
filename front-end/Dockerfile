# STAGE 1: Build
FROM node:12-alpine AS build
WORKDIR /app
COPY package.json ./
RUN npm install --only=prod
COPY . /app
RUN npm run build

# STAGE 2: add nginx
FROM nginx:stable-alpine
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]