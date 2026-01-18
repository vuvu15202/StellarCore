FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ExaminationService/ExaminationService.API/ExaminationService.API.csproj", "ExaminationService/ExaminationService.API/"]
COPY ["ExaminationService/ExaminationService.Application/ExaminationService.Application.csproj", "ExaminationService/ExaminationService.Application/"]
COPY ["ExaminationService/ExaminationService.Domain/ExaminationService.Domain.csproj", "ExaminationService/ExaminationService.Domain/"]
COPY ["ExaminationService/ExaminationService.Infrastructure/ExaminationService.Infrastructure.csproj", "ExaminationService/ExaminationService.Infrastructure/"]
COPY ["Stellar.Shared/Stellar.Shared.csproj", "Stellar.Shared/"]
RUN dotnet restore "ExaminationService/ExaminationService.API/ExaminationService.API.csproj"
COPY . .
WORKDIR "/src/ExaminationService/ExaminationService.API"
RUN dotnet build "ExaminationService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExaminationService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExaminationService.API.dll"]
