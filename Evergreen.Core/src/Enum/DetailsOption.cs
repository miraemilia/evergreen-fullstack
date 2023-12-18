using System.Text.Json.Serialization;

namespace Evergreen.Core.src.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DetailsOption
{
    Low,
    Medium,
    High
}