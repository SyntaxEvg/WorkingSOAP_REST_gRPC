using LibraryService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LibraryService.Services.Impl
{
    public class LibraryDatabaseContext : ILibraryDatabaseContextService
    {

        private IList<Book> _libraryDatabase;

        public IList<Book> Books { get { return _libraryDatabase; } }


        public LibraryDatabaseContext()
        {
            Initialize();
        }

        private void Initialize()
        {
            _libraryDatabase = JsonConvert.DeserializeObject<List<Book>>(Encoding.UTF8.GetString(LibraryService.Properties.Resources.books));
        }


    }
}