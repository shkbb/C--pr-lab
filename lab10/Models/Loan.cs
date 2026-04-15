namespace LibraryApp.Models;

public class Loan
{
    public int Id { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public bool IsReturned => ReturnDate.HasValue;

    // Foreign keys
    public int BookId { get; set; }
    public Book? Book { get; set; }

    public int MemberId { get; set; }
    public Member? Member { get; set; }

    public override string ToString() =>
        $"[{Id}] Книга#{BookId} → Читач#{MemberId} | " +
        $"Видана: {LoanDate:dd.MM.yyyy} | " +
        $"{(IsReturned ? $"Повернена: {ReturnDate:dd.MM.yyyy}" : "Не повернена")}";
}
