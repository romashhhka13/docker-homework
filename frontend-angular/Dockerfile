# ============================================
# Stage 1: Build Angular приложения
# ============================================
FROM node:20-alpine AS build

WORKDIR /app

# Копируем только package.json и package-lock.json для кэширования зависимостей
COPY package*.json ./

# Устанавливаем зависимости
RUN npm ci

# Копируем весь исходный код
COPY . .

# Собираем Angular приложение в production режиме
RUN npm run build -- --configuration production

# ============================================
# Stage 2: Nginx для раздачи статики
# ============================================
FROM nginx:alpine AS final

# Копируем собранное приложение из stage build
COPY --from=build /app/dist/angular_app/browser /usr/share/nginx/html

# Копируем кастомную конфигурацию Nginx (опционально)
COPY nginx.conf /etc/nginx/nginx.conf

# Открываем порт 80
EXPOSE 80

# Запускаем Nginx
CMD ["nginx", "-g", "daemon off;"]
