#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Prototype.Identity/Prototype.Identity.csproj", "Prototype.Identity/"]
RUN dotnet restore "Prototype.Identity/Prototype.Identity.csproj"
COPY . .
WORKDIR "/src/Prototype.Identity"
RUN dotnet build "Prototype.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Prototype.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Prototype.Identity.dll"]