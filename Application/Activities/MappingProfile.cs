using AutoMapper;
using Domain;

namespace Application.Activities
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //automapper is convention based; it knows if the props are same name it will map to it
            CreateMap<Activity, ActivityDto>();
            //for member is used to get us the username and display name back
            //o means options
            CreateMap<UserActivity, AttendeeDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName));
        }
    }
}