using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibraryService.Web.Controllers
{
    public class ClinicServController : Controller
    {
        private readonly ILogger<ClinicServController> logger;
        private readonly IClinicClient _clinicClient;

        public ClinicServController(ILogger<ClinicServController> logger, IClinicClient clinicClient )
        {
            this.logger = logger;
            this._clinicClient = clinicClient;
        }

        public IActionResult Index(SearchType searchType, string searchString)
        {
            logger.LogInformation(nameof(ClinicServController));
            LibraryWebServiceSoapClient client =
                new LibraryWebServiceSoapClient(LibraryWebServiceSoapClient.EndpointConfiguration.LibraryWebServiceSoap);


            if (!string.IsNullOrEmpty(searchString) && searchString.Length >= 3)
                switch (searchType)
                {
                    case SearchType.Title:
                        return View(new BookCategoryViewModel() { Books = client.GetBooksByTitle(searchString) });
                    case SearchType.Category:
                        return View(new BookCategoryViewModel() { Books = client.GetBooksByCategory(searchString) });
                    case SearchType.Author:
                        return View(new BookCategoryViewModel() { Books = client.GetBooksByAuthor(searchString) });
                }

            return View(new BookCategoryViewModel() { Books = new Book[] { } });
        }
    }
}
