using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Threading;
using BFF.DataVirtualizingCollection.DataVirtualizingCollection;
using SecretaryDesktopApp.Controls;
using SecretaryDesktopApp.Models;
using SecretaryDesktopApp.Models.DTO;
using SecretaryDesktopApp.Services;

namespace SecretaryDesktopApp.ViewModels;

public class StudentsViewModel:PageBaseViewModel
{
    private readonly PposaAPI _apiService;

    private List<Filter> _filters;

    private string _sortedFieldName = "id";
    private bool _isDesc = false;
    public StudentsViewModel()
    {
        Title = "Студенты";
        IconData = "M12,5.5A3.5,3.5 0 0,1 15.5,9A3.5,3.5 0 0,1 12,12.5A3.5,3.5 0 0,1 8.5,9A3.5,3.5 0 0,1 12,5.5M5,8C5.56,8 6.08,8.15 6.53,8.42C6.38,9.85 6.8,11.27 7.66,12.38C7.16,13.34 6.16,14 5,14A3,3 0 0,1 2,11A3,3 0 0,1 5,8M19,8A3,3 0 0,1 22,11A3,3 0 0,1 19,14C17.84,14 16.84,13.34 16.34,12.38C17.2,11.27 17.62,9.85 17.47,8.42C17.92,8.15 18.44,8 19,8M5.5,18.25C5.5,16.18 8.41,14.5 12,14.5C15.59,14.5 18.5,16.18 18.5,18.25V20H5.5V18.25M0,20V18.5C0,17.11 1.89,15.94 4.45,15.6C3.86,16.28 3.5,17.22 3.5,18.25V20H0M24,20H20.5V18.25C20.5,17.22 20.14,16.28 19.55,15.6C22.11,15.94 24,17.11 24,18.5V20Z";
        /*var student = new Student() { FirstName = "test", MiddleName = "test2" };
        _students.Add(student);*/
       
        _apiService = new PposaAPI();
        CreateNewCollection();
        
        CreateColumnActivations();
    }

    private IDataVirtualizingCollection<Student> _students;

    public IDataVirtualizingCollection<Student> Students
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


    private void CreateNewCollection()
    {
        Func<int, int, Student> placeholderFactory = (page, items) =>
            new Student();
        Func<int, int, CancellationToken, Task<Student[]>> TaskpageFetcher = (offset, pageSize, _) =>
            _apiService.GetStudentsAsync(offset, offset + pageSize, _isDesc, _sortedFieldName);
        Func<CancellationToken, Task<int>> TaskcountFetcher = _ =>
            _apiService.GetStudentsCount();
        _students = DataVirtualizingCollectionBuilder.Build<Student>(AvaloniaScheduler.Instance).Preloading(placeholderFactory).LeastRecentlyUsed(3).TaskBasedFetchers(TaskpageFetcher, TaskcountFetcher).AsyncIndexAccess(placeholderFactory);
    }
    public void Sorting(object eventArgs)
    {
        if (eventArgs is not StudentGridControl.SortingEventArg args) return;
        _sortedFieldName = args.ColumnName;
        CreateNewCollection();
    }

    public async Task LoadAdditionalInfo(object arg)
    {
        if (arg is Student student)
        {
            await _apiService.GetStudentAdditions(student);
        }
    }

    private void CreateColumnActivations()
    {
        var properties = typeof(Student).GetProperties();
        foreach (var property in properties)
        {
            var header = ParseDescription(property);
            if (header == null)
                continue;
            var columnActivation = new StudentColumnActivation(header, new Binding(property.Name),true);
            _columnActivations.Add(columnActivation);
        }
    }

    private TableColumnHeader? ParseDescription(MemberInfo property)
    {
        var attributes = property.GetCustomAttributes();
        string originalName = "";
        string visibleName = "";
        foreach (var attribute in attributes)
        {
            
            /*if (!string.IsNullOrEmpty(originalName) && !string.IsNullOrEmpty(visibleName))
                break;*/
            if (attribute is DescriptionAttribute descriptionAttribute)
            {
                visibleName = descriptionAttribute.Description;
                continue;
            }

            if (attribute is JsonPropertyNameAttribute jsonPropertyNameAttribute)
            {
                originalName = jsonPropertyNameAttribute.Name;
                continue;
            }
            
            if (attribute is JsonIgnoreAttribute)
            {
                return null;
            }
            
        }

        if (string.IsNullOrEmpty(originalName))
            originalName = property.Name;
        if (string.IsNullOrEmpty(visibleName))
            visibleName = originalName;
        

        return new TableColumnHeader(visibleName, originalName);
    }
    
}