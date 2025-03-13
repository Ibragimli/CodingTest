using CodingTest.Application.Abstractions.Services;
using CodingTest.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<CSVLoader>();
            services.AddScoped<TXTLoader>();
            services.AddScoped<XMLLoader>();

            services.AddScoped<IFileLoader, CSVLoader>();
            services.AddScoped<IFileLoader, TXTLoader>();
            services.AddScoped<IFileLoader, XMLLoader>();

            services.AddScoped<IFileMonitorService, FileMonitorService>();
            services.AddScoped<IFileExporter, FileExporter>();
        }
    }

}
