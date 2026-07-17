FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Directory.Build.props", "./"]

COPY ["src/ConexaoSolidaria.DonationWorker/ConexaoSolidaria.DonationWorker.csproj", "src/ConexaoSolidaria.DonationWorker/"]
COPY ["src/ConexaoSolidaria.Application/ConexaoSolidaria.Application.csproj", "src/ConexaoSolidaria.Application/"]
COPY ["src/ConexaoSolidaria.Infrastructure/ConexaoSolidaria.Infrastructure.csproj", "src/ConexaoSolidaria.Infrastructure/"]
COPY ["src/ConexaoSolidaria.Domain/ConexaoSolidaria.Domain.csproj", "src/ConexaoSolidaria.Domain/"]
COPY ["src/ConexaoSolidaria.Contracts/ConexaoSolidaria.Contracts.csproj", "src/ConexaoSolidaria.Contracts/"]

RUN dotnet restore "src/ConexaoSolidaria.DonationWorker/ConexaoSolidaria.DonationWorker.csproj"

COPY . .

RUN dotnet publish "src/ConexaoSolidaria.DonationWorker/ConexaoSolidaria.DonationWorker.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM runtime AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ConexaoSolidaria.DonationWorker.dll"]
