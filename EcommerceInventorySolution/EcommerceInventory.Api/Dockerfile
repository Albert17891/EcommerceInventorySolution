# -------------------------------------------------------------------------
# Stage 1: Base Runtime Image (AS base)
# Used for the final lightweight image.
# -------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Using the non-root user provided by the base image for security
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# -------------------------------------------------------------------------
# Stage 2: Build and Publish (AS publish)
# Renamed from 'build' to 'publish' to match the intended copy source below.
# This stage compiles the code and generates the final deployment artifacts.
# -------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish 
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files first for efficient NuGet restore caching
COPY ["EcommerceInventory.Api/EcommerceInventory.Api.csproj", "EcommerceInventory.Api/"]
COPY ["EcommerceInventory.Application/EcommerceInventory.Application.csproj", "EcommerceInventory.Application/"]
COPY ["EcommerceInventory.Domain/EcommerceInventory.Domain.csproj", "EcommerceInventory.Domain/"]
COPY ["EcommerceInventory.Infrastructure/EcommerceInventory.Infrastructure.csproj", "EcommerceInventory.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "EcommerceInventory.Api/EcommerceInventory.Api.csproj"

# Install EF Core tools (kept from original for potential migration needs)
RUN dotnet tool install --global dotnet-ef --version 8.0.*
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy all remaining source files
COPY . .

# Build and publish the application
WORKDIR "/src/EcommerceInventory.Api"
# Publish directly to /app/publish inside this stage
RUN dotnet publish "EcommerceInventory.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# -------------------------------------------------------------------------
# Stage 3: Final Runtime Image (AS final)
# Creates the final image by copying only the published output from the 
# 'publish' stage into the 'base' runtime environment.
# -------------------------------------------------------------------------
FROM base AS final
WORKDIR /app

# Correctly copy the published output from the 'publish' stage
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "EcommerceInventory.Api.dll"]
