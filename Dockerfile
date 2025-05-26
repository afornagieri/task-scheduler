# Etapa base com o runtime .NET 6
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

# Etapa de build com SDK .NET 6
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copia os arquivos do projeto
COPY . .

# Restaura as dependências
RUN dotnet restore "TaskScheduler.Api/TaskScheduler.Api.csproj"

# Compila o projeto
RUN dotnet build "TaskScheduler.Api/TaskScheduler.Api.csproj" -c Release -o /app/build

# Publica para pasta de saída
RUN dotnet publish "TaskScheduler.Api/TaskScheduler.Api.csproj" -c Release -o /app/publish

# Etapa final: usa imagem base com runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TaskScheduler.Api.dll"]
