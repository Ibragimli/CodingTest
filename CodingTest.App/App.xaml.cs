using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using CodingTest.Application.Abstractions.Services;
using CodingTest.Infrastructure.Services;

namespace CodingTest.App
{
    public partial class App : System.Windows.Application
    {
        public static IHost? Host { get; private set; }

        // App.xaml.cs faylında hostu qururuq
        public App()
        {
            // Host-u konfiqurasiya edirik
            Host = CreateHostBuilder().Build();

            // UI-ni yaratmadan əvvəl asılılıq inyeksiyasını işə salırıq
            var mainWindow = Host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<IFileLoader, CSVLoader>();
                services.AddScoped<IFileLoader, TXTLoader>();
                services.AddScoped<IFileLoader, XMLLoader>();
                services.AddScoped<IFileMonitorService, FileMonitorService>();
                services.AddScoped<MainWindow>();
            });

    }
}
