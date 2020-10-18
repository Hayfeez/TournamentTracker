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
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src=>src.Email.ToLower()));

            CreateMap<UpdateUser.Request, Team>(MemberList.Source);


            CreateMap<AssignUsersToAccount.Request, UserAccount>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<CreateUser.Request, UserAccount>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<User, GetUserById.Model>();
        }

    }
}
