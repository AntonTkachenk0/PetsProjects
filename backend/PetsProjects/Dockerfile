FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
EXPOSE 80
COPY . .
RUN dotnet restore timeweb-s3-api.csproj
RUN dotnet publish timeweb-s3-api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "timeweb-s3-api.dll"]