using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using PlumMediaCenter.Data;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using PlumMediaCenter.Business;
using PlumMediaCenter.Business.MetadataProcessing;
using PlumMediaCenter.Attributues;

namespace PlumMediaCenter.Controllers
{
    [ExceptionHandlerFilter]
    [Route("api/[controller]")]
    public class MetadataController : Controller
    {
        private Manager _Manager;
        public Manager Manager
        {
            get
            {
                if (_Manager == null)
                {
                    _Manager = new Manager();
                }
                return _Manager;
            }
        }

        [HttpGet("raw")]
        public async Task<object> GetRawTmdb([FromQuery]int tmdbId)
        {
            var client = new TMDbClient(new AppSettings().TmdbApiString);

            var movie = await client.GetMovieAsync(tmdbId,
                        MovieMethods.AlternativeTitles |
                        MovieMethods.Credits |
                        MovieMethods.Images |
                        MovieMethods.Keywords |
                        MovieMethods.Releases |
                        MovieMethods.ReleaseDates |
                        MovieMethods.Videos
                   );
            return movie;
        }

        /// <summary>
        /// Get a list of all movies that match the search text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet("movies/search")]
        public async Task<List<MovieSearchResult>> GetMovieSearchResults([FromQuery]string text, [FromQuery]int tmdbId)
        {
            var fetcher = new MovieMetadataProcessor();
            return await fetcher.GetSearchResults(text);
        }


        /// <summary>
        /// Retrieves full metadata information from tmdb, and also all current metadata information from the filesystem
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet("movies/compare")]
        public async Task<MovieMetadataComparison> CompareMetadata([FromQuery]int tmdbId, int movieId)
        {
            var processor = new MovieMetadataProcessor();
            return await processor.GetComparison(tmdbId, movieId);
        }

        [HttpPost("movies/{movieId}")]
        public async Task SaveMetadata(int movieId, [FromBody] MovieMetadata metadata)
        {
            var processor = new MovieMetadataProcessor();
            await processor.Save(movieId, metadata);
        }
    }
}
