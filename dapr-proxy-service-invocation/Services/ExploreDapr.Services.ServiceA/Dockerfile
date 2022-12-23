#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/ExploreDapr.Services.ServiceA/ExploreDapr.Services.ServiceA.csproj", "Services/ExploreDapr.Services.ServiceA/"]
RUN dotnet restore "Services/ExploreDapr.Services.ServiceA/ExploreDapr.Services.ServiceA.csproj"
COPY . .
WORKDIR "/src/Services/ExploreDapr.Services.ServiceA"
RUN dotnet build "ExploreDapr.Services.ServiceA.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExploreDapr.Services.ServiceA.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExploreDapr.Services.ServiceA.dll"]