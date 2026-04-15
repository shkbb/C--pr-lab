# Практична 10 — EF Core Migrations, Change Tracking

## Що нового відносно pr9
| Зміна | Деталі |
|-------|--------|
| Нове поле | `Book.Genre` — жанр книги |
| Нова сутність | `Member` (читач бібліотеки) |
| Паттерн | Короткотривалий `DbContext` для кожної операції |
| Демонстрація | Change Tracking: `EntityState` до/після зміни та після `SaveChanges()` |

## Структура проєкту
```
pr10/
├── Data/
│   └── LibraryContext.cs   # DbContext (+ Member DbSet, Genre config)
├── Models/
│   ├── Author.cs
│   ├── Book.cs              # + Genre поле
│   └── Member.cs            # Нова сутність
├── Program.cs               # Demo: зміни + change tracking
└── LibraryApp.csproj
```

## Запуск
```bash
# SQL Server Docker (якщо ще не запущений)
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_Strong_Password_123!" \
  -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest

cd pr10
dotnet run
```

## Симуляція міграцій (реальна команда)
```bash
# Встановити dotnet-ef tool
dotnet tool install --global dotnet-ef --version 8.0.0

# Додати міграцію
dotnet ef migrations add AddGenreAndMember

# Застосувати до DB
dotnet ef database update
```

> В демо-режимі використовується `EnsureCreated()` для спрощення, що є еквівалентом результату першої міграції.
