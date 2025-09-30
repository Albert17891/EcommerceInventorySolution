# See https://aka.ms/customizecontainer to learn how to customize your debug container
# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all project files
COPY ["EcommerceInventory.Api/EcommerceInventory.Api.csproj", "EcommerceInventory.Api/"]
COPY ["EcommerceInventory.Application/EcommerceInventory.Application.csproj", "EcommerceInventory.Application/"]
COPY ["EcommerceInventory.Domain/EcommerceInventory.Domain.csproj", "EcommerceInventory.Domain/"]
COPY ["EcommerceInventory.Infrastructure/EcommerceInventory.Infrastructure.csproj", "EcommerceInventory.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "EcommerceInventory.Api/EcommerceInventory.Api.csproj"

# Copy all source files
COPY . .

# Build
WORKDIR "/src/EcommerceInventory.Api"
RUN dotnet build "EcommerceInventory.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EcommerceInventory.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install EF Core tools for migrations
USER root
RUN dotnet tool install --global dotnet-ef --version 8.0.*
ENV PATH="${PATH}:/root/.dotnet/tools"
USER $APP_UID

ENTRYPOINT ["dotnet", "EcommerceInventory.Api.dll"]