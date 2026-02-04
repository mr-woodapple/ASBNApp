FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY Directory.Packages.props .
COPY src/ASBNApp.DataAPI/ASBNApp.DataAPI.csproj src/ASBNApp.DataAPI/
COPY src/ASBNApp.Models/ASBNApp.Models.csproj src/ASBNApp.Models/

RUN dotnet restore "src/ASBNApp.DataAPI/ASBNApp.DataAPI.csproj"

COPY src/ASBNApp.DataAPI/ src/ASBNApp.DataAPI/
COPY src/ASBNApp.Models/ src/ASBNApp.Models/

RUN dotnet publish "src/ASBNApp.DataAPI/ASBNApp.DataAPI.csproj" -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ASBNApp.DataAPI.dll"]