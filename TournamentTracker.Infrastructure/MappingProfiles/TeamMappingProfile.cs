﻿using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;

using JetBrains.Annotations;

using TournamentTracker.Data.Models;
using TournamentTracker.Infrastructure.Commands.Teams;

namespace TournamentTracker.Infrastructure.MappingProfiles
{
    [UsedImplicitly]
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<CreateTeam.Request, Team>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(x => DateTime.Now));

            CreateMap<UpdateTeam.Request, Team>(MemberList.Source)
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(x => DateTime.Now));

        }

    }
}
