FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# copy everything and restore specific project as distinct layers
COPY ./DaycareSolutionSystem/. .
RUN dotnet restore ./DaycareSolutionSystem.Api.Host/DaycareSolutionSystem.Api.Host.csproj

# copy everything else and build app
COPY . .
WORKDIR /app
RUN dotnet publish ./DaycareSolutionSystem.Api.Host/DaycareSolutionSystem.Api.Host.csproj -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app/
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "DaycareSolutionSystem.Api.Host.dll"]