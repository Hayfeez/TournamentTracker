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
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdatePlayer.Request, Team>(MemberList.Source);


            CreateMap<AssignPlayersToTeam.Request, TeamPlayer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<CreatePlayer.Request, TeamPlayer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Player, GetPlayerById.Model>();
            CreateMap<Player, GetPlayers.Model>();
        }

    }
}
