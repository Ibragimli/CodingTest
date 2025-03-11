using CodingTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Application.Abstractions.Services
{
    public interface IFileLoader
    {
        Task<List<TradeData>> LoadDataAsync(string filePath);
    }
}
