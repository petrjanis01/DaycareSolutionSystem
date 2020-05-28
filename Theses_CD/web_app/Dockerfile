FROM node:lts-buster-slim AS build

WORKDIR /app
COPY ./src/. .

# build angular application
WORKDIR /app
RUN npm install
RUN npm install -g @angular/cli@latest
RUN ng build --prod


# run apache and copy builded web app
FROM httpd:2.4
COPY --from=build /app/dist/ /usr/local/apache2/htdocs/
