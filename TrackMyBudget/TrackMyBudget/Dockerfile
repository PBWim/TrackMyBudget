# Stage 1: Build the application - for the build process
# Stage 1 (Build): This stage compiles the application. 
# It restores dependencies, builds the project, and publishes the output to the out directory (Sets the working directory inside the container).
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files and restore any dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application files
COPY . ./

# Build the project and publish it to the out directory (Builds and publishes the application in the Release configuration.)
RUN dotnet publish -c Release -o out

# Stage 2: Build the runtime image - for running the application
# Stage 2 (Runtime): The second stage uses the compiled files from the first stage and prepares 
# the runtime image. This ensures that the final Docker image is smaller because it only contains 
# the compiled application and the ASP.NET Core runtime, not the full SDK.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expose port 80 for the application
# This exposes port 80 inside the container. You can use this to map your application to a port on the host machine.
EXPOSE 80

# Set the entry point for the application
# This command tells Docker to start your application by running ''dotnet TrackMyBudget.dll' when the container is started.
ENTRYPOINT ["dotnet", "TrackMyBudget.dll"]