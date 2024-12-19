# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Ensure the correct project path
COPY ["GrpcServer/GrpcServer.csproj", "GrpcServer/"]
RUN dotnet restore "GrpcServer/GrpcServer.csproj"

# Copy the remaining project files
COPY . .

# Build the project
WORKDIR "/src/GrpcServer"
RUN dotnet build "GrpcServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GrpcServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

# Copy the published app to the final image
COPY --from=publish /app/publish .

# Set the entrypoint for the container
ENTRYPOINT ["dotnet", "GrpcServer.dll"]
