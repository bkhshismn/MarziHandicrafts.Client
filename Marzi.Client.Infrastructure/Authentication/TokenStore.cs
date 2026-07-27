using Marzi.Client.Core.Models;

namespace Marzi.Client.Infrastructure.Authentication;

public class TokenStore
{
    public CurrentUser? User { get; set; }

    public bool IsLoggedIn => User != null;
}