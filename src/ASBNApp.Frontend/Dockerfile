# Use the .NET SDK image to build the Blazor WebAssembly app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY Directory.Packages.props .
COPY src/ASBNApp.Frontend/ASBNApp.Frontend.csproj src/ASBNApp.Frontend/
COPY src/ASBNApp.Models/ASBNApp.Models.csproj src/ASBNApp.Models/

RUN dotnet restore "src/ASBNApp.Frontend/ASBNApp.Frontend.csproj"

COPY src/ASBNApp.Frontend/ src/ASBNApp.Frontend/
COPY src/ASBNApp.Models/ src/ASBNApp.Models/

RUN dotnet publish "src/ASBNApp.Frontend/ASBNApp.Frontend.csproj" -c Release -o /app/out

# Serve the published application with Nginx
FROM nginx:stable-alpine
WORKDIR /usr/share/nginx/html

# Remove the default Nginx index.html and error pages
RUN rm -rf ./*

# Copy the published Blazor files from the build stage
# Copy a custom Nginx configuration to handle Blazor routing (essential!)
COPY --from=build /app/out/wwwroot .
COPY src/ASBNApp.Frontend/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]