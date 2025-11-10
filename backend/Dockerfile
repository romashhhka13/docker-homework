FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /src

COPY ["src/ShpaginApp.csproj", "."]
RUN dotnet restore "ShpaginApp.csproj"

COPY src/ .

RUN dotnet build "ShpaginApp.csproj" -c Release -o /app/build

RUN dotnet publish "ShpaginApp.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

WORKDIR /app/publish

RUN useradd -m -u 1000 dotnetuser

USER dotnetuser

ENV ASPNETCORE_URLS=http://0.0.0.0:80
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "ShpaginApp.dll"]