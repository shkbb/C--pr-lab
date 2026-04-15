namespace LibraryApp.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublishedYear { get; set; }

    // Foreign key
    public int AuthorId { get; set; }

    // Navigation property: many Books → one Author
    public Author? Author { get; set; }

    public override string ToString() =>
        $"[{Id}] \"{Title}\" (ISBN: {ISBN}, Year: {PublishedYear}, AuthorId: {AuthorId})";
}
