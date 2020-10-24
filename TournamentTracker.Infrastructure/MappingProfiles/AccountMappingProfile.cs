using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;

using JetBrains.Annotations;

using TournamentTracker.Data.Models;
using TournamentTracker.Infrastructure.Commands.Accounts;
using TournamentTracker.Infrastructure.Queries.Users;

namespace TournamentTracker.Infrastructure.MappingProfiles
{
    [UsedImplicitly]
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<CreateAccount.Request, Account>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn,  opt=>opt.MapFrom(x=> DateTime.Now));

            CreateMap<UpdateAccount.Request, Account>(MemberList.Source)
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(x => DateTime.Now));

        }

    }
}
