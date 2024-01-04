using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;

namespace HowTo.UseHostInWPF;

partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string title = "How to Use Host in WPF";
    [ObservableProperty] private string? message;

    public MainViewModel(IConfiguration configuration)
    {
        Message = configuration["ux:message"];
    }
    
    [RelayCommand]
    void Greetings() => Message = "Greetings!";
}