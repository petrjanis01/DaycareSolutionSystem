FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["DaycareSolutionSystem.Database.Migrator/DaycareSolutionSystem.Database.Migrator.csproj", "DaycareSolutionSystem.Database.Migrator/"]
COPY ["DaycareSolutionSystem.DataContext/DaycareSolutionSystem.Database.DataContext.csproj", "DaycareSolutionSystem.DataContext/"]
COPY ["DaycareSolutionSystem.Entities/DaycareSolutionSystem.Database.Entities.csproj", "DaycareSolutionSystem.Entities/"]
COPY ["DaycareSolutionSystem.Services/DaycareSolutionSystem.Helpers.csproj", "DaycareSolutionSystem.Services/"]
RUN dotnet restore "DaycareSolutionSystem.Database.Migrator/DaycareSolutionSystem.Database.Migrator.csproj"
COPY . .
WORKDIR "/src/DaycareSolutionSystem.Database.Migrator"
RUN dotnet build "DaycareSolutionSystem.Database.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaycareSolutionSystem.Database.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaycareSolutionSystem.Database.Migrator.dll"]