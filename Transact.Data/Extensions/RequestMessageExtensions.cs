using System.Net.Http;
using Transact.Data.Abstractions.Repository;

namespace Transact.Data.Extensions
{
    public static class RequestMessageExtensions
    {

        internal static IEntityBaseRepository<T> GetDataRepository<T>(this HttpRequestMessage request) where T : class, new()
        {
            return request.GetService<IEntityBaseRepository<T>>();
        }

        private static TService GetService<TService>(this HttpRequestMessage request)
        {
            TService service = (TService)request.GetService<TService>();

            return service;
        }
    }
}
