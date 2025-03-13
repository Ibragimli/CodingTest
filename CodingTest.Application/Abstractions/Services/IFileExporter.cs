using CodingTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Application.Abstractions.Services
{
    public interface IFileExporter
    {
        Task ExportDataToFileAsync(List<TradeData> trades, string exportDirectory, string format);

    }
}
