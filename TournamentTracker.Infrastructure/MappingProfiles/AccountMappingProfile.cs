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
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateAccount.Request, Team>(MemberList.Source);

            CreateMap<Account, GetUserAccountById.Model>();
        }

    }
}
