using CodingTest.Application.Abstractions.Services;
using CodingTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodingTest.Infrastructure.Services
{
    public class XMLLoader : IFileLoader
    {
        public async Task<List<TradeData>> LoadDataAsync(string filePath)
        {
            var trades = new List<TradeData>();

            try
            {
                var xml = await Task.Run(() => XElement.Load(filePath));

                foreach (var element in xml.Elements("value"))
                {
                    trades.Add(new TradeData
                    {
                        Date = DateTime.Parse(element.Attribute("date")?.Value ?? throw new Exception("Date attribute is missing")),
                        Open = double.Parse(element.Attribute("open")?.Value ?? throw new Exception("Open attribute is missing")),
                        High = double.Parse(element.Attribute("high")?.Value ?? throw new Exception("High attribute is missing")),
                        Low = double.Parse(element.Attribute("low")?.Value ?? throw new Exception("Low attribute is missing")),
                        Close = double.Parse(element.Attribute("close")?.Value ?? throw new Exception("Close attribute is missing")),
                        Volume = int.Parse(element.Attribute("volume")?.Value ?? throw new Exception("Volume attribute is missing"))
                    });
                }

                if (!trades.Any())
                    throw new Exception("No trades found in the XML file");

            }
            catch (Exception ex)
            {
                throw new Exception($"Error XML file: {ex.Message}");
            }

            return trades;
        }
    }

}
