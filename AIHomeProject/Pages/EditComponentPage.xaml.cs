using AIHomeProject.ViewModels;
using AIHomeProject.Models;
using Microsoft.Maui.Controls;

namespace AIHomeProject.Pages
{
    [QueryProperty(nameof(Component), "Component")]
    public partial class EditComponentPage : ContentPage
    {
        public Component Component { get; set; } // Должен вызываться сеттер!
        public EditComponentPage(EditComponentViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            if (Component != null && BindingContext is EditComponentViewModel vm)
            {
                vm.Initialize(Component);
            }
            else
            {
                // Обработка случая, когда компонент не передан
                Shell.Current.GoToAsync("..");
            }
        }
    }
}