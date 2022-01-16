using Transact.Data.ViewModels;

namespace Transact.Data.Abstractions.Services
{
    public interface IGraphService
    {
        public CustomResponse CreateUser(UserVM vmObj);
    }
}
