FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["DaycareSolutionSystem.Api.Host/DaycareSolutionSystem.Api.Host.csproj", "DaycareSolutionSystem.Api.Host/"]
COPY ["DaycareSolutionSystem.Services/DaycareSolutionSystem.Helpers.csproj", "DaycareSolutionSystem.Services/"]
COPY ["DaycareSolutionSystem.DataContext/DaycareSolutionSystem.Database.DataContext.csproj", "DaycareSolutionSystem.DataContext/"]
COPY ["DaycareSolutionSystem.Entities/DaycareSolutionSystem.Database.Entities.csproj", "DaycareSolutionSystem.Entities/"]
RUN dotnet restore "DaycareSolutionSystem.Api.Host/DaycareSolutionSystem.Api.Host.csproj"
COPY . .
WORKDIR "/src/DaycareSolutionSystem.Api.Host"
RUN dotnet build "DaycareSolutionSystem.Api.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaycareSolutionSystem.Api.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaycareSolutionSystem.Api.Host.dll"]