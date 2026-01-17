FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CourseService/CourseService.API/CourseService.API.csproj", "CourseService/CourseService.API/"]
COPY ["CourseService/CourseService.Application/CourseService.Application.csproj", "CourseService/CourseService.Application/"]
COPY ["CourseService/CourseService.Domain/CourseService.Domain.csproj", "CourseService/CourseService.Domain/"]
COPY ["CourseService/CourseService.Infrastructure/CourseService.Infrastructure.csproj", "CourseService/CourseService.Infrastructure/"]
COPY ["Stellar.Shared/Stellar.Shared.csproj", "Stellar.Shared/"]
RUN dotnet restore "CourseService/CourseService.API/CourseService.API.csproj"
COPY . .
WORKDIR "/src/CourseService/CourseService.API"
RUN dotnet build "CourseService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CourseService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CourseService.API.dll"]
