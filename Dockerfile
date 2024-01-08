FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /Evergreen.WebAPI
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /Evergreen.WebAPI
COPY --from=build-env /Evergreen.WebAPI/out .
ENTRYPOINT ["dotnet", "Evergreen.WebAPI.dll"]