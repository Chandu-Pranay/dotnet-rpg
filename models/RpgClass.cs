using System.Text.Json.Serialization;

namespace dotnet_rpg.models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClass
    {
        Knight=1,
        Mage=2,
        clerik=3
    }
}