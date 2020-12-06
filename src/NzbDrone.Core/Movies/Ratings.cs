using NzbDrone.Core.Datastore;

namespace NzbDrone.Core.Movies
{
    public class Ratings : IEmbeddedDocument
    {
        public int Votes { get; set; }
        public decimal Value { get; set; }
        public RatingOrigin Origin { get; set; }
        public RatingType Type { get; set; }
    }

    public enum RatingOrigin
    {
        Tmdb,
        Imdb,
        Trakt,
        Metacritic,
        RottenTomatos
    }

    public enum RatingType
    {
        User,
        Critic
    }
}
