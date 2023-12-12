using System.Text.Json.Serialization;

namespace Evergreen.Core.src.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    Customer,
    Admin
}