# =================== BUILD

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

WORKDIR /app

COPY . /app
RUN dotnet publish "SpotPG.Frontend/SpotPG.Frontend.csproj" -c Release -o /app/publish


# =================== RUNTIME

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime-env
COPY --from=0 /app/publish /app
WORKDIR /app
ENTRYPOINT ["dotnet", "/app/SpotPG.dll"]