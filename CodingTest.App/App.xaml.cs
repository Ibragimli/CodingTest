using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CodingTest.Application.Abstractions.Services;
using CodingTest.Infrastructure.Services;
using CodingTest.Infrastructure;

namespace CodingTest.App
{
    public partial class App : System.Windows.Application
    {
        public static IHost? Host { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Catching unhandled exceptions in the UI thread  
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Handling background task exceptions (async/await)  
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            Host = CreateHostBuilder().Build();
            await Host.StartAsync();

            var mainWindow = Host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddInfrastructureServices();
                    services.AddScoped<MainWindow>();
                });

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"UI Error: {e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show($"Background Error: {e.Exception.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.SetObserved();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (Host != null)
            {
                await Host.StopAsync();
                Host.Dispose();
            }
            base.OnExit(e);
        }
    }
}
