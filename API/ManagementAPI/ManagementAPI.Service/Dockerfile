FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["ManagementAPI.Service/NuGet.Config", "."]
COPY ["ManagementAPI.Service/ManagementAPI.Service.csproj", "ManagementAPI.Service/"]
COPY ["ManagementAPI.TournamentAggregate/ManagementAPI.TournamentAggregate.csproj", "ManagementAPI.TournamentAggregate/"]
COPY ["ManagementAPI.Tournament.DomainEvents/ManagementAPI.Tournament.DomainEvents.csproj", "ManagementAPI.Tournament.DomainEvents/"]
COPY ["ManagementAPI.ClubConfigurationAggregate/ManagementAPI.ClubConfigurationAggregate.csproj", "ManagementAPI.ClubConfigurationAggregate/"]
COPY ["ManagementAPI.ClubConfiguration.DomainEvents/ManagementAPI.ClubConfiguration.DomainEvents.csproj", "ManagementAPI.ClubConfiguration.DomainEvents/"]
RUN dotnet restore "ManagementAPI.Service/ManagementAPI.Service.csproj"
COPY . .
WORKDIR "/src/ManagementAPI.Service"
RUN dotnet build "ManagementAPI.Service.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ManagementAPI.Service.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ManagementAPI.Service.dll"]