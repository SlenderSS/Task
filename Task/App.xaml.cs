using System.Configuration;
using System.Data;
using System.Windows;
using Task.Services.Implementations;
using Task.Services.Interfaces;
using Task.Views;

namespace Task
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IFileService, FileService>();
        }

        protected override Window CreateShell()
        {
            return ContainerLocator.Container.Resolve<MainView>();
        }
    }

}
