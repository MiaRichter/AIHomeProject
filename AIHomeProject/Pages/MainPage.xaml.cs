using AIHomeProject.ViewModels;
namespace AIHomeProject
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            OnAppearing();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is MainViewModel vm)
            {
                vm.LoadComponents();
            }
        }
    }

}
