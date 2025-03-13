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

            try
            {
                var lines = await Task.Run(() => File.ReadLines(filePath).ToList());

                if (!lines.Any())
                    throw new Exception("The file is empty");

                if (lines[0].Contains("Date") && lines[0].Contains("Open"))
                    lines = lines.Skip(1).ToList();

                foreach (var line in lines)
                {
                    var parts = line.Split(';');

                    if (parts.Length >= 6)
                    {
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
                    else
                        throw new Exception("There is not enough information in the text file.");

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error CSV file: {ex.Message}");
            }

            return trades;
        }
    }

}
