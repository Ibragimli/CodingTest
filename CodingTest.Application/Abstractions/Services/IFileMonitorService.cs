using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Application.Abstractions.Services
{
    public interface IFileMonitorService
    {
        Task MonitorFilesAsync();
    }
}
