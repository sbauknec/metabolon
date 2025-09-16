public class RefreshToken
{
    public int Id { get; set; }
    public required string Token { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime Expires { get; set; }
    public int UserId { get; set; }
}