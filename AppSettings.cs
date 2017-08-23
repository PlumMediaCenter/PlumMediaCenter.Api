using System.IO;

namespace PlumMediaCenter
{
    public class AppSettings
    {
        /// <summary>
        /// The path to the path to the folder where the posters should live. Includes trailing slash
        /// </summary>
        /// <returns></returns>
        public string PosterFolderPath
        {
            get
            {
                var slash = Path.DirectorySeparatorChar;
                return $"{Directory.GetCurrentDirectory()}{slash}wwwroot{slash}posters{slash}";
            }
        }
        
        public string BackdropFolderPath
        {
            get
            {
                var slash = Path.DirectorySeparatorChar;
                return $"{Directory.GetCurrentDirectory()}{slash}wwwroot{slash}backdrops{slash}";
            }
        }
        public string TmdbApiString
        {
            get
            {
                return "90dbc17887e30eae3095d213fa803190";
            }
        }
    }
}