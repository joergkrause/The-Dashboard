#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TheDashboard.DataConsumerService/TheDashboard.DataConsumerService.csproj", "TheDashboard.DataConsumerService/"]
COPY ["TheDashboard.BuildingBlocks/TheDashboard.BuildingBlocks.csproj", "TheDashboard.BuildingBlocks/"]
COPY ["TheDashboard.DatabaseLayer/TheDashboard.DatabaseLayer.csproj", "TheDashboard.DatabaseLayer/"]
RUN dotnet restore "TheDashboard.DataConsumerService/TheDashboard.DataConsumerService.csproj"
COPY . .
WORKDIR "/src/TheDashboard.DataConsumerService"
RUN dotnet build "TheDashboard.DataConsumerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheDashboard.DataConsumerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheDashboard.DataConsumerService.dll"]