using Interview.Round3.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Interview.Round3.Web.Core;
using Interview.Round3.Web.Models;

namespace Interview.Round3.Web.Controllers
{
    public class SalesController : Controller
    {
        #region Member Variables
        private readonly SalesTerritoryRepository _repo;
        #endregion

        #region Constructor
        public SalesController(AppSettings settings)
        {
            // AppSettings is injected via MVC's inversion of control container.
            // The AppSettings object is setup in Startup.cs

            _repo = new SalesTerritoryRepository(settings.ConnectionString);
        }
        #endregion

        public IActionResult Territories()
        {
            // I prefer to use a functional approach to converting data from the
            // domain model into a ViewModel.  You do not have to take the same
            // approach.  If you are more comfortable with looping, use that.
            
            return View(
                _repo                                           // <- Using the repository
                    .GetAllTerritories()                        //    get all territories.
                    .SelectMany(                                // <- For each territory
                        t => t.Geographies.Select(              //    get all realted geographies.
                            g => new SalesTerritoryViewModel    // <- Convert the domain model to the viewmodel.
                            {
                                SalesTerritoryId = t.Id,
                                GeographyId = g.Id,
                                City = g.City,
                                Region = t.Region,
                                Country = t.Country
                            })).ToList());
        }
    }
}