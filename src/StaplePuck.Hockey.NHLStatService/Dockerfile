FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY nuget.config src/
COPY ["src/StaplePuck.Hockey.NHLStatService/StaplePuck.Hockey.NHLStatService.csproj", "src/StaplePuck.Hockey.NHLStatService/"]
RUN dotnet restore "src/StaplePuck.Hockey.NHLStatService/StaplePuck.Hockey.NHLStatService.csproj"
COPY . .
WORKDIR "/src/src/StaplePuck.Hockey.NHLStatService"
RUN dotnet build "StaplePuck.Hockey.NHLStatService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "StaplePuck.Hockey.NHLStatService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "StaplePuck.Hockey.NHLStatService.dll"]