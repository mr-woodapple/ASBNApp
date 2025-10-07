# Use the .NET SDK image to build the Blazor WebAssembly app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .
RUN dotnet restore "ASBNApp.Frontend/ASBNApp.Frontend.csproj"
RUN dotnet publish "ASBNApp.Frontend/ASBNApp.Frontend.csproj" -c Release -o out

# Serve the published application with Nginx
FROM nginx:stable-alpine
WORKDIR /usr/share/nginx/html

# Remove the default Nginx index.html and error pages
RUN rm -rf ./*

# Copy the published Blazor files from the build stage
# Copy a custom Nginx configuration to handle Blazor routing (essential!)
COPY --from=build /app/out/wwwroot .
COPY ASBNApp.Frontend/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]