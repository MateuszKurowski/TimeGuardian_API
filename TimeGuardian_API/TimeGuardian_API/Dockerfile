#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 9011
EXPOSE 9012


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TimeGuardian_API/TimeGuardian_API.csproj", "TimeGuardian_API/"]
RUN dotnet restore "TimeGuardian_API/TimeGuardian_API.csproj"
COPY . .
WORKDIR "/src/TimeGuardian_API"
RUN dotnet build "TimeGuardian_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeGuardian_API.csproj" -c Release -o /app/publish /p:UseAppHost=false
RUN dotnet dev-certs https

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeGuardian_API.dll"]
