using System.ComponentModel;
using System.Text.Json.Serialization;
using SecretaryDesktopApp.ViewModels;

namespace SecretaryDesktopApp.Models.DTO;

public class StudentAdditionalInfo:NotifyBase
{
    private string _dateOfEntry;

    [Description("Дата вступления")]
    [JsonPropertyName("dateOfEntry")]
    public string DateOfEntry
    {
        get => _dateOfEntry;
        set => Update(ref _dateOfEntry, value);
    }

    private string _dateOfLeaving;
    
    [Description("Дата выхода")]
    [JsonPropertyName("dateOfLeaving")]
    public string DateOfLeaving
    {
        get => _dateOfLeaving;
        set => Update(ref _dateOfLeaving, value);
    }

    private string _email;

    [Description("Email")]
    [JsonPropertyName("email")]
    public string Email
    {
        get => _email;
        set => Update(ref _email, value);
    }

    private string _phone;

    [Description("Телефон")]
    [JsonPropertyName("phone")]
    public string Phone
    {
        get => _phone;
        set => Update(ref _phone, value);
    }

    private string _address;

    [Description("Адрес")]
    [JsonPropertyName("address")]
    public string Address
    {
        get => _address;
        set => Update(ref _address, value);
    }

    private string _status;

    [Description("Статус")]
    [JsonPropertyName("status")]
    public string Status
    {
        get => _status;
        set => Update(ref _status, value);
    }
    
    
}