# Используем полный SDK-образ для сборки и запуска в одном контейнере
FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости
COPY ["src/ShpaginApp.csproj", "."]
RUN dotnet restore "ShpaginApp.csproj"

# Копируем весь исходный код
COPY src/ .

# Собираем приложение в Release режиме
RUN dotnet build "ShpaginApp.csproj" -c Release -o /app/build

# Публикуем приложение
RUN dotnet publish "ShpaginApp.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Переходим в папку с опубликованным приложением
WORKDIR /app/publish

# Создаем непривилегированного пользователя
RUN useradd -m -u 1000 dotnetuser

# Переходим на непривилегированного пользователя
USER dotnetuser

# Health check для проверки работоспособности
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD dotnet --info || exit 1

# Запускаем приложение
ENTRYPOINT ["dotnet", "ShpaginApp.dll"]
