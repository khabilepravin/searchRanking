using APIProxy;
using Prism.Ioc;
using Prism.Unity;
using SearchRankingApp.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SearchRankingApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var prismBootstrappedWindow = Container.Resolve<MainWindow>();
            return prismBootstrappedWindow;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ISearchRankingAPIProxy, SearchRankingAPIProxy>();
        }
    }
}
