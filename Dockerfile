# --- Build stage: compiles the app using the full .NET SDK image ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy just the project file first so Docker can cache the restore step
# (only re-runs restore when the .csproj actually changes)
COPY DockerDemo.csproj ./
RUN dotnet restore

# Now copy the rest of the source and publish a release build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# --- Runtime stage: much smaller image, no SDK, just what's needed to run ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "DockerDemo.dll"]
