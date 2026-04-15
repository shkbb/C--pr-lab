namespace LibraryApp.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublishedYear { get; set; }

    // ──── NEW FIELD added in pr10 migration ────
    public string Genre { get; set; } = string.Empty;

    public int AuthorId { get; set; }
    public Author? Author { get; set; }

    public override string ToString() =>
        $"[{Id}] \"{Title}\" ({Genre}, {PublishedYear}) ISBN={ISBN}";
}
