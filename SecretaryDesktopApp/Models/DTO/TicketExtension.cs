using System.ComponentModel;
using System.Text.Json.Serialization;
using SecretaryDesktopApp.ViewModels;

namespace SecretaryDesktopApp.Models.DTO;

public class TicketExtension:NotifyBase
{
    private int _id;

    [Description("id")]
    [JsonPropertyName("id")]
    public int Id
    {
        get => _id;
        set => Update(ref _id, value);
    }

    private string _semesterTitle;

    [Description("Семестр")]
    [JsonPropertyName("semesterTitle")]
    public string SemesterTitle
    {
        get => _semesterTitle;
        set => Update(ref _semesterTitle, value);
    }

    private int _semesterId;

    [Description("id Семестра")]
    [JsonPropertyName("semesterId")]
    public int SemesterId
    {
        get => _semesterId;
        set => Update(ref _semesterId, value);
    }

    private bool _payment;

    [Description("Оплата")]
    [JsonPropertyName("payment")]
    public bool Payment
    {
        get => _payment;
        set => Update(ref _payment, value);
    }
}