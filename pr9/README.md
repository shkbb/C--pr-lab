# Практична 9 — EF Core CRUD з SQL Server

## Предметна область
Система управління бібліотекою. Сутності: **Author** (Автор) та **Book** (Книга).

## Структура проєкту
```
pr9/
├── Data/
│   └── LibraryContext.cs   # DbContext + конфігурація моделей
├── Models/
│   ├── Author.cs            # Сутність Автор
│   └── Book.cs              # Сутність Книга (FK AuthorId)
├── Program.cs               # Демонстраційний сценарій CRUD
└── LibraryApp.csproj
```

## Залежності
- `Microsoft.EntityFrameworkCore.SqlServer 8.0.0`
- `Microsoft.EntityFrameworkCore.Design 8.0.0`

## Підготовка до запуску

### 1. Запустити SQL Server у Docker
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_Strong_Password_123!" \
  -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Застосувати схему БД та запустити
```bash
cd pr9
dotnet run
```
> База даних `LibraryDB_pr9` та таблиці створюються автоматично через `EnsureCreated()`.

## Демонстраційний сценарій
Програма послідовно виконує:
1. **CREATE** — додає 2 авторів і 3 книги
2. **READ** — завантажує авторів із книгами через `Include()`
3. **UPDATE** — змінює назву та рік видання книги
4. **DELETE** — видаляє одну книгу
5. **Підсумок** — виводить фінальний стан БД

## Рядок підключення
```
Server=localhost,1433;Database=LibraryDB_pr9;User Id=sa;Password=Your_Strong_Password_123!;TrustServerCertificate=True;
```
