	FROM microsoft/dotnet:2.2-sdk AS build
	WORKDIR /src
	COPY ["ManagementAPI.Service/NuGet.Config", "."]
	COPY ["ManagementAPI.Projections/", "ManagementAPI.Projections/"]
	COPY ["ManagementAPI.Service/ManagementAPI.Service.csproj", "ManagementAPI.Service/"]
	COPY ["ManagementAPI.Service/NuGet.Config", "ManagementAPI.Service/"]
	COPY ["ManagementAPI.TournamentAggregate/ManagementAPI.TournamentAggregate.csproj", "ManagementAPI.TournamentAggregate/"]
	COPY ["ManagementAPI.Tournament.DomainEvents/ManagementAPI.Tournament.DomainEvents.csproj", "ManagementAPI.Tournament.DomainEvents/"]
	COPY ["ManagementAPI.GolfClubAggregate/ManagementAPI.GolfClubAggregate.csproj", "ManagementAPI.GolfClubAggregate/"]
	COPY ["ManagementAPI.GolfClub.DomainEvents/ManagementAPI.GolfClub.DomainEvents.csproj", "ManagementAPI.GolfClub.DomainEvents/"]
	COPY ["ManagementAPI.Service.DataTransferObjects/ManagementAPI.Service.DataTransferObjects.csproj", "ManagementAPI.Service.DataTransferObjects/"]
		
	RUN dotnet restore "ManagementAPI.Service/ManagementAPI.Service.csproj"
	COPY . .
	WORKDIR "/src/ManagementAPI.Service"
	RUN dotnet build "ManagementAPI.Service.csproj" -c Release -o /app

	FROM build AS publish
	RUN dotnet publish "ManagementAPI.Service.csproj" -c Release -o /app

	FROM microsoft/dotnet:2.2-aspnetcore-runtime AS final
	WORKDIR /app/projections
	COPY ["ManagementAPI.Projections/", "."]	

	WORKDIR /app
	COPY --from=publish /app .
	ENTRYPOINT ["dotnet", "ManagementAPI.Service.dll"]