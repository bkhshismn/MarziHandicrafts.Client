namespace Marzi.Client.Core.Requests;

public sealed class RegisterRequest
{
    public string Phone { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // از سمت Client عادی Role نفرستید.
    public string? Role { get; set; }
}