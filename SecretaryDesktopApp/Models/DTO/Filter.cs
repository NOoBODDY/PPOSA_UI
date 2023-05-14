using System.Text.Json.Serialization;

namespace SecretaryDesktopApp.Models.DTO;

public class Filter
{
    [JsonPropertyName("key")]
    public string Key { get; set; }
    
    [JsonPropertyName("operation")]
    public string Operation { get; set; }
    
    [JsonPropertyName("value")]
    public string Value { get; set; }
}