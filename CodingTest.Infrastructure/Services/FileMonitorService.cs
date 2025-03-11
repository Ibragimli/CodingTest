using CodingTest.Application.Abstractions.Services;
using CodingTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Infrastructure.Services
{
    public class FileMonitorService : IFileMonitorService
    {
        private readonly IFileLoader _fileLoader;
        private readonly string _directoryPath;
        private readonly int _monitoringInterval;
        private readonly Dictionary<string, IFileLoader> _loaders;
        private readonly Action<List<TradeData>> _onDataLoaded;

        public FileMonitorService(IFileLoader fileLoader, string directoryPath, int monitoringInterval, Action<List<TradeData>> onDataLoaded)
        {
            _fileLoader = fileLoader ?? throw new ArgumentNullException(nameof(fileLoader));
            _directoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
            _monitoringInterval = monitoringInterval;
            _onDataLoaded = onDataLoaded;
            _loaders = new Dictionary<string, IFileLoader>
            {
                { ".csv", new CSVLoader() },
                { ".txt", new TXTLoader() },
                { ".xml", new XMLLoader() }
            };
        }

        public async Task MonitorFilesAsync()
        {
            try
            {
                // Fayl sistemindəki bütün faylları götürürük
                var files = Directory.GetFiles(_directoryPath);

                foreach (var file in files)
                {
                    var ext = Path.GetExtension(file);

                    // Faylın növü üzrə uyğun loader tapırıq
                    if (_loaders.ContainsKey(ext))
                    {
                        // Yükləmə prosesini başlatmaq üçün asinxron metod istifadə edirik
                        var trades = await _loaders[ext].LoadDataAsync(file);
                        _onDataLoaded(trades);  // Yüklənmiş məlumatı geri çağırmaq
                        File.Delete(file);  // Faylı sildik
                    }
                }

                // Faylları mütəmadi olaraq monitorinq etməyə davam edirik
                while (true)
                {
                    // Yeni faylları yoxlayırıq
                    foreach (var file in Directory.GetFiles(_directoryPath))
                    {
                        var ext = Path.GetExtension(file);
                        if (_loaders.ContainsKey(ext))
                        {
                            var trades = await _loaders[ext].LoadDataAsync(file);
                            _onDataLoaded(trades);  // Yüklənmiş məlumatı geri çağırmaq
                            File.Delete(file);  // Faylı sildik
                        }
                    }
                    await Task.Delay(_monitoringInterval);  // Monitorinq intervalı
                }
            }
            catch (Exception ex)
            {
                // Xətaları idarə etmək üçün
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
