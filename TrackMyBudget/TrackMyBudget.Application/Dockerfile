# Stage 1: Build the application - for the build process
# Stage 1 (Build): This stage compiles the application. 
# It restores dependencies, builds the project, and publishes the output to the out directory (Sets the working directory inside the container).
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution file and restore dependencies
COPY TrackMyBudget.sln ./
COPY TrackMyBudget.Core/TrackMyBudget.Core.csproj ./TrackMyBudget.Core/
COPY TrackMyBudget.Infrastructure/TrackMyBudget.Infrastructure.csproj ./TrackMyBudget.Infrastructure/
COPY TrackMyBudget.Application/TrackMyBudget.Application.csproj ./TrackMyBudget.Application/
COPY TrackMyBudget.Tests/TrackMyBudget.Tests.csproj ./TrackMyBudget.Tests/
RUN dotnet restore

# Copy all project files and build the applicatio
COPY . ./

# Build the project and publish it to the out directory (Builds and publishes the application in the Release configuration.)
WORKDIR /app/TrackMyBudget.Application
RUN dotnet publish -c Release -o /app/out

# Stage 2: Build the runtime image - for running the application
# Stage 2 (Runtime): The second stage uses the compiled files from the first stage and prepares 
# the runtime image. This ensures that the final Docker image is smaller because it only contains 
# the compiled application and the ASP.NET Core runtime, not the full SDK.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app


# Copy the published output from the build stage
COPY --from=build /app/out .

# Expose port 80 for the application
# This exposes port 80 inside the container. You can use this to map your application to a port on the host machine.
EXPOSE 80

# Set the entry point for the application
# This command tells Docker to start your application by running ''dotnet TrackMyBudget.dll' when the container is started.
ENTRYPOINT ["dotnet", "TrackMyBudget.dll"]