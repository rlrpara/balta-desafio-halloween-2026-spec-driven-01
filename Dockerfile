FROM mcr.microsoft.com/dotnet/sdk:10.0@sha256:e70cdb7f80b0348f5cb85f19a8f670fca061f033d57eed12fa003d58b0e06317 AS build
WORKDIR /src

COPY src/PasswordGenerator.slnx src/
COPY src/PasswordGenerator.Domain/PasswordGenerator.Domain.csproj src/PasswordGenerator.Domain/
COPY src/PasswordGenerator.Service/PasswordGenerator.Service.csproj src/PasswordGenerator.Service/
COPY src/PasswordGenerator.Infra.Data/PasswordGenerator.Infra.Data.csproj src/PasswordGenerator.Infra.Data/
COPY src/PasswordGenerator.Infra.Database/PasswordGenerator.Infra.Database.csproj src/PasswordGenerator.Infra.Database/
COPY src/PasswordGenerator.Api/PasswordGenerator.Api.csproj src/PasswordGenerator.Api/
COPY tests/PasswordGenerator.UnitTests/PasswordGenerator.UnitTests.csproj tests/PasswordGenerator.UnitTests/
COPY tests/PasswordGenerator.IntegrationTests/PasswordGenerator.IntegrationTests.csproj tests/PasswordGenerator.IntegrationTests/
RUN dotnet restore src/PasswordGenerator.slnx

COPY src/ src/
RUN dotnet publish src/PasswordGenerator.Api/PasswordGenerator.Api.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    --property:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0@sha256:222759b391a1aaf241166672c8f99b2d4ada452e7b5319f3c6e8f265a37b5ad4 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ConnectionStrings__Passwords="Data Source=/app/data/passwords.db"
EXPOSE 8080
COPY --from=build /app/publish .
RUN mkdir -p /app/data && chown -R 1654:1654 /app
USER 1654
ENTRYPOINT ["dotnet", "PasswordGenerator.Api.dll"]
