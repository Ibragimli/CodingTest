using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CodingTest.Application.Abstractions.Services;
using CodingTest.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace CodingTest.Infrastructure.Services
{
    public class FileMonitorService : IFileMonitorService
    {
        private readonly IServiceProvider _serviceProvider;
        private CancellationTokenSource _cts;
        public event Action<string> OnStatusChanged;

        public FileMonitorService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MonitorFilesAsync(int monitoringInterval, string directoryPath, Action<List<TradeData>> onDataLoaded)
        {
            OnStatusChanged?.Invoke("Monitoring started...");
            _cts = new CancellationTokenSource();

            try
            {
                // Process existing files initially
                await ProcessFilesAsync(directoryPath, onDataLoaded);

                // Loop to enable continuous file monitoring
                while (!_cts.Token.IsCancellationRequested)
                {
                    await ProcessFilesAsync(directoryPath, onDataLoaded);
                    await Task.Delay(TimeSpan.FromSeconds(monitoringInterval), _cts.Token);
                }
            }
            catch (TaskCanceledException)
            {
                OnStatusChanged?.Invoke("Monitoring canceled.");
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Error: {ex.Message}");
            }
        }

        private async Task ProcessFilesAsync(string directoryPath, Action<List<TradeData>> onDataLoaded)
        {
            // Retrieve all files in the specified directory
            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                try
                {
                    var ext = Path.GetExtension(file).ToLower();
                    IFileLoader fileLoader = ext switch
                    {
                        ".csv" => _serviceProvider.GetRequiredService<CSVLoader>(),
                        ".txt" => _serviceProvider.GetRequiredService<TXTLoader>(),
                        ".xml" => _serviceProvider.GetRequiredService<XMLLoader>(),
                        _ => throw new InvalidOperationException($"Unsupported file type: {ext}")
                    };

                    // Load data from the file and call onDataLoaded callback
                    var trades = await fileLoader.LoadDataAsync(file);
                    onDataLoaded(trades);

                    File.Delete(file);
                }
                catch (Exception ex)
                {

                    OnStatusChanged?.Invoke($"Error processing file {file}: {ex.Message}");
                }
            }
        }


        public void StopMonitoring()
        {
            _cts?.Cancel();
            OnStatusChanged?.Invoke("Monitoring stopped.");
        }
    }
}
