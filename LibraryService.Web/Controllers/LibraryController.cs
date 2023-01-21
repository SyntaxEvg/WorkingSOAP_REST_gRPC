using LibraryService.Web.Models;
using LibraryServiceReference;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Web.Controllers
{
    public class LibraryController : Controller
    {
        public IActionResult Index(SearchType searchType, string searchString)
        {

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
