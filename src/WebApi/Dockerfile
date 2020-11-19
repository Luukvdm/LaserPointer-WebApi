FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
# Copy csproj and restore as distinct layers
# COPY WebApi/*.csproj ./
# RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet restore WebApi/WebApi.csproj
RUN dotnet clean -c Release -o out WebApi/WebApi.csproj
RUN dotnet publish -c Release -o out WebApi/WebApi.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "LaserPointer.WebApi.WebApi.dll"]

