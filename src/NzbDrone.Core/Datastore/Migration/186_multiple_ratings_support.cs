using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using Dapper;
using FluentMigrator;
using NzbDrone.Core.Datastore.Migration.Framework;

namespace NzbDrone.Core.Datastore.Migration
{
    [Migration(186)]
    public class multiple_ratings_support : NzbDroneMigrationBase
    {
        private readonly JsonSerializerOptions _serializerSettings;

        public multiple_ratings_support()
        {
            _serializerSettings = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                IgnoreNullValues = false,
                PropertyNameCaseInsensitive = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        protected override void MainDbUpgrade()
        {
            Execute.WithConnection((conn, tran) => FixRatings(conn, tran, "Movies"));
            Execute.WithConnection((conn, tran) => FixRatings(conn, tran, "ImportListMovies"));
        }

        private void FixRatings(IDbConnection conn, IDbTransaction tran, string table)
        {
            var rows = conn.Query<Movie185>($"SELECT Id, Ratings FROM {table}");

            var corrected = new List<Movie186>();

            foreach (var row in rows)
            {
                var oldRatings = JsonSerializer.Deserialize<Ratings185>(row.Ratings, _serializerSettings);

                var newRatings = new List<Ratings186>
                {
                    new Ratings186
                    {
                        Votes = oldRatings.Votes,
                        Value = oldRatings.Value,
                        Provider = RatingOrigin186.Tmdb,
                        Type = RatingType186.User
                    }
                };

                corrected.Add(new Movie186
                {
                    Id = row.Id,
                    Ratings = JsonSerializer.Serialize(newRatings, _serializerSettings)
                });
            }

            var updateSql = $"UPDATE {table} SET Ratings = @Ratings WHERE Id = @Id";
            conn.Execute(updateSql, corrected, transaction: tran);
        }

        private class Movie185
        {
            public int Id { get; set; }
            public string Ratings { get; set; }
        }

        private class Ratings185
        {
            public int Votes { get; set; }
            public decimal Value { get; set; }
        }

        private class Movie186
        {
            public int Id { get; set; }
            public string Ratings { get; set; }
        }

        private class Ratings186
        {
            public int Votes { get; set; }
            public decimal Value { get; set; }
            public RatingOrigin186 Provider { get; set; }
            public RatingType186 Type { get; set; }
        }

        private enum RatingOrigin186
        {
            Tmdb
        }

        private enum RatingType186
        {
            User
        }
    }
}
