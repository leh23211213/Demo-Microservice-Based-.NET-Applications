 # See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY App.Frontend.csproj .
RUN dotnet restore 
COPY . .
WORKDIR "/src/App.Frontend"
RUN dotnet build "App.Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.Frontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app        
COPY --from=publish /app/publish .  
ENTRYPOINT ["dotnet", "App.Frontend.dll"]

