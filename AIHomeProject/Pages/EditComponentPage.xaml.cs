using AIHomeProject.ViewModels;
using AIHomeProject.Models;
using Microsoft.Maui.Controls;

namespace AIHomeProject.Pages
{
    public partial class EditComponentPage : ContentPage
    {
        public EditComponentPage(EditComponentViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}