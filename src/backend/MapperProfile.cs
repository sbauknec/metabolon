namespace metabolon.Profiles;

using System.Reflection.PortableExecutable;
using AutoMapper;
using metabolon.DTOs;
using metabolon.Models;

//Mapping Profile
//Hier drin steht welche DTO <-> Model Transformationen möglich sein sollen
//Am Beispiel 'User'
//1. Das Model muss zu Export Zwecken in ein UserDTO umgewandelt werden können
//2. Das UserDTO muss intern zu Speicher Zwecken in das Model umgewandelt werden können
//3. Das CreateDTO muss zu Anlage zwecken in das Model umgewandelt werden können
//4. Das Model muss zu geschachtelten Export Zwecken in ein QueryDTO umgewandelt werden können
//Diese semantische Benennung zieht sich durch alle Models und DTOs 
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();
        CreateMap<UserCreateDTO, User>();
        CreateMap<User, UserQueryDTO>();
        CreateMap<UserPutDTO, User>();

        CreateMap<Room, RoomDTO>();
        CreateMap<RoomDTO, Room>();
        CreateMap<RoomCreateDTO, Room>();
        CreateMap<Room, RoomQueryDTO>();

        CreateMap<Device, DeviceDTO>();
        CreateMap<DeviceDTO, Device>();
        CreateMap<DeviceCreateDTO, Device>();
        CreateMap<Device, DeviceQueryDTO>();

        CreateMap<Item, ItemDTO>();
        CreateMap<ItemDTO, Item>();
        CreateMap<ItemCreateDTO, Item>();
        CreateMap<Item, ItemQueryDTO>();

        CreateMap<Document, DocumentDTO>();
        CreateMap<DocumentDTO, Document>();
        CreateMap<DocumentCreateDTO, Document>();
        CreateMap<Document, DocumentQueryDTO>();

        CreateMap<PermissionCreateDTO, Permission>();
        CreateMap<Permission, PermissionDTO>();
    }
}