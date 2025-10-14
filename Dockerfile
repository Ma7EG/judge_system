# Use the official .NET 8 SDK image as the build environment.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the project file and restore dependencies.
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the source code and build the application.
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image for the final, smaller image.
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port the app runs on.
EXPOSE 8080

# The command to run the application.
ENTRYPOINT ["dotnet", "CPJS.dll"]