using Transact.Data.Models;

namespace Transact.Data.Abstractions.DBFactory
{
    public interface IDbFactory
    {
        TransactDBContext Init();
    }
}
