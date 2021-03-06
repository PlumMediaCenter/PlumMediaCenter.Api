using System;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Http;
using PlumMediaCenter.Business;

namespace PlumMediaCenter
{
    public class AppSettings
    {
        public AppSettings(
        )
        {
        }
        public static HttpContextAccessor HttpContextAccessor;

        public string ApiUrl { get; set; }
        public int BCryptWorkFactor { get; set; }
        public string DbHost { get; set; }
        public string DbName { get; set; }
        public string DbUsername { get; set; }
        public string DbPassword { get; set; }

        /// <summary>
        /// The full path to the search indexes directory, excluding trailing slash
        /// </summary>
        /// <returns></returns>
        public string SearchIndexesDirectoryPath
        {
            get
            {
                return $"{Directory.GetCurrentDirectory()}/search-indexes";
            }
        }

        /// <summary>
        /// Full path to the tmdb cache directory, excluding trailing slash
        /// </summary>
        /// <returns></returns>
        public string TmdbCacheDirectoryPath
        {
            get
            {
                return $"{Directory.GetCurrentDirectory()}/temp/tmdb-cache";
            }
        }

        /// <summary>
        /// The path to the path to the folder where all media item images should live. Includes trailing slash.
        /// </summary>
        /// <returns></returns>
        public string ImageFolderPath
        {
            get
            {
                return $"{Directory.GetCurrentDirectory()}/wwwroot/img";
            }
        }

        public string GetImageFolderUrl()
        {
            return GetImageFolderUrlStatic();
        }


        public static string GetImageFolderUrlStatic()
        {
            return $"{GetBaseUrlStatic()}/img";
        }

        public static string TempPath
        {
            get
            {
                var path = Utility.NormalizePath($"{Directory.GetCurrentDirectory()}/temp/", false);
                //make sure the temp folder path exists
                Directory.CreateDirectory(path);
                return path;
            }
        }
        public string TmdbApiString = "90dbc17887e30eae3095d213fa803190";

        /// <summary>
        /// The total number of seconds of wiggle room between media item progress before it creates a new progress record.
        /// For example, when you are watching a movie, every n seconds a new progress event will be sent. Those are consecutive, and because 
        /// the amount of progress from last entry to this entry equals the difference in thime, that gap would be roughly zero.
        /// Now imagine the user pauses the movie for 5 minutes for a snack break. The next progress gap will be roughly 5 minutes. This is still 
        /// the same viewing session, so we need to account for the max size of a gap before creating a new progress record.
        /// </summary>
        public int MaxMediaProgressGapSeconds = 20;

        /// <summary>
        /// The percentage of a video that must be watched before being consered watched.
        /// This value is used whenever a media item does not explicitly indicate a CompletionSeconds value
        /// </summary>
        public int CompletionPercentage
        {
            get
            {
                return CompletionPercentageStatic;
            }
        }

        /// <summary>
        /// The percentage of a video that must be watched before being consered watched.
        /// This value is used whenever a media item does not explicitly indicate a CompletionSeconds value
        /// </summary>
        public static int CompletionPercentageStatic = 95;

        public string GetBaseUrl()
        {
            return GetBaseUrlStatic();
        }

        private static ThreadLocal<string> OverriddenBaseUrl = new ThreadLocal<string>(() =>
        {
            return null;
        });
        public static void SetBaseUrlStatic(string baseUrl)
        {
            OverriddenBaseUrl.Value = baseUrl;
        }

        /// <summary>
        /// Get the base url statically. This will still derive the value from an instance of AppSettings and HttpContext,
        /// so only call
        /// </summary>
        /// <returns></returns>
        public static string GetBaseUrlStatic()
        {
            if (OverriddenBaseUrl.Value != null)
            {
                return OverriddenBaseUrl.Value;
            }

            if (HttpContextAccessor.HttpContext == null)
            {
                throw new Exception("Unable to determine base url because current thread does not have an associated HttpContext");
            }
            var ctx = HttpContextAccessor.HttpContext;
            var store = ctx.Items;
            if (store.ContainsKey("baseUrl") == false)
            {
                var request = ctx.Request;
                string url;
                //if there is an original url header (sent from a reverse proxy), use that
                if (request.Headers.ContainsKey("X-ORIGINAL-URL"))
                {
                    url = request.Headers["X-ORIGINAL-URL"];
                }
                else
                {
                    url = $"{request.Scheme}://{request.Host}{request.Path}";
                }


                //remove anything after and including /graphql
                var baseUrl = url.Substring(0, url.ToLowerInvariant().IndexOf("/graphql") + 1);
                baseUrl = string.IsNullOrWhiteSpace(baseUrl) ? url.Substring(0, url.ToLowerInvariant().IndexOf("/api") + 1) : baseUrl;

                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    throw new Exception("Unable to calculate base url");
                }
                store["baseUrl"] = baseUrl;
            }
            return (string)store["baseUrl"];
        }
    }
}