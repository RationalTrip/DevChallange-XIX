#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TrustNetwork.WebAPI/TrustNetwork.WebAPI.csproj", "src/TrustNetwork.WebAPI/"]
RUN dotnet restore "src/TrustNetwork.WebAPI/TrustNetwork.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/TrustNetwork.WebAPI"
RUN dotnet build "TrustNetwork.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrustNetwork.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrustNetwork.WebAPI.dll"]