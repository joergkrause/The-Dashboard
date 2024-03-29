#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TheDashboard.Frontend/TheDashboard.Frontend.csproj", "TheDashboard.Frontend/"]
COPY ["TheDashboard.ViewModels/TheDashboard.ViewModels.csproj", "TheDashboard.ViewModels/"]
COPY ["TheDashboard.BuildingBlocks/TheDashboard.BuildingBlocks.csproj", "TheDashboard.BuildingBlocks/"]
COPY ["TheDashboard.SharedEntities/TheDashboard.SharedEntities.csproj", "TheDashboard.SharedEntities/"]
RUN dotnet restore "TheDashboard.Frontend/TheDashboard.Frontend.csproj"
COPY . .
WORKDIR "/src/TheDashboard.Frontend"
RUN dotnet build "TheDashboard.Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheDashboard.Frontend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheDashboard.Frontend.dll"]