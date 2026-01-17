FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CategoryService/CategoryService.API/CategoryService.API.csproj", "CategoryService/CategoryService.API/"]
COPY ["CategoryService/CategoryService.Application/CategoryService.Application.csproj", "CategoryService/CategoryService.Application/"]
COPY ["CategoryService/CategoryService.Domain/CategoryService.Domain.csproj", "CategoryService/CategoryService.Domain/"]
COPY ["CategoryService/CategoryService.Infrastructure/CategoryService.Infrastructure.csproj", "CategoryService/CategoryService.Infrastructure/"]
COPY ["Stellar.Shared/Stellar.Shared.csproj", "Stellar.Shared/"]
RUN dotnet restore "CategoryService/CategoryService.API/CategoryService.API.csproj"
COPY . .
WORKDIR "/src/CategoryService/CategoryService.API"
RUN dotnet build "CategoryService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CategoryService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CategoryService.API.dll"]
