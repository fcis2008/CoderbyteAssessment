using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Model;

namespace BusinessLogicLayer.AutoMapperConfig
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<NewUserDTO, User>();
        }
    }
}
