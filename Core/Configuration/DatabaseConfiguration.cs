using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration
{
    public static class DatabaseConfiguration
    {
        public static string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../WebAPI"));
                configurationManager.AddJsonFile("appsettings.json");
                return configurationManager.GetConnectionString("Default");
            }
        }
    }
}
