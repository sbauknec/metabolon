namespace metabolon.Profiles;

using AutoMapper;
using metabolon.DTOs;
using metabolon.Models;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();
    }
}