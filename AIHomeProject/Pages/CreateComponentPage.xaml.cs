using AIHomeProject.ViewModels;
using AIHomeProject.Models;
using Microsoft.Maui.Controls;

namespace AIHomeProject.Pages
{
    public partial class CreateComponentPage : ContentPage
    {
        public CreateComponentPage(CreateComponentViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}