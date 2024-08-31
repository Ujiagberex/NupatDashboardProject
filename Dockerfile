# TO-DO: add Dockerfile description here

FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app

COPY . ./

RUN dotnet restore "NupatDashboardProject.csproj"

RUN dotnet build "NupatDashboardProject.csproj"

COPY ./bin/Release/net8.0/NupatDashboardProject.dll ./


EXPOSE 80

ENTRYPOINT ["dotnet", "NupatDashboardProject.dll"]
