#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5443
ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TheDashboard.Proxy/TheDashboard.Proxy.csproj", "TheDashboard.Proxy/"]
RUN dotnet restore "TheDashboard.Proxy/TheDashboard.Proxy.csproj"
COPY . .
WORKDIR "/src/TheDashboard.Proxy"
RUN dotnet build "TheDashboard.Proxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheDashboard.Proxy.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheDashboard.Proxy.dll"]