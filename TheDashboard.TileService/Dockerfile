#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TheDashboard.TileService/TheDashboard.TileService.csproj", "TheDashboard.TileService/"]
COPY ["TheDashboard.BuildingBlocks/TheDashboard.BuildingBlocks.csproj", "TheDashboard.BuildingBlocks/"]
COPY ["TheDashboard.DatabaseLayer/TheDashboard.DatabaseLayer.csproj", "TheDashboard.DatabaseLayer/"]
COPY ["TheDashboard.SharedEntities/TheDashboard.SharedEntities.csproj", "TheDashboard.SharedEntities/"]
RUN dotnet restore "TheDashboard.TileService/TheDashboard.TileService.csproj"
COPY . .
WORKDIR "/src/TheDashboard.TileService"
RUN dotnet build "TheDashboard.TileService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheDashboard.TileService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheDashboard.TileService.dll"]