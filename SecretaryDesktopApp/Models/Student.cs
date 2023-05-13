using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SecretaryDesktopApp.Models;

public class Student
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("firstname")]
    [Description("Имя")]
    public string Firstname { get; set; }
    [JsonPropertyName("middlename")]
    [Description("Фамилия")]
    public string Middlename { get; set; }
    
    [JsonPropertyName("lastname")]
    [Description("Отчество")]
    public string Lastname { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("group")]
    public string Group { get; set; }
    [JsonPropertyName("phone")]
    public string Phone { get; set; }
    [JsonPropertyName("formOfStudy")]
    public string FormOfStudy { get; set; }
    [JsonPropertyName("profileTicketNumber")]
    public int ProfileTicketNumber { get; set; }
    [JsonPropertyName("address")]
    public string Address { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; }
    [JsonPropertyName("dateOfEntry")]
    public string DateOfEntry { get; set; }
    [JsonPropertyName("dateOfLeaving")]
    public string DateOfLeaving { get; set; }
    [JsonPropertyName("materialAidList")]
    public MaterialAidList[] MaterialAidList { get; set; }
    [JsonPropertyName("ticketExtensions")]
    public TicketExtensions[] TicketExtensions { get; set; }
    [JsonPropertyName("rolesInGroup")]
    public RolesInGroup[] RlesInGroup { get; set; }
    [JsonPropertyName("rolesInPPOSA")]
    public RolesInPPOSA[] RolesInPPOSA { get; set; }
    [JsonPropertyName("documents")]
    public Documents[] Documents { get; set; }
}

public class MaterialAidList
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("monthNumber")]
    public string MonthNumber { get; set; }
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
}

public class TicketExtensions
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("semester")]
    public Semester Semester { get; set; }
    [JsonPropertyName("payment")]
    public bool Payment { get; set; }
}

public class Semester
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
}

public class RolesInGroup
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("rank")]
    public int Rank { get; set; }
    [JsonPropertyName("authorities")]
    public Authorities[] Authorities { get; set; }
}

public class Authorities
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("authority")]
    public string Authority { get; set; }
}

public class RolesInPPOSA
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("rank")]
    public int Rank { get; set; }
    [JsonPropertyName("authorities")]
    public Authorities[] Authorities { get; set; }
}


public class Documents
{
    public int id { get; set; }
    public Document document { get; set; }
    public string actionType { get; set; }
}

public class Document
{
    public int id { get; set; }
    public Type type { get; set; }
    public string title { get; set; }
    public string[] document { get; set; }
    public string documentFormat { get; set; }
    public string createdDateTime { get; set; }
}

public class Type
{
    public int id { get; set; }
    public string title { get; set; }
}

