using System.Text.Json.Serialization;
using SecretaryDesktopApp.ViewModels;

namespace SecretaryDesktopApp.Models.DTO;

public class UserModel:NotifyBase
{
    private string _username;
    [JsonPropertyName("username")]
    public string Username
    {
        get => _username;
        set => Update(ref _username, value);
    }

    private string _fullname;

    [JsonPropertyName("fullName")]
    public string FullName
    {
        get => _fullname;
        set => Update(ref _fullname, value);
    }

    private string _avatar;

    [JsonPropertyName("avatar")]
    public string Avatar
    {
        get => _avatar;
        set => Update(ref _avatar, value);
    }

    private Roles[] _roles;

    [JsonPropertyName("roles")]
    public Roles[] Roles
    {
        get => _roles;
        set => Update(ref _roles, value);
    }
    
    [JsonPropertyName("authorities")]
    public Authorities[] Authorities { get; set; }

    private int _studentId;

    [JsonPropertyName("studentId")]
    public int StudentId
    {
        get => _studentId;
        set => Update(ref _studentId, value);
    }
}

public class Roles
{
    public int id { get; set; }
    public string title { get; set; }
    public int rank { get; set; }
    public Authorities[] authorities { get; set; }
}


public class Authorities
{
    public int id { get; set; }
    public string title { get; set; }
    public string authority { get; set; }
}