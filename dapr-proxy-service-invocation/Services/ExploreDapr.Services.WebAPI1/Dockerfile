#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/ExploreDapr.Services.WebAPI1/ExploreDapr.Services.WebAPI1.csproj", "Services/ExploreDapr.Services.WebAPI1/"]
RUN dotnet restore "Services/ExploreDapr.Services.WebAPI1/ExploreDapr.Services.WebAPI1.csproj"
COPY . .
WORKDIR "/src/Services/ExploreDapr.Services.WebAPI1"
RUN dotnet build "ExploreDapr.Services.WebAPI1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExploreDapr.Services.WebAPI1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExploreDapr.Services.WebAPI1.dll"]