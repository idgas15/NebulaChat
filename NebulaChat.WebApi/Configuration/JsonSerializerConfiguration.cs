using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace NebulaChat.WebApi.Configuration
{
    public static  class JsonSerializerConfiguration
    {   
        public static void Configure(IServiceCollection services)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();

            var serializer = JsonSerializer.Create(settings);

            services.Add(new ServiceDescriptor(typeof(JsonSerializer),
                                            provider => serializer,
                                            ServiceLifetime.Transient));
        }
    }
}
