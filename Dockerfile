FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

RUN apt update && apt install -y libfreetype6 libfontconfig1 fontconfig

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Jazer.Server/Jazer.Server.csproj", "Jazer.Server/"]
RUN dotnet restore "Jazer.Server/Jazer.Server.csproj"
COPY src/ .
WORKDIR "/src/Jazer.Server"
RUN dotnet build "Jazer.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Jazer.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jazer.Server.dll"]