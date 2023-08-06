#!/bin/bash
os="$(uname)"
docker compose --profile all up --build
case $os in
    "Linux")
        if grep -qEi "(Microsoft|WSL)" /proc/version &> /dev/null ; then
            # Windows Subsystem for Linux (WSL)
            /mnt/c/Windows/System32/cmd.exe /c start https://localhost:7500
        else
            # Native Linux
            xdg-open https://localhost:7500
        fi
        ;;
    "Darwin")
        # macOS
        open https://localhost:7500
        ;;
    *) 
        # Unknown OS
        echo "Unknown OS: $os"
        ;;
esac
