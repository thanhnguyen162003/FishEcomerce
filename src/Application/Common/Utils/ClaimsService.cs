using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Utils;

public interface IClaimsService
{
    public Guid GetCurrentUserId { get; }
    // public string GetCurrentUsername { get; }
    public string GetCurrentFullname { get; }
}

public class ClaimsService : IClaimsService
{

    public ClaimsService(IHttpContextAccessor httpContextAccessor)
    {
        var identity = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
        var extractedId = AuthenTools.GetCurrentAccountId(identity);
        // var username = AuthenTools.GetCurrentUsernam(identity);
        var fullname = AuthenTools.GetCurrentFullname(identity);
        GetCurrentUserId = string.IsNullOrEmpty(extractedId) ? Guid.Empty : new Guid(extractedId);
        // GetCurrentUsername = string.IsNullOrEmpty(username) ? "" : username;
        GetCurrentFullname = string.IsNullOrEmpty(fullname) ? "" : fullname;
    }
    
    public Guid GetCurrentUserId { get; }

    public string GetCurrentFullname { get; }
    // public string GetCurrentUsername { get; }
    // public string GetCurrentFullname { get; }
}

public class AuthenTools
{
    public static string? GetCurrentAccountId(ClaimsIdentity identity)
    {
        if (identity != null)
        {
            var userClaims = identity.Claims;
            return userClaims.FirstOrDefault(x => x.Type == "UserId")?.Value;
        }
        return null;
    }
    
    // public static string GetCurrentUsernam(ClaimsIdentity identity)
    // {
    //     if (identity != null)
    //     {
    //         var userClaims = identity.Claims;
    //         return userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
    //     }
    //     return null;
    // }
    //
    public static string? GetCurrentFullname(ClaimsIdentity identity)
    {
        if (identity != null)
        {
            var userClaims = identity.Claims;
            return userClaims.FirstOrDefault(x => x.Type == "Fullname")?.Value;
        }
        return null;
    }
}