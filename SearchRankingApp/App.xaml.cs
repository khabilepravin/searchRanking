using APIProxy;
using Prism.Ioc;
using Prism.Unity;
using SearchRankingApp.Views;
using Serilog;
using System;
using System.IO;
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
            string logsDirectory = Path.Combine(Environment.CurrentDirectory, "logs");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.File(Path.Combine(logsDirectory, "errors.txt")) // Ideally this would be some kind of external sink e.g. Azure AppInsights or Amazon Cloudwatch
                .CreateLogger();

            SetupExceptionHandling();
            var prismBootstrappedWindow = Container.Resolve<MainWindow>();
            return prismBootstrappedWindow;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ISearchRankingAPIProxy, SearchRankingAPIProxy>();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
            {
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved();
            };
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            string message = $"Unhandled exception ({source})";
            try
            {
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = string.Format("Unhandled exception in {0} v{1} message: {2}", assemblyName.Name, assemblyName.Version, exception.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Exception in LogUnhandledException");
            }
            finally
            {
                Log.Logger.Error(exception, message);
                MessageBox.Show(message);
            }
        }
    }
}
