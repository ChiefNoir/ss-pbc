FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SSPBC/SSPBC.csproj", "SSPBC/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Security/Security.csproj", "Security/"]
COPY ["Abstractions/Abstractions.csproj", "Abstractions/"]
COPY ["Infrastructure.Cache/Infrastructure.Cache.csproj", "Infrastructure.Cache/"]
RUN dotnet restore "SSPBC/SSPBC.csproj"
COPY . .
WORKDIR "/src/SSPBC"
RUN dotnet build "SSPBC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SSPBC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SSPBC.dll"]