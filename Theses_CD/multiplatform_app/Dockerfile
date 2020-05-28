FROM node:lts-buster-slim AS build

WORKDIR /app
COPY ./src/. .

# build ionic application
WORKDIR /app/DaycareSolutionSystem.MobileApp
RUN npm install
RUN npm install -g @ionic/cli
RUN ionic build --prod 


# run apache and copy builded web app
FROM httpd:2.4
 COPY --from=build /app/www/ /usr/local/apache2/htdocs/
