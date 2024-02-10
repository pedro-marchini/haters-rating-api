FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HatersRating.csproj", "."]
RUN dotnet restore "./././HatersRating.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./HatersRating.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HatersRating.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN dotnet tool install --global dotnet-ef --version 7.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "HatersRating.dll"]