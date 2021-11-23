using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BackupsExtra.Services
{
    public class PrivateContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.Writable = (member as PropertyInfo)?.GetSetMethod(true) != null;

            return property;
        }
    }
}