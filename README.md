# Посібник із запуску проєктів

## 1. Запуск бази даних

Усі проєкти потребують локального SQL Server. Перед перевіркою робіт запустіть Docker-контейнер:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Your_Strong_Password_123!" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

## 2. Запуск базових проєктів (pr9 – lab11)

Перейдіть до вибраної директорії завдання та запустіть проєкт командою `dotnet run`:

```bash
cd pr9
dotnet run
```
За цим принципом запускаються: `pr9`, `pr10`, `lab10`, `pr11`, `lab11`.

## 3. Запуск рішення з багатошаровою архітектурою (lab12 – lab14)

Такі проєкти структурувано в `LibrarySystem.sln`. Ви повинні вказати конкретний шар виконання.

Для запуску серверної частини (Web API):
```bash
cd lab12
dotnet run --project Library.Api/Library.Api.csproj
```
Це стосується папок: `lab12`, `pr12`, `pr13`, `pr14`, `pr16`, `pr17`, `lab14`.

Для запуску клієнтської частини (Razor Pages):
```bash
cd pr15
dotnet run --project Library.Web/Library.Web.csproj
```
Це стосується папок: `pr15`, `lab13`.
