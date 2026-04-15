# Library Management System — C# Assignments

Цей репозиторій містить послідовне виконання практичних і лабораторних робіт на базі C# / .NET 8 / EF Core.
Проєкт еволюціонує: від простого консольного застосунку у `pr9` до багатошарової Web API + Razor Pages системи у `lab14`.

## ⚙️ Попередні вимоги (ОБОВ'ЯЗКОВО)

Всі проєкти налаштовано на роботу із **SQL Server** базою даних, розгорнутою локально. Оскільки ви на Linux, перед тестуванням будь-якого з проєктів необхідно запустити Docker-контейнер з MSSQL:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Your_Strong_Password_123!" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

*Бази даних (`LibraryDB_pr9`, `LibraryDB_lab12` тощо) будуть створюватися автоматично (завдяки `EnsureCreated()` або міграціям під час запуску).*

---

## 🚀 Як запускати проєкти

### 1-й блок: Незалежні проєкти (pr9 – lab11)
Ці папки містять окремі самостійні консольні застосунки або прості API.

1. **pr9** (Консольний EF Core CRUD)
   ```bash
   cd pr9 && dotnet run
   ```
2. **pr10** (Міграції та Change Tracking)
   ```bash
   cd pr10 && dotnet run
   ```
3. **lab10** (Репозиторій та LINQ)
   ```bash
   cd lab10 && dotnet run
   ```
4. **pr11** (Базовий Web API на 3 endpoint-и)
   ```bash
   cd pr11 && dotnet run
   # Відкрийте у браузері: http://localhost:5253/swagger
   ```
5. **lab11** (Web API: DTOs, HTTP статуси)
   ```bash
   cd lab11 && dotnet run
   # Відкрийте у браузері: http://localhost:5169/swagger
   ```

*(Порти можуть незначно відрізнятися, звертайте увагу на вивід консолі після запуску).*

---

### 2-й блок: Solution-орієнтовані (lab12 – lab14)
Починаючи з `lab12`, проєкт реорганізовано у **багатошарову архітектуру** (`LibrarySystem.sln`), що включає в себе `Library.Domain`, `Library.Application`, `Library.Infrastructure`, `Library.Api`, а пізніше й `Library.Web`.

Щоб запустити API для кожної роботи:

1. **lab12** (Layered Architecture + AutoMapper)
   ```bash
   cd lab12 && dotnet run --project Library.Api/Library.Api.csproj
   # Swagger: http://localhost:<port>/swagger
   ```

2. **pr12** (RequestLoggingMiddleware)
   ```bash
   cd pr12 && dotnet run --project Library.Api/Library.Api.csproj
   ```

3. **pr13** (ExceptionHandlingMiddleware та обробка помилок)
   ```bash
   cd pr13 && dotnet run --project Library.Api/Library.Api.csproj
   # Протестуйте endpoint-и: http://localhost:<port>/api/DemoExceptions/bad-request
   ```

4. **pr14** (Swagger XML Docs та Route/Query параметри)
   ```bash
   cd pr14 && dotnet run --project Library.Api/Library.Api.csproj
   # Протестуйте пошук: GET /api/books/search?title=...
   ```

5. **pr15** (Додається Razor Pages UI - List, Create, Details)
   Запуск Web API:
   ```bash
   cd pr15 && dotnet run --project Library.Api/Library.Api.csproj
   ```
   **Для UI екранів (Razor Pages):**
   ```bash
   cd pr15 && dotnet run --project Library.Web/Library.Web.csproj
   # Перейдіть на адресу вебдодатка (напр., http://localhost:<port>)
   ```

6. **pr16** (Архітектура + Логування ILogger + Кешування IMemoryCache)
   ```bash
   cd pr16 && dotnet run --project Library.Api/Library.Api.csproj
   ```

7. **pr17** (JWT Auth)
   ```bash
   cd pr17 && dotnet run --project Library.Api/Library.Api.csproj
   # 1. Викличте POST /api/Auth/login JSON: {"username": "admin", "password": "admin"}
   # 2. Скопіюйте token. Для авторизації в Swagger натисніть "Authorize"
   ```

8. **lab13** (Повне Razor Pages CRUD - Update, Delete)
   Запуск UI додатку:
   ```bash
   cd lab13 && dotnet run --project Library.Web/Library.Web.csproj
   ```

9. **lab14** (Фінальна версія: всі напрацювання разом з інвалідацією кешу та детальним логуванням)
   ```bash
   cd lab14/Library.Api && dotnet run
   ```

### 💡 Порада при тестуванні багатошарових API
Більшість завдань вимагають запуск саме `Library.Api`. Достатньо зайти у папку із потрібним завданням та виконати `dotnet run --project Library.Api/Library.Api.csproj` (при цьому автоматично скомпілюються усі залежні проєкти типу Application, Domain, Infrastructure).

Якщо ви тестуєте UI (`pr15`, `lab13`), необхідно запускати `dotnet run --project Library.Web/Library.Web.csproj`.
