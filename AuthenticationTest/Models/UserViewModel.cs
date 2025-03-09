public class UserViewModel
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; } = "No Role"; // ✅ Default to "No Role"
}
