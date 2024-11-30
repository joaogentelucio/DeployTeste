# Base para execução no modo rápido (Padrão para Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Fase de compilação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DeployTeste/DeployTeste.csproj", "src/DeployTeste/"]
RUN dotnet restore "src/DeployTeste/DeployTeste.csproj"

# Copiar todos os arquivos para a construção do projeto
COPY . .
RUN dotnet build "src/DeployTeste/DeployTeste.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Fase de publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/DeployTeste/DeployTeste.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Fase final para execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeployTeste.dll"]
