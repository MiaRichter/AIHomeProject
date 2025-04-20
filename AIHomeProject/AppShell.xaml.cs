using AIHomeProject.Pages;

namespace AIHomeProject
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CreateComponentPage), typeof(CreateComponentPage));
            Routing.RegisterRoute(nameof(EditComponentPage), typeof(EditComponentPage));
        }
    }
}
