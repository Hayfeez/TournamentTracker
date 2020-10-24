using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;

using JetBrains.Annotations;

using TournamentTracker.Data.Models;
using TournamentTracker.Infrastructure.Commands.Users;
using TournamentTracker.Infrastructure.Queries.Users;

namespace TournamentTracker.Infrastructure.MappingProfiles
{
    [UsedImplicitly]
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<CreateUser.Request, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src=>src.Email.ToLower()))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(x => DateTime.Now));

            CreateMap<UpdateUser.Request, Team>(MemberList.Source)
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(x => DateTime.Now));


            CreateMap<CreateUser.Request, UserAccount>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsPending, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(x => DateTime.Now));

            CreateMap<User, GetUserById.Model>();

            CreateMap<User, GetUsers.Model>();

            CreateMap<UserAccount, GetUserAccount.Model>();
        }

    }
}
