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
                .ForMember(dest=>dest.Id, opt=>opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(x => DateTime.Now));

            CreateMap<UpdateTournament.Request, Tournament>(MemberList.Source)
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(x => DateTime.Now));

            CreateMap<Tournament, GetTournaments.Model>();
            CreateMap<Tournament, GetTournamentById.Model>();


            CreateMap<TournamentPrize, GetTournamentPrizes.Model>();
            CreateMap<TournamentPrize, GetTournamentPrizeById.Model>();

            CreateMap<TournamentGroup, GetTournamentGroups.Model>();
            CreateMap<TournamentGroup, GetTournamentGroupById.Model>();

            CreateMap<TournamentTeam, GetTournamentTeams.Model>();
            CreateMap<TournamentTeam, GetTournamentTeamById.Model>();

            CreateMap<TournamentTeamGroup, GetTournamentTeamGroups.Model>();

            CreateMap<TournamentRound, GetTournamentRounds.Model>();
        }

    }
}
