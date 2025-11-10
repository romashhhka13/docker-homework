# Docker (Web API C#)

## 1. Первый этап - запуск веб-сервера на C# 

### Dockerfile (non-multistage)

***Сборка образа***
```bash
docker build -t <имя_образа>:<тег> <путь_к_контексту_сборки>
```

***Запуск контейнера***
```bash
docker run [OPTIONS] IMAGE [COMMAND] [ARG...]
```

***Проброс портов***
```bash
docker run -p [хост_порт]:[контейнер_порт] image_name
```

Пример:
```bash
-p 5555:8080 — порт 5555 на хосте будет проброшен на порт 8080 в контейнере
```


***Переменные окружения***
```bash
docker run -e VARIABLE_NAME=value image_name
```
Примеры:

```bash
-e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;..." — установка переменной окружения

-e ASPNETCORE_ENVIRONMENT=Development — переменная для ASP.NET

docker run -p 5555:8080 --rm -e ConnectionStrings__ShpaginAppContext="Host=host.docker.internal;Port=5432;Database=db;Username=postgres;Password=postgres" backend-non-multistage:latest
```

***Отключение контейнера:***
```bash
docker run -d -p 5555:8080 backend:latest
```

***Интерактивный режим***

-i (--interactive) — оставляет stdin открытым
-t (--tty) — выделяет псевдо-терминал
```bash
docker run -it backend:latest bash
```

***Имя контейнера***
```bash
docker run --name my-api -p 5555:8080 backend:latest
```

***Автоматическое удаление контейнера***
```bash
docker run --rm -p 5555:8080 backend:latest
```

***Переменные окружения из файла (--env-file)***
```bash
docker run --env-file .env -p 5555:8080 backend:latest
```
