# Builder stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file
COPY VisitorCounter/VisitorCounter.csproj ./VisitorCounter/
RUN dotnet restore "VisitorCounter/VisitorCounter.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/VisitorCounter"
RUN dotnet build "VisitorCounter.csproj" -c Release -o /app/build

# Publish
RUN dotnet publish "VisitorCounter.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/ || exit 1

# Expose port
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "VisitorCounter.dll", "--urls", "http://0.0.0.0:8080"]