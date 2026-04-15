namespace LibraryApp.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
    public string Genre { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;

    public int AuthorId { get; set; }
    public Author? Author { get; set; }

    public List<Loan> Loans { get; set; } = new();

    public override string ToString() =>
        $"[{Id}] \"{Title}\" ({Genre}, {PublishedYear}) " +
        $"ISBN={ISBN} | {(IsAvailable ? "Доступна" : "Видана")}";
}
