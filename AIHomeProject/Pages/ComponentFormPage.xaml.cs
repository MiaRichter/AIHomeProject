using AIHomeProject.ViewModels;
using AIHomeProject.Services;
namespace AIHomeProject.Pages;

public partial class ComponentFormPage : ContentPage
{
    public ComponentFormPage(ComponentFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}