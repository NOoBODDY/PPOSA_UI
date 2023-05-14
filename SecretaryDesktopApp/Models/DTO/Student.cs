using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;
using SecretaryDesktopApp.ViewModels;

namespace SecretaryDesktopApp.Models.DTO;

public class Student:NotifyBase
{

    private int _id;

    
    [JsonPropertyName("id")]
    public int Id
    {
        get => _id;
        set => Update(ref _id, value);
    }

    private string _firstName;
    [Description("Имя")]
    [JsonPropertyName("firstname")]
    public string FirstName
    {
        get => _firstName;
        set => Update(ref _firstName, value);
    }

    private string _middleName;

    [Description("Фамилия")]
    [JsonPropertyName("middlename")]
    public string MiddleName
    {
        get => _middleName;
        set => Update(ref _middleName, value);
    }

    private string _lastName;

    [Description("Отчество")]
    [JsonPropertyName("lastname")]
    public string LastName
    {
        get => _lastName;
        set => Update(ref _lastName, value);
    }

    private int _profileTicketNumber;

    [JsonPropertyName("profileTicketNumber")]
    [Description("№ проф. билета")]
    public int ProfileTicketNumber
    {
        get => _profileTicketNumber;
        set => Update(ref _profileTicketNumber, value);
    }

    private string _group;
    [Description("Группа")]
    [JsonPropertyName("group")]
    public string Group
    {
        get => _group;
        set => Update(ref _group, value);
    }

    private string _institute;

    [Description("Институт")]
    [JsonPropertyName("institute")]
    public string Institute
    {
        get => _institute;
        set => Update(ref _institute, value);
    }

    private System.Boolean _extended;

    [Description("Продление")]
    [JsonPropertyName("extended")]
    public System.Boolean Extended
    {
        get => _extended;
        set => Update(ref _extended, value);
    }

    private ObservableCollection<TicketExtension> _ticketExtensions;

    [JsonPropertyName("ticketExtensions")]
    public ObservableCollection<TicketExtension> TicketExtensions
    {
        get => _ticketExtensions;
        set => Update(ref _ticketExtensions, value);
    }

    private StudentAdditionalInfo _additionalInfo;

    [JsonIgnore]
    public StudentAdditionalInfo AdditionalInfo
    {
        get => _additionalInfo;
        set => Update(ref _additionalInfo, value);
    }

}