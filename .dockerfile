# Use a imagem do SDK do .NET para compilar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS build-env

# Configure o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copie os arquivos de projeto e restaure as dependências
COPY *.csproj ./
RUN dotnet restore

# Copie todo o código fonte e compile a aplicação
COPY . ./
RUN dotnet publish -c Release -o out

# Use a imagem de runtime do .NET para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Configure o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copie os arquivos compilados do estágio anterior
COPY --from=build-env /app/out .

# Exponha a porta que a aplicação vai rodar
EXPOSE 80

# Configure o comando de entrada
ENTRYPOINT ["dotnet", "Api.dll"]
