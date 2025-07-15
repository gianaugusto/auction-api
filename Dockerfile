# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copia toda a pasta src com todos os projetos
COPY src src

# Define a pasta da API como diretório de trabalho
WORKDIR /app/src/AutoAuction.API

# Restaura dependências
RUN dotnet restore

# Publica o projeto
RUN dotnet publish -c Release -o /app/out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia os arquivos publicados do build
COPY --from=build-env /app/out .

# Expondo a porta
EXPOSE 80

# Entry point da aplicação
ENTRYPOINT ["dotnet", "AutoAuction.API.dll"]
