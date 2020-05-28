FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# copy everything and run restore for all projects in solution
COPY ./src/. .
RUN dotnet restore

# copy everything and build app
COPY . .
WORKDIR /app
RUN dotnet publish ./DaycareSolutionSystem.Api.Host/DaycareSolutionSystem.Api.Host.csproj -c Release -o out

# run unit tests
FROM build as test
WORKDIR /app/DaycareSolutionSystem.Api.Host.Test
RUN dotnet test

# copy builded app into runtime image and set it as entry point
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app/
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "DaycareSolutionSystem.Api.Host.dll"]