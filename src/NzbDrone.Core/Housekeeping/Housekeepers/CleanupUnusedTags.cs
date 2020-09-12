using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using NzbDrone.Core.Datastore;

namespace NzbDrone.Core.Housekeeping.Housekeepers
{
    public class CleanupUnusedTags : IHousekeepingTask
    {
        private readonly IMainDatabase _database;

        public CleanupUnusedTags(IMainDatabase database)
        {
            _database = database;
        }

        public void Clean()
        {
            using (var mapper = _database.OpenConnection())
            {
                var usedTags = new[] { "Movies", "Notifications", "DelayProfiles", "Restrictions", "ImportLists" }
                    .SelectMany(v => GetUsedTags(v, mapper))
                    .Distinct()
                    .ToArray();

                var usedTagsList = string.Join(",", usedTags.Select(d => d.ToString()).ToArray());

                var cleanLibraryTags = mapper.Query<string>($"SELECT Value FROM Config WHERE Config.Key='cleanlibrarytags'");

                foreach (var t1 in cleanLibraryTags)
                {
                    var cleanLibraryTagsList = string.Empty;
                    if (!(t1.Equals(string.Empty) || t1.Equals("[]")))
                    {
                        cleanLibraryTagsList = string.Join(",", Array.ConvertAll(t1.Replace("[", "").Replace("]", "").Split(' '), s => int.Parse(s)));
                    }

                    usedTagsList = usedTagsList + cleanLibraryTagsList;
                }

                mapper.Execute($"DELETE FROM Tags WHERE NOT Id IN ({usedTagsList})");
            }
        }

        private int[] GetUsedTags(string table, IDbConnection mapper)
        {
            return mapper.Query<List<int>>($"SELECT DISTINCT Tags FROM {table} WHERE NOT Tags = '[]'")
                .SelectMany(x => x)
                .Distinct()
                .ToArray();
        }
    }
}
