namespace metabolon.Profiles;

using System.Reflection.PortableExecutable;
using AutoMapper;
using metabolon.DTOs;
using metabolon.Models;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();

        CreateMap<Room, RoomDTO>();
        CreateMap<RoomDTO, Room>();

        CreateMap<Device, DeviceDTO>();
        CreateMap<DeviceDTO, Device>();

        CreateMap<Item, ItemDTO>();
        CreateMap<ItemDTO, Item>();

        CreateMap<Document, DocumentDTO>();
        CreateMap<DocumentDTO, Document>();
    }
}