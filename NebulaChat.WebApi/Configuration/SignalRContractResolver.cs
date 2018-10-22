using Newtonsoft.Json.Serialization;
using System;

namespace NebulaChat.WebApi.Configuration
{
    public class SignalRContractResolver : IContractResolver
    {
        private readonly IContractResolver _camelCaseContractResolver;
        private readonly IContractResolver _defaultContractSerializer;

        public SignalRContractResolver()
        {
            _defaultContractSerializer = new DefaultContractResolver();
            _camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public JsonContract ResolveContract(Type type)
        {
            return _camelCaseContractResolver.ResolveContract(type);
        }
    }
}
