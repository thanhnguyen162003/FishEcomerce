using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Domain.Constants;

public class DefaultPassword
{
    public string Password { get; }
    
    private DefaultPassword(string password)
    {
        Password = password;
    }
    
    public static DefaultPassword Create(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetService<IOptions<DefaultPassword>>();
        return new DefaultPassword(options.Value.Password);
    }
}