#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /src
#COPY ["src/Sukt.Core.ApiGateway/Sukt.Core.ApiGateway.csproj", "src/Sukt.Core.ApiGateway/"]
#RUN dotnet restore "src/Sukt.Core.ApiGateway/Sukt.Core.ApiGateway.csproj"
COPY . .
#WORKDIR "/src/src/Sukt.Core.ApiGateway"
#RUN dotnet build "Sukt.Core.ApiGateway.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Sukt.Core.ApiGateway.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sukt.Core.ApiGateway.dll"]