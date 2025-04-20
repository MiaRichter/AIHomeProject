using AIHomeProject.ViewModels;
namespace AIHomeProject
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            OnAppearing();
            BindingContext = viewModel;
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
