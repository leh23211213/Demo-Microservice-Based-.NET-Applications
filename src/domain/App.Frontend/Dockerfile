FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV SimpleProperty="BASE-dockerfile"

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["App.Frontend.csproj", "./"]
RUN dotnet restore "App.Frontend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "App.Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.Frontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app        
COPY --from=publish /app/publish .  
ENV SimpleProperty="FINAL-dockerfile"
ENTRYPOINT ["dotnet", "App.Frontend.dll"]

