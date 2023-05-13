using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Avalonia.Data;
using SecretaryDesktopApp.Models;

namespace SecretaryDesktopApp.ViewModels;

public class StudentsViewModel:NotifyBase
{
    public StudentsViewModel()
    {
        var student = new Student() { Firstname = "test", Middlename = "test2" };
        _students.Add(student);
        CreateColumnActivations();
    }

    private ObservableCollection<Student> _students = new ObservableCollection<Student>();

    public ObservableCollection<Student> Students
    {
        get => _students;
        set => Update(ref _students, value);
    }
    
    
    private ObservableCollection<StudentColumnActivation> _columnActivations = new ();

    public ObservableCollection<StudentColumnActivation> ColumnActivations
    {
        get => _columnActivations;
        set => Update(ref _columnActivations, value);
    }

    public void Sorting()
    {
        
    }

    private void CreateColumnActivations()
    {
        var properties = typeof(Student).GetProperties();
        foreach (var property in properties)
        {
            var columnActivation = new StudentColumnActivation(ParseDescription(property), new Binding(property.Name),true);
            _columnActivations.Add(columnActivation);
        }
    }

    private string ParseDescription(MemberInfo property)
    {
        var attribute = property.GetCustomAttributes(typeof(DescriptionAttribute)).FirstOrDefault();
        if (attribute is DescriptionAttribute descriptionAttribute)
        {
            return descriptionAttribute.Description;
        }

        return property.Name;
    }
    
}