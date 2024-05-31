# Dockerfile za .NET projekt
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EventInfoService/EventInfoService.csproj", "EventInfoService/"]
RUN dotnet restore "EventInfoService/EventInfoService.csproj"
COPY . .
WORKDIR "/src/EventInfoService"
RUN dotnet build "EventInfoService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventInfoService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventInfoService.dll"]
