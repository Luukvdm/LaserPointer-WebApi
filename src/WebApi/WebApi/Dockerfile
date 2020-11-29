FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
ENV ASPNETCORE_URLS=https://+:443;http://+:80
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/WebApi/WebApi/WebApi.csproj", "src/WebApi/WebApi/"]
COPY ["src/WebApi/Application/Application.csproj", "src/WebApi/Application/"]
COPY ["src/WebApi/Domain/Domain.csproj", "src/WebApi/Domain/"]
COPY ["src/WebApi/Infrastructure/Infrastructure.csproj", "src/WebApi/Infrastructure/"]
RUN dotnet restore "src/WebApi/WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/src/WebApi/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LaserPointer.WebApi.WebApi.dll"] 

