using AutoMapper;
using Transact.Data.Models;
using Transact.Data.ViewModels;

namespace Transact.Data.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Role, RoleVM>();
            CreateMap<Tenant, TenantVM>();
            CreateMap<UserAccount, UserVM>();
            CreateMap<Widget, WidgetVM>();
        }
    }
}
