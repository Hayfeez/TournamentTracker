using System;
using System.Collections.Generic;

using TournamentTracker.Common.Helpers;
using TournamentTracker.Data.Models;

namespace TournamentTracker.Infrastructure.Helpers
{
    public interface IRandomizeHelper
    {
        List<TournamentTeamGroup> AddTeamsToGroups(Guid accountId, Guid tournamentId, List<Guid> teamIds, List<Guid> groupIds, int teamPerGroup, int noOfGroupsNeeded);

        List<TournamentTeamGroup> AddTeamsToAllGroup(Guid accountId, Guid tournamentId, List<Guid> teamIds, List<Guid> groupIds, int teamPerGroup, int noOfGroupsNeeded);
    }

    public class RandomizeHelper : IRandomizeHelper
    {
        public List<TournamentTeamGroup> AddTeamsToAllGroup(Guid accountId, Guid tournamentId, List<Guid> teamIds, List<Guid> groupIds, int teamPerGroup, int noOfGroupsNeeded)
        {
            var teamGroups = new List<TournamentTeamGroup>();
            Random groupRand = new Random(DateTime.Now.ToString().GetHashCode());
            Random teamRand = new Random(DateTime.Now.ToString().GetHashCode());

            //this will add all teams to a group before moving to the next group
            for (int i = 0; i < noOfGroupsNeeded; i++)
            {
                var groupIndex = groupRand.Next(0, groupIds.Count);
                var groupId = groupIds[groupIndex];

                for (int j = 0; j < teamPerGroup; j++)
                {
                    var teamIndex = teamRand.Next(0, teamIds.Count);
                    var teamId = teamIds[teamIndex];

                    teamGroups.Add(new TournamentTeamGroup
                    {
                        AccountId = accountId,
                        Id = SequentialGuid.Create(),
                        TournamentId = tournamentId,
                        TournamentGroupId = groupId,
                        TournamentTeamId = teamId
                    });

                    teamIds.RemoveAt(teamIndex);
                }

                groupIds.RemoveAt(groupIndex);
            }

            return teamGroups;
        }

        //todo: incomplete implementation
        public List<TournamentTeamGroup> AddTeamsToGroups(Guid accountId, Guid tournamentId, List<Guid> teamIds, List<Guid> groupIds, int teamPerGroup, int noOfGroupsNeeded)
        {
            var teamGroups = new List<TournamentTeamGroup>();
            Random groupRand = new Random(DateTime.Now.ToString().GetHashCode());
            Random teamRand = new Random(DateTime.Now.ToString().GetHashCode());

            for (int i = 0; i < noOfGroupsNeeded; i++)
            {
                var groupIndex = groupRand.Next(0, groupIds.Count);
                var groupId = groupIds[groupIndex];

                var teamIndex = teamRand.Next(0, teamIds.Count);
                var teamId = teamIds[teamIndex];

               teamGroups.Add(new TournamentTeamGroup
                {
                    AccountId = accountId,
                    Id = SequentialGuid.Create(),
                    TournamentId = tournamentId,
                    TournamentGroupId = groupId,
                    TournamentTeamId = teamId
                });

                teamIds.RemoveAt(teamIndex);

                //todo: find out how to remove the group if no of teams is reached
                //  groupIds.RemoveAt(groupIndex);
            }

            return teamGroups;
        }


    }
}