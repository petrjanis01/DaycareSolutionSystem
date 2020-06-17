FROM node:lts-buster-slim AS build

WORKDIR /app
COPY . .

# build angular application
WORKDIR /app/DaycareSolutionSystem.WebApp
RUN npm install
RUN npm install -g @angular/cli@latest
RUN ng build --prod --base-href /manager_app/

# build ionic application
WORKDIR /app/DaycareSolutionSystem.MobileApp
RUN npm install
RUN npm install -g @ionic/cli
RUN ionic build --prod 

# run apache and copy builded web apps 
FROM httpd:2.4
COPY --from=build /app/DaycareSolutionSystem.WebApp/dist/ /usr/local/apache2/htdocs/manager_app/
COPY --from=build /app/DaycareSolutionSystem.MobileApp/www/ /usr/local/apache2/htdocs/