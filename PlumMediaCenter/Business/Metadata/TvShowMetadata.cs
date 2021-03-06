using System.Collections.Generic;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;

namespace PlumMediaCenter.Business.Metadata
{
    public class TvShowMetadata
    {
        public TvShowMetadata()
        {

        }
        /// <summary>
        /// Create a new object based on an existing object. This is only a shallow clone, but only currently exists so that
        /// we can write the file to disc without serializing additional properties from child objects
        /// </summary>
        /// <param name="tvShowMetadata"></param>
        public TvShowMetadata(TvShowMetadata tvShowMetadata)
        {
            var t = this.GetType();
            var properties = t.GetProperties();
            foreach (var prop in properties)
            {
                var incomingValue = prop.GetValue(tvShowMetadata);
                prop.SetValue(this, incomingValue);
            }
        }
        /// <summary>
        /// The title of the tv show
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The title to use to sort the tv show with. If omitted, Title should be used
        /// </summary>
        /// <returns></returns>
        public string SortTitle { get; set; }
        /// <summary>
        /// Additional phrases to use for searching. For example, sometimes people spell Dalmatians with an o "Dalmations", 
        /// so adding "101 Dalmations" into this list would help with that search
        /// </summary>
        public List<string> ExtraSearchText { get; set; } = new List<string>();
        /// <summary>
        /// A short summary of the tv show
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// A lengthier description of the tv show
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of the collection that this video belongs to (i.e. 'Star Trek', 'Die Hard').
        /// If the tv show is not in a collection, this will be null
        /// </summary>
        public List<string> Genres { get; set; } = new List<string>();
        /// <summary>
        /// A fine-grained list of things related to the tv show. Kind of like sub-genres
        /// </summary>
        public List<string> Keywords { get; set; } = new List<string>();
        /// <summary>
        /// The MPAA rating (G, PG, R, etc...)
        /// </summary>
        public string Rating { get; set; }

        /// <summary>
        /// The date this tv show was released
        /// </summary>
        public int? ReleaseYear { get; set; }

        /// <summary>
        /// The normal runtime of each episode of this show
        /// </summary>
        public int? RuntimeSeconds { get; set; }
        /// <summary>
        /// The TMDB ID for this tv show. Null if tv show is not on TMDB
        /// </summary>
        public int? TmdbId { get; set; }
        /// <summary>
        /// A list of all actors in the film
        /// </summary>
        public List<CastMember> Cast { get; set; } = new List<CastMember>();
        /// <summary>
        /// A list of crew members who worked on the film (directors, writers)
        /// </summary>
        /// <returns></returns>
        public List<CrewMember> Crew { get; set; } = new List<CrewMember>();

        /// <summary>
        /// A collection of posters. Each item is relative to the root folder, and should use linux slashes
        /// </summary>
        /// <returns></returns>
        public List<Image> Backdrops { get; set; } = new List<Image>();

        public void AddCast(List<Cast> cast)
        {
            if (cast == null)
            {
                return;
            }
            foreach (var member in cast)
            {
                this.Cast.Add(new CastMember
                {
                    Character = member.Character,
                    Name = member.Name,
                    TmdbId = member.Id
                });
            }
        }
        public void AddCrew(List<Crew> crew)
        {
            if (crew == null)
            {
                return;
            }
            foreach (var member in crew)
            {
                this.Crew.Add(new CrewMember
                {
                    Job = member.Job,
                    Name = member.Name,
                    TmdbId = member.Id
                });
            }
        }
    }
}