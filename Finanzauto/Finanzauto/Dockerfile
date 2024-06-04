#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV DB_CONNECTION_STRING="Server=192.168.1.1,1433; Database=Finanzauto; User ID=sa;Password=12345678;TrustServerCertificate=true;Trusted_Connection=true;"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Finanzauto/Finanzauto.csproj", "Finanzauto/"]
COPY ["Finanzauto.Common/Finanzauto.Common.csproj", "Finanzauto.Common/"]
COPY ["Finanzauto.Infrastructure/Finanzauto.Infrastructure.csproj", "Finanzauto.Infrastructure/"]
COPY ["Finanzauto.Domain/Finanzauto.Domain.csproj", "Finanzauto.Domain/"]
RUN dotnet restore "./Finanzauto/Finanzauto.csproj"
COPY . .
WORKDIR "/src/Finanzauto"
RUN dotnet build "./Finanzauto.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Finanzauto.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Finanzauto.dll"]