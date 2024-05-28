#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TransportTicketApp.API/TransportTicketApp.API.csproj", "TransportTicketApp.API/"]
COPY ["TransportTicketApp.Data.Mongo/TransportTicketApp.Data.Mongo.csproj", "TransportTicketApp.Data.Mongo/"]
COPY ["TransportTicketApp.Data/TransportTicketApp.Data.csproj", "TransportTicketApp.Data/"]
COPY ["TransportTicketApp.Entities/TransportTicketApp.Entities.csproj", "TransportTicketApp.Entities/"]
RUN dotnet restore "./TransportTicketApp.API/TransportTicketApp.API.csproj"
COPY . .
WORKDIR "/src/TransportTicketApp.API"
RUN dotnet build "./TransportTicketApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TransportTicketApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransportTicketApp.API.dll"]