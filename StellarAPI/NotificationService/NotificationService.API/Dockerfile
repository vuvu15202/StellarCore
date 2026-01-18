FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NotificationService/NotificationService.API/NotificationService.API.csproj", "NotificationService/NotificationService.API/"]
COPY ["NotificationService/NotificationService.Application/NotificationService.Application.csproj", "NotificationService/NotificationService.Application/"]
COPY ["NotificationService/NotificationService.Domain/NotificationService.Domain.csproj", "NotificationService/NotificationService.Domain/"]
COPY ["NotificationService/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj", "NotificationService/NotificationService.Infrastructure/"]
COPY ["Stellar.Shared/Stellar.Shared.csproj", "Stellar.Shared/"]
RUN dotnet restore "NotificationService/NotificationService.API/NotificationService.API.csproj"
COPY . .
WORKDIR "/src/NotificationService/NotificationService.API"
RUN dotnet build "NotificationService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.API.dll"]
