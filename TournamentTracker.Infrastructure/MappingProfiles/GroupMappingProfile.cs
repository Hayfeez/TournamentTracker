using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;

using JetBrains.Annotations;

using TournamentTracker.Data.Models;
using TournamentTracker.Infrastructure.Commands.Groups;
using TournamentTracker.Infrastructure.Queries.Groups;

namespace TournamentTracker.Infrastructure.MappingProfiles
{
    [UsedImplicitly]
    public class GroupMappingProfile : Profile
    {
        public GroupMappingProfile()
        {
            CreateMap<CreateGroup.Request, Group>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateGroup.Request, Group>(MemberList.Source);

            CreateMap<Group, GetGroups.Model>();

            CreateMap<Group, GetGroupById.Result>();

        }

    }
}
