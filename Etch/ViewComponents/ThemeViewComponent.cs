using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Etch.Models;

namespace Etch.Controllers {
    public class ThemeViewComponent : ViewComponent {
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// Background controller constructor method.
        /// </summary>
        public ThemeViewComponent(UserManager<ApplicationUser> userManager) {
            this.userManager = userManager;
        }

        /// <summary>
        /// Method used to get the current users selected background colour.
        /// </summary>
        /// <returns>Returns theme model containing theme model of current signed in user.</returns>
        public async Task<IViewComponentResult> InvokeAsync() {
            var user = await userManager.GetUserAsync(HttpContext.User);
            
            // Create model with default theme
            ThemeModel model = new ThemeModel { Theme = ApplicationUser.Themes.Default };

            // If user exists set theme
            if (user != null)
                model.Theme = user.ThemeEnum;

            // Return background view containing model
            return View("~/Views/Shared/_Theme.cshtml", model);
        }
    }
}
