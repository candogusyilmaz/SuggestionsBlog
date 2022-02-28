using System.Security.Claims;

using Microsoft.AspNetCore.Components.Authorization;

namespace SuggestionsBlog.Server.Extensions;

public static class AuthenticationExtensions
{
    public static async Task<string> GetObjectIdentifierAsync(this AuthenticationStateProvider authProvider)
    {
        return (await authProvider.GetAuthenticationStateAsync()).User.Claims.FirstOrDefault(s => s.Type.Contains("objectidentifier"))?.Value;
    }

    public static async Task<User> GetUser(this AuthenticationStateProvider authProvider, IUserService userService)
    {
        return await userService.GetUserByObjectId(await authProvider.GetObjectIdentifierAsync()) ?? new();
    }

    public static string GetClaimValue(this AuthenticationState state, string claim, bool equals = false)
    {
        if (equals)
            return state.User.Claims.FirstOrDefault(c => c.Type.Equals(claim))?.Value;
        else
            return state.User.Claims.FirstOrDefault(c => c.Type.Contains(claim))?.Value;
    }
}
