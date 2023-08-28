#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TheDashboard.DataSourceService/TheDashboard.DataSourceService.csproj", "TheDashboard.DataSourceService/"]
COPY ["TheDashboard.BuildingBlocks/TheDashboard.BuildingBlocks.csproj", "TheDashboard.BuildingBlocks/"]
COPY ["TheDashboard.DatabaseLayer/TheDashboard.DatabaseLayer.csproj", "TheDashboard.DatabaseLayer/"]
COPY ["TheDashboard.SharedEntities/TheDashboard.SharedEntities.csproj", "TheDashboard.SharedEntities/"]
RUN dotnet restore "TheDashboard.DataSourceService/TheDashboard.DataSourceService.csproj"
COPY . .
WORKDIR "/src/TheDashboard.DataSourceService"
RUN dotnet build "TheDashboard.DataSourceService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheDashboard.DataSourceService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheDashboard.DataSourceService.dll"]