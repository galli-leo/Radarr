using System.Collections.Generic;
using NLog;
using NzbDrone.Core.CustomFormats;
using NzbDrone.Core.DecisionEngine;
using NzbDrone.Core.Download;
using NzbDrone.Core.Parser.Model;
using NzbDrone.Core.Qualities;

namespace NzbDrone.Core.MediaFiles.MovieImport.Specifications
{
    public class UpgradeSpecification : IImportDecisionEngineSpecification
    {
        private readonly ICustomFormatCalculationService _customFormatCalculationService;
        private readonly Logger _logger;

        public UpgradeSpecification(ICustomFormatCalculationService customFormatCalculationService, Logger logger)
        {
            _customFormatCalculationService = customFormatCalculationService;
            _logger = logger;
        }

        public Decision IsSatisfiedBy(LocalMovie localMovie, DownloadClientItem downloadClientItem)
        {
            var qualityComparer = new QualityModelComparer(localMovie.Movie.Profile);
            var preferredWordScore = GetCustomFormatScore(localMovie);

            if (localMovie.Movie.MovieFileId > 0)
            {
                var movieFile = localMovie.Movie.MovieFile;

                if (movieFile == null)
                {
                    _logger.Trace("Unable to get movie file details from the DB. MovieId: {0} MovieFileId: {1}", localMovie.Movie.Id, localMovie.Movie.MovieFileId);
                    return Decision.Accept();
                }

                var qualityCompare = qualityComparer.Compare(localMovie.Quality.Quality, movieFile.Quality.Quality);

                if (qualityCompare < 0)
                {
                    _logger.Debug("This file isn't a quality upgrade for movie. Skipping {0}", localMovie.Path);
                    return Decision.Reject("Not a quality upgrade for existing movie file(s)");
                }

                var customFormats = _customFormatCalculationService.ParseCustomFormat(movieFile);
                var movieFileCustomFormatScore = localMovie.Movie.Profile.CalculateCustomFormatScore(customFormats);

                if (qualityCompare == 0 && preferredWordScore < movieFileCustomFormatScore)
                {
                    _logger.Debug("This file isn't a custom format upgrade for movie. Skipping {0}", localMovie.Path);
                    return Decision.Reject("Not a custom format upgrade for existing movie file(s)");
                }
            }

            return Decision.Accept();
        }

        private int GetCustomFormatScore(LocalMovie localMovie)
        {
            var movie = localMovie.Movie;
            var scores = new List<int>();

            if (localMovie.FileMovieInfo != null)
            {
                var formats = _customFormatCalculationService.ParseCustomFormat(localMovie.FileMovieInfo);
                scores.Add(movie.Profile.CalculateCustomFormatScore(formats));
            }

            if (localMovie.FolderMovieInfo != null)
            {
                var formats = _customFormatCalculationService.ParseCustomFormat(localMovie.FolderMovieInfo);
                scores.Add(movie.Profile.CalculateCustomFormatScore(formats));
            }

            if (localMovie.DownloadClientMovieInfo != null)
            {
                var formats = _customFormatCalculationService.ParseCustomFormat(localMovie.DownloadClientMovieInfo);
                scores.Add(movie.Profile.CalculateCustomFormatScore(formats));
            }

            return scores.MaxOrDefault();
        }
    }
}
