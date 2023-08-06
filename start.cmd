@echo off
docker compose --profile all up --build
start "" "https://localhost:7500"
