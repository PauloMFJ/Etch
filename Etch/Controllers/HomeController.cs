using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Etch.Models;

namespace Etch.Controllers {
    public class HomeController : Controller {
        /// <summary>
        /// Method to get and return view of the index page.
        /// </summary>
        /// <returns>Index page view.</returns>
        public IActionResult Index() {
            return View();
        }

        /// <summary>
        /// Method used to handle errors.
        /// </summary>
        /// <returns>New error view containing error message.</returns>
        public IActionResult Error() {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
