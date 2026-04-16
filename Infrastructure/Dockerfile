# Utiliza o SDK do .NET 8 como base para compilação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos de projeto e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia todo o código fonte e compila
COPY . ./
RUN dotnet build -c Release -o out

# Utiliza a imagem do Playwright que já contém as dependências de sistema para os browsers
# IMPORTANTE: A imagem mcr.microsoft.com/playwright/dotnet:v1.49.0-noble (ou similar) 
# é recomendada para rodar testes .NET com Playwright em Linux/Docker.
FROM mcr.microsoft.com/playwright/dotnet:v1.49.0-noble AS final
WORKDIR /app
COPY --from=build /app/out .

# Configurações de ambiente
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV TEST_ENVIRONMENT=producao

# Comando padrão para rodar os testes
# O parâmetro --results-directory define onde o relatório (trx/allure) será gerado
ENTRYPOINT ["dotnet", "test", "--logger:trx", "--results-directory:TestResults"]
