# Use the official .NET core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GerminationChamber-Backend.csproj", "./"]
RUN dotnet restore "./GerminationChamber-Backend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "GerminationChamber-Backend.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "GerminationChamber-Backend.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GerminationChamber-Backend.dll"]