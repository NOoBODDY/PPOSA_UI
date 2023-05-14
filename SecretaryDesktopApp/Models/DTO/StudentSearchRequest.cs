using System.Text.Json.Serialization;

namespace SecretaryDesktopApp.Models.DTO;

public class StudentSearchRequest
{
    [JsonPropertyName("filters")]
    public Filter[] Filters { get; set; }
    
    [JsonPropertyName("sortedBy")]
    public string SortedBy { get; set; }
    
    [JsonPropertyName("desc")]
    public bool IsDesc { get; set; }
    
    [JsonPropertyName("startIndex")]
    public int StartIndex { get; set; }
    
    [JsonPropertyName("endIndex")]
    public int EndIndex { get; set; }
}