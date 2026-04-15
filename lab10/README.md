# Лабораторна 10 — Full EF Core App (4 сутності, Repository, LINQ)

## Доменна модель
```
Author ─── Book ─── Loan ─── Member
  1:*        *:*
```
| Сутність | Поля |
|----------|------|
| `Author` | Id, FirstName, LastName, BirthYear |
| `Book` | Id, Title, ISBN, PublishedYear, Genre, IsAvailable, AuthorId |
| `Member` | Id, FullName, Email, JoinDate |
| `Loan` | Id, BookId, MemberId, LoanDate, ReturnDate |

## Структура
```
lab10/
├── Data/LibraryContext.cs
├── Models/  Author.cs / Book.cs / Member.cs / Loan.cs
├── Repositories/
│   ├── IRepository.cs        # Generic + IBookRepository interfaces
│   └── BookRepository.cs     # Реалізація з LINQ
├── Program.cs                # Demo scenario
└── LibraryApp.csproj
```

## Ключові LINQ запити
- `Include(b => b.Author)` — eager loading
- `.ThenInclude(b => b.Author)` — вкладений Include через Loan
- `GroupBy(b => b.Genre)` — групування книг за жанром
- `Where` + `OrderBy` + `Contains` — фільтрація та пошук

## Запуск
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_Strong_Password_123!" \
  -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest

cd lab10
dotnet run
```
