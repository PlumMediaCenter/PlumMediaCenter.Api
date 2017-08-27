using System;
using System.Data;

namespace PlumMediaCenter.Business
{
    public class Manager : IDisposable
    {
        public Manager()
        {
            this.Connection = Data.ConnectionManager.GetConnection();
            this.LibraryGeneration = new LibraryGenerationManager(this);
        }
        public IDbConnection Connection;

        public LibraryGenerationManager LibraryGeneration;

        private Utility _Utility;
        public Utility Utility
        {
            get
            {
                return this._Utility = this._Utility != null ? this._Utility : new Utility();
            }
        }

        private AppSettings _AppSettings;
        public AppSettings AppSettings
        {
            get
            {
                return this._AppSettings = this._AppSettings != null ? this._AppSettings : new AppSettings();
            }
        }

        private Managers.MovieManager _Movies;
        public Managers.MovieManager Movies
        {
            get
            {
                return this._Movies = this._Movies != null ? this._Movies : new Managers.MovieManager(this);
            }
        }

        void IDisposable.Dispose()
        {
            try
            {
                this.Connection.Close();
                this.Connection.Dispose();
            }
            catch (Exception)
            {

            }
        }
    }

    public class LibraryGenerationManager : BaseManager
    {

        public LibraryGenerationManager(Manager manager) : base(manager)
        {

        }

        private LibraryGeneration.Managers.SourceManager _Sources;
        public LibraryGeneration.Managers.SourceManager Sources
        {
            get
            {
                return this._Sources = this._Sources != null ? this._Sources : new LibraryGeneration.Managers.SourceManager(this.Manager);
            }
        }

        private LibraryGeneration.Managers.MovieManager _Movies;
        public LibraryGeneration.Managers.MovieManager Movies
        {
            get
            {
                return this._Movies = this._Movies != null ? this._Movies : new LibraryGeneration.Managers.MovieManager(this.Manager);
            }
        }

        private LibraryGeneration.Managers.ShowManager _Shows;
        public LibraryGeneration.Managers.ShowManager Shows
        {
            get
            {
                return this._Shows = this._Shows != null ? this._Shows : new LibraryGeneration.Managers.ShowManager(this.Manager);
            }
        }
    }
}