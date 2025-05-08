FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["kubernetes-app/kubernetes-app.csproj", "kubernetes-app/"]
RUN dotnet restore "kubernetes-app/kubernetes-app.csproj"
COPY . .
WORKDIR "/src/kubernetes-app"
RUN dotnet build "kubernetes-app.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "kubernetes-app.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "kubernetes-app.dll"]
