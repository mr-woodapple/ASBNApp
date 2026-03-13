# Use the .NET SDK image to build the Blazor WebAssembly app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY Directory.Packages.props .
COPY src/ASBNApp.Frontend/ASBNApp.Frontend.csproj src/ASBNApp.Frontend/
COPY src/ASBNApp.Models/ASBNApp.Models.csproj src/ASBNApp.Models/

RUN dotnet restore "src/ASBNApp.Frontend/ASBNApp.Frontend.csproj"

COPY src/ASBNApp.Frontend/ src/ASBNApp.Frontend/
COPY src/ASBNApp.Models/ src/ASBNApp.Models/

RUN dotnet publish "src/ASBNApp.Frontend/ASBNApp.Frontend.csproj" -c Release -o /app

# Serve the published application with Nginx
FROM nginx:stable-alpine
WORKDIR /usr/share/nginx/html

# Copy the published Blazor files from the build stage
# Copy a custom Nginx configuration to handle Blazor routing (essential!)
COPY --from=build /app/wwwroot /usr/share/nginx/html/
COPY src/ASBNApp.Frontend/nginx.conf /etc/nginx/templates/default.conf.template

# Run custom script to set appsettings.json from docker envs
# Copy the shell script
COPY src/ASBNApp.Frontend/wwwroot/set-docker-env-variables.sh /usr/share/nginx/html/
# Set the execution permission
RUN chmod +x /usr/share/nginx/html/set-docker-env-variables.sh

# Execute the script on container startup
EXPOSE 80
CMD /usr/share/nginx/html/set-docker-env-variables.sh && nginx -g "daemon off;"
# CMD nginx -g "daemon off;"
