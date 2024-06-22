FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /sol
EXPOSE 80

# ---------------------------------------------
COPY BikeShop.sln BikeShop.sln
COPY src/Srv_Sale/Srv_Sale.csproj src/Srv_Sale/Srv_Sale.csproj
COPY src/Srv_Id/Srv_Id.csproj src/Srv_Id/Srv_Id.csproj
COPY src/SaleFinder/SaleFinder.csproj src/SaleFinder/SaleFinder.csproj
COPY src/Gateway/Gateway.csproj src/Gateway/Gateway.csproj
COPY src/Transit/Transit.csproj src/Transit/Transit.csproj

# ---------------------------------------------
RUN dotnet restore BikeShop.sln

COPY src/Gateway src/Gateway
WORKDIR /sol/src/Gateway
RUN dotnet publish -c Release -o /sol/src/out

# ---------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /sol
COPY --from=build /sol/src/out .
ENTRYPOINT [ "dotnet", "Gateway.dll" ]