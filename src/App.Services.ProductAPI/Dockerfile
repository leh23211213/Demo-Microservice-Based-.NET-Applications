
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV SimpleProperty="BASE-dockerfile"

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["App.Services.ProductAPI.csproj", "./"]
RUN dotnet restore "App.Services.ProductAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "App.Services.ProductAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.Services.ProductAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app        
COPY --from=publish /app/publish .  
ENV SimpleProperty="FINAL-dockerfile"
ENTRYPOINT ["dotnet", "App.Services.ProductAPI.dll"]

