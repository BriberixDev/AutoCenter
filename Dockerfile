FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AutoCenter.sln .

COPY AutoCenter/AutoCenter.Web.csproj AutoCenter/

RUN dotnet restore

COPY . .

WORKDIR /src/AutoCenter

RUN dotnet publish AutoCenter.Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AutoCenter.Web.dll"]
