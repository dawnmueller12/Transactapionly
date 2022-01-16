using AutoMapper;
using Transact.Data.Models;
using Transact.Data.ViewModels;

namespace Transact.Data.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<RoleVM, Role>();
            CreateMap<TenantVM, Tenant>();
            CreateMap<UserVM, UserAccount>();
            CreateMap<WidgetVM, Widget>();
        }
    }
}
