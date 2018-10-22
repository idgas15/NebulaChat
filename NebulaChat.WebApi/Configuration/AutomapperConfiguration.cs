using AutoMapper;
using NebulaChat.Core.Dto;
using NebulaChat.Core.Models;

namespace NebulaChat.WebApi.Configuration
{
    public class AutomapperConfiguration : Profile
    {
        public AutomapperConfiguration()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, ChatUserDto>();
            CreateMap<ChatUserDto, User>();
            CreateMap<Message, SendMessageNotification>()
                .ForMember(dest => dest.Author, obj=> obj.MapFrom<User>(c=> c.Author))
                .ForMember(dest => dest.Recipient, obj=> obj.MapFrom<User>(c=> c.Recipient));
            CreateMap<CreateMessageRequest, Message>();


        }
        
    }
}
