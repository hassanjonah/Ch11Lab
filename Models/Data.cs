namespace Ch11Lab.Models;

public class Data
{
    public static List<User> Users => _users;

    public static List<string> GetTickets() => new List<string> 
    {
        "T978-654311231",
        "T978-654322112",
        "T978-654322155",
        "T978-654322200"
    };

    private static List<User> _users = new List<User>() {
        new User() {
            FirstName = "Jose",
            LastName = "Brown",
            Email = "jose_francis_brown@rocketmail.com",
            Phone = "208-936-3452",
            Street = "54541 Locust Street",
            City = "Gassville",
            RegionCode = "AR",
            PostalCode = "72635"
        },
        new User() {
            FirstName = "Stephen",
            LastName = "Miller",
            Email = "s_t_miller@rocketmail.com",
            Phone = "361-777-2467",
            Street = "83 7th Street East",
            City = "Loudon",
            RegionCode = "NH",
            PostalCode = "03307"
        },
        new User() {
            FirstName = "Emily",
            LastName = "Watson",
            Email = "emilymae@outlook.com",
            Phone = "927-162-9179",
            Street = "123 East Street",
            City = "Milligan",
            RegionCode = "NE",
            PostalCode = "68406"
        },
        new User() {
            FirstName = "Layla",
            LastName = "Walker",
            Email = "layla_s@live.com",
            Phone = "310-244-3699",
            Street = "57342 8th Street",
            City = "Bryants Store",
            RegionCode = "KY",
            PostalCode = "40921"
        },
        new User() {
            FirstName = "Haley",
            LastName = "Davis",
            Email = "haleymariedavis@yahoo.com",
            Phone = "743-195-6391",
            Street = "91016 Pleasant Street",
            City = "Belcourt",
            RegionCode = "ND",
            PostalCode = "58316"
        },
        new User() {
            FirstName = "Jasmine",
            LastName = "Cooper",
            Email = "jasminecooper@rocketmail.com",
            Phone = "209-959-0526",
            Street = "77 Broad Street",
            City = "Oakpark",
            RegionCode = "VA",
            PostalCode = "22730"
        }
    };
}
