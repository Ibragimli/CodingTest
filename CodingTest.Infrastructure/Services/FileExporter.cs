using CodingTest.Application.Abstractions.Services;
using CodingTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class FileExporter : IFileExporter
{
    public async Task ExportDataToFileAsync(List<TradeData> trades, string exportDirectory, string format)
    {
        string fileName = "ExportedListData"; 
        string filePath = Path.Combine(exportDirectory, $"{fileName}.{format.ToLower()}");

        if (File.Exists(filePath))
            File.Delete(filePath);  

        await ExportToFileAsync(trades, filePath, format);
    }

    private async Task ExportToFileAsync(List<TradeData> trades, string filePath, string format)
    {
        switch (format.ToLower())
        {
            case "csv":
                await ExportToCsvAsync(trades, filePath);
                break;
            case "txt":
                await ExportToTxtAsync(trades, filePath);
                break;
            case "xml":
                await ExportToXmlAsync(trades, filePath);
                break;
            default:
                throw new InvalidOperationException("Unsupported file format!");
        }
    }

    private async Task ExportToCsvAsync(List<TradeData> trades, string filePath)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Date,Open,High,Low,Volume");

        foreach (var trade in trades)
        {
            sb.AppendLine($"{trade.Date},{trade.Open},{trade.High},{trade.Low},{trade.Volume}");
        }

        await File.WriteAllTextAsync(filePath, sb.ToString());
    }

    private async Task ExportToTxtAsync(List<TradeData> trades, string filePath)
    {
        var sb = new StringBuilder();

        foreach (var trade in trades)
        {
            sb.AppendLine($"Date: {trade.Date} | Open: {trade.Open} | High: {trade.High} | Low: {trade.Low} | Volume: {trade.Volume}");
        }

        await File.WriteAllTextAsync(filePath, sb.ToString());
    }

    private async Task ExportToXmlAsync(List<TradeData> trades, string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<TradeData>));

        using (TextWriter writer = new StreamWriter(filePath))
        {
            await Task.Run(() => serializer.Serialize(writer, trades));
        }
    }
}
