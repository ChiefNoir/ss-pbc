FROM node:16-alpine as builder

# install and cache app dependencies
COPY package.json package-lock.json ./
RUN npm install --only=prod&& mkdir /app && mv ./node_modules ./app

WORKDIR /app

COPY . .

RUN npm run build