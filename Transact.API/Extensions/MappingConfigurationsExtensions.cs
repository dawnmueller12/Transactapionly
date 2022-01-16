using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using Transact.Data.Mappings;

namespace Transact.API.Extensions
{
    public static class MappingConfigurationsExtensions
    {
        public static void ConfigureMapper(this IServiceCollection services)
        {
            // Automapper Configuration -- https://digitalcontrol.me/blog/mapping-asp-net-core/
            IEnumerable<Assembly> assemblies = new[]
            {
                Assembly.GetAssembly(typeof(DomainToViewModelMappingProfile)),
                Assembly.GetAssembly(typeof(ViewModelToDomainMappingProfile)),
            };
            services.AddAutoMapper(assemblies);
            services.AddAutoMapper(cfg =>
            {
                cfg.AddCollectionMappers();
            });
        }
    }
}
