using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;

using JetBrains.Annotations;

using TournamentTracker.Data.Models;
using TournamentTracker.Infrastructure.Commands.Tournaments;
using TournamentTracker.Infrastructure.Queries.Tournaments;

namespace TournamentTracker.Infrastructure.MappingProfiles
{
    [UsedImplicitly]
    public class TournamentMappingProfile : Profile
    {
        public TournamentMappingProfile()
        {
            CreateMap<CreateTournament.Request, Tournament>()
                .ForMember(dest=>dest.Id, opt=>opt.Ignore());

            CreateMap<UpdateTournament.Request, Tournament>(MemberList.Source);

            CreateMap<Tournament, GetTournaments.Model>();
        }

    }
}
