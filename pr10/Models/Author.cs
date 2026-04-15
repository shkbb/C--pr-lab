namespace LibraryApp.Models;

public class Author
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int BirthYear { get; set; }

    public List<Book> Books { get; set; } = new();

    public override string ToString() =>
        $"[{Id}] {FirstName} {LastName} (нар. {BirthYear})";
}
