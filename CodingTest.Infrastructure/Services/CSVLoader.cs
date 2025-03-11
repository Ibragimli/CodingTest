using CodingTest.Application.Abstractions.Services;
using CodingTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Infrastructure.Services
{
    public class CSVLoader : IFileLoader
    {
        public async Task<List<TradeData>> LoadDataAsync(string filePath)
        {
            var trades = new List<TradeData>();

            // Faylı asinxron oxuyuruq
            var lines = await Task.Run(() => File.ReadLines(filePath).Skip(1)); // Faylı asinxron oxumaq üçün Task.Run istifadə edilir

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                trades.Add(new TradeData
                {
                    Date = DateTime.Parse(parts[0]),
                    Open = double.Parse(parts[1]),
                    High = double.Parse(parts[2]),
                    Low = double.Parse(parts[3]),
                    Close = double.Parse(parts[4]),
                    Volume = int.Parse(parts[5])
                });
            }

            return trades;
        }
    }
}
