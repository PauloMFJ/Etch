using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Etch.Models;
using System.IO;

namespace Etch.Controllers {
    [Authorize]
    [Route("[controller]/[action]")]
    public class SharedController : Controller {
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// Shared controller constructor method.
        /// </summary>
        public SharedController(UserManager<ApplicationUser> userManager) {
            this.userManager = userManager;
        }

        /// <summary>
        /// Method used to get users profile photo if it exists or a default one if not.
        /// </summary>
        /// <returns>Returns the contents of the binary file to the response.</returns>
        public async Task<FileContentResult> ProfilePhoto() {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");

            // If user doesnt have a set profile photo
            if (user.ProfilePhoto.Length < 1) {
                // Get default profile image
                var defaultFile = Path.Combine("~/assets/images/content/profile/default.png");
                var request = HttpContext.Request;
                var uriBuilder = new UriBuilder {
                    Host = request.Host.Host,
                    Scheme = request.Scheme
                };
                if (request.Host.Port.HasValue)
                    uriBuilder.Port = request.Host.Port.Value;

                // Convert default image into byte array
                FileInfo fileInfo = new FileInfo(uriBuilder.ToString());
                long imageFileLength = fileInfo.Length;
                FileStream stream = new FileStream(defaultFile, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(stream);
                byte[] profileData = reader.ReadBytes((int)imageFileLength);

                // Return default image
                return new FileContentResult(profileData, "image/png");
            }

            // Return profile photo image
            return new FileContentResult(user.ProfilePhoto, "image/jpeg");
        }

        /// <summary>
        /// Method used to change the users current theme.
        /// </summary>
        /// <param name="submit">String type of new selected theme to change to.</param>
        /// <returns>Redirect to dashboard view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeTheme(string submit) {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");

            // Update theme
            user.Theme = submit;
            await userManager.UpdateAsync(user);

            // Redirect to dashboard
            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }
    }
}