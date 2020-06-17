FROM node:lts-buster-slim AS build

WORKDIR /app
COPY . .

WORKDIR /app/DaycareSolutionSystem.MobileApp
RUN npm install
RUN npm install -g @ionic/cli
RUN ionic build --prod 

FROM httpd:2.4
COPY --from=build /app/DaycareSolutionSystem.MobileApp/www/ /usr/local/apache2/htdocs/