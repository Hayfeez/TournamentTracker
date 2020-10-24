using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;

using JetBrains.Annotations;

using TournamentTracker.Data.Models;
using TournamentTracker.Infrastructure.Commands.Players;
using TournamentTracker.Infrastructure.Queries.Players;

namespace TournamentTracker.Infrastructure.MappingProfiles
{
    [UsedImplicitly]
    public class PlayerMappingProfile : Profile
    {
        public PlayerMappingProfile()
        {
            CreateMap<CreatePlayer.Request, Player>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(x => DateTime.Now));

            CreateMap<UpdatePlayer.Request, Team>(MemberList.Source)
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(x => DateTime.Now));


            CreateMap<AssignPlayersToTeam.Request, TeamPlayer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(x => DateTime.Now));

            CreateMap<CreatePlayer.Request, TeamPlayer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(x => DateTime.Now));

            CreateMap<Player, GetPlayerById.Model>();
            CreateMap<Player, GetPlayers.Model>();
        }

    }
}
