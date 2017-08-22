using System.Collections.Generic;
using System.Threading.Tasks;
using PlumMediaCenter.Data;

namespace PlumMediaCenter.Business.LibraryGeneration.Managers
{
    public class ShowManager : BaseManager
    {
        public ShowManager(Manager manager) : base(manager)
        {

        }

        /// <summary>
        /// Get a list of every show directory
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetDirectories()
        {
            var task = Task.FromResult(new List<string>());
            return await task;
        }
    }
}