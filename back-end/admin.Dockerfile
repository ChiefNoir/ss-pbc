FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SSPBC.Admin/SSPBC.Admin.csproj", "SSPBC.Admin/"]
COPY ["Security/Security.csproj", "Security/"]
COPY ["Abstractions/Abstractions.csproj", "Abstractions/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Infrastructure.Cache/Infrastructure.Cache.csproj", "Infrastructure.Cache/"]
RUN dotnet restore "SSPBC.Admin/SSPBC.Admin.csproj"
COPY . .
WORKDIR "/src/SSPBC.Admin"
RUN dotnet build "SSPBC.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SSPBC.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SSPBC.Admin.dll"]