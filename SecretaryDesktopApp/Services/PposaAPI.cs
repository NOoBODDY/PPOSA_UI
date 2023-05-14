using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serializers.Json;
using SecretaryDesktopApp.Models.DTO;

namespace SecretaryDesktopApp.Services;

public class PposaAPI
{
    private readonly RestClient _client;
    private readonly string baseUri = "http://pposa.gimaz.ru";
    public PposaAPI()
    {
        _client = new RestClient(baseUri, configureSerialization: s=> s.UseSystemTextJson(new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        }));
    }

    public async Task<Student[]> GetStudentsAsync(int startIndex, int endIndex, bool isDesc, string sortedBy)
    {
        var request = new RestRequest("api/v1/student/short/search", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        var requestBody = new StudentSearchRequest()
        {
            StartIndex = startIndex,
            EndIndex = endIndex,
            IsDesc = isDesc,
            SortedBy = "id"
        };
        request.AddJsonBody(requestBody);
        RestResponse response = await _client.ExecuteAsync(request);
        return JsonSerializer.Deserialize<Student[]>(response.Content);
    }

    public async Task<int> GetStudentsCount()
    {
        var request = new RestRequest("api/v1/student/short/metadata", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        var requestBody = new StudentSearchRequest()
        {
            StartIndex = 0,
            EndIndex = 0,
            IsDesc = false,
            SortedBy = "id"
        };
        request.AddJsonBody(requestBody);
        RestResponse response = await _client.ExecuteAsync(request);
        return Convert.ToInt32(response.Content);
    }

    public async Task GetStudentAdditions(Student student)
    {
        var request = new RestRequest($"api/v1/student/full/{student.Id}", Method.Get);
        var response = await _client.ExecuteAsync(request);
        var additionalInfo = JsonSerializer.Deserialize<StudentAdditionalInfo>(response.Content);
        var studentWithTicketExtensions = JsonSerializer.Deserialize<Student>(response.Content);
        student.AdditionalInfo = additionalInfo;
        student.TicketExtensions = studentWithTicketExtensions.TicketExtensions;
    }
}