#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Backend/CookRun/CookRun.csproj", "CookRun/"]
COPY ["Backend/Domain/CookRun.Domain.csproj", "Domain/"]
COPY ["Backend/AmoClient/AmoClient.csproj", "AmoClient/"]
COPY ["Backend/RoistatClient/RoistatClient.csproj", "RoistatClient/"]
RUN dotnet restore "CookRun/CookRun.csproj"
COPY Backend/ .
WORKDIR "/src/CookRun"
RUN dotnet build "CookRun.csproj" -c Release -o /app/build

FROM build AS publish
ARG appVersion
RUN dotnet publish "CookRun.csproj" -c Release /p:AssemblyVersion=$appVersion -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CookRun.dll"]