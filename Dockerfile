# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
# should be done for each project.
COPY ComputerGraphics/*.csproj ./ComputerGraphics/ 
RUN dotnet restore

# copy everything else and build app
COPY ComputerGraphics/. ./ComputerGraphics/
WORKDIR /source/ComputerGraphics
RUN dotnet publish -r win-x64 -o /app --self-contained