using Microsoft.Extensions.Configuration;

namespace ICMSDemo.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
