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
    //XMLLoader.cs - XML faylları oxuyur
    public class XMLLoader : IFileLoader
    {
        public async Task<List<TradeData>> LoadDataAsync(string filePath)
        {
            var trades = new List<TradeData>();

            // Asinxron olaraq XML faylını yükləyirik
            var xml = await Task.Run(() => XElement.Load(filePath));

            // XML elementləri arasında dövr edirik
            foreach (var element in xml.Elements("value"))
            {
                // Hər bir elementi işləyirik və trade məlumatını siyahıya əlavə edirik
                trades.Add(new TradeData
                {
                    Date = DateTime.Parse(element.Attribute("date").Value),
                    Open = double.Parse(element.Attribute("open").Value),
                    High = double.Parse(element.Attribute("high").Value),
                    Low = double.Parse(element.Attribute("low").Value),
                    Close = double.Parse(element.Attribute("close").Value),
                    Volume = int.Parse(element.Attribute("volume").Value)
                });
            }
            return trades;
        }
    }
}
