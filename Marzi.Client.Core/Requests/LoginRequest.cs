namespace Marzi.Client.Core.Requests;

public class LoginRequest
{
    public string Phone { get; set; } = "";
    public string Username { get => Phone; set => Phone = value; }

    public string Password { get; set; } = "";
}
