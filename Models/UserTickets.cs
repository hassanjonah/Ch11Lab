namespace Ch11Lab.Models;

public class UserTickets
{
    public User User { get; set; } = null!;

    public List<string> Tickets { get; set; } = new List<string>();
}