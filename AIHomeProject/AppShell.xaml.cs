using AIHomeProject.Pages;

namespace AIHomeProject
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("ComponentFormPage", typeof(ComponentFormPage));
        }
    }
}
