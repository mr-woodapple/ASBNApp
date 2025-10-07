FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .
RUN dotnet restore "ASBNApp.DataAPI/ASBNApp.DataAPI.csproj"
RUN dotnet publish  "ASBNApp.DataAPI/ASBNApp.DataAPI.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ASBNApp.DataAPI.dll"]