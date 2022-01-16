using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Transact.Data.Models.Common;

namespace Transact.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static AppSettings ConfigureBusinessServices(this IServiceCollection services, IConfiguration configuration, string AssemblyPath)
        {

            var appSettingsSection = configuration.GetSection(nameof(AppSettings));
            if (appSettingsSection == null)
                throw new Exception("No appsettings section has been found");

            AppSettings appSettings = new AppSettings();
            appSettingsSection.Bind(appSettings);

            var errors = appSettings.IsValid();
            if (errors.Count != 0)
            {
                Debug.WriteLine("Errors in Appsetting file, Error are  : " + JsonConvert.SerializeObject(errors));
                throw new Exception("No valid app settings. Error are  : " + JsonConvert.SerializeObject(errors));
            }

            services.AddSingleton(appSettings);


            return appSettings;
        }
    }
}
