# ============================================
# Stage 1: Восстановление зависимостей (caching)
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS restore
WORKDIR /src

# Копируем только файлы проекта для кэширования зависимостей
COPY ["MvcMovie.csproj", "."]
RUN dotnet restore "MvcMovie.csproj"

# ============================================
# Stage 2: Сборка приложения
# ============================================
FROM restore AS build
WORKDIR /src

# Копируем исходный код
COPY . .

# Собираем в Release режиме
RUN dotnet build "MvcMovie.csproj" -c Release -o /app/build

# ============================================
# Stage 3: Публикация
# ============================================
FROM build AS publish
WORKDIR /src
RUN dotnet publish "MvcMovie.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# ============================================
# Stage 4: Runtime (финальный образ)
# ============================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# Создаем непривилегированного пользователя для security best practices
RUN useradd -m -u 1000 dotnetuser

WORKDIR /app

# Копируем опубликованное приложение из предыдущего stage
COPY --from=publish /app/publish .

# Переходим на непривилегированного пользователя
USER dotnetuser

# Настройки для ASP.NET Core
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 80

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD dotnet --info || exit 1

ENTRYPOINT ["dotnet", "MvcMovie.dll"]
