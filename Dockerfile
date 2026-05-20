FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["LibraryOpitech.sln", "."]
COPY ["Core/Domain/LibraryOpitech.Domain.csproj", "Core/Domain/"]
COPY ["Core/Application/LibraryOpitech.Application.csproj", "Core/Application/"]
COPY ["Infrastructure/Infrastructure/LibraryOpitech.Infrastructure.csproj", "Infrastructure/Infrastructure/"]
COPY ["Presentation/Api/LibraryOpitech.Api.csproj", "Presentation/Api/"]

RUN dotnet restore "Presentation/Api/LibraryOpitech.Api.csproj"

COPY . .
RUN dotnet publish "Presentation/Api/LibraryOpitech.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LibraryOpitech.Api.dll"]
