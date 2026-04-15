namespace LibraryApp.Models;

public class Member
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }

    public override string ToString() =>
        $"[{Id}] {FullName} <{Email}> (з {JoinDate:dd.MM.yyyy})";
}
