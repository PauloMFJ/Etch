using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Etch.Models;
using Etch.Models.ManageModels;
using System.IO;

namespace Etch.Controllers {
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Manage controller constructor method.
        /// </summary>
        public ManageController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// Method used to return manage index view.
        /// </summary>
        /// <returns>Manage index view.</returns>
        [HttpGet]
        public async Task<IActionResult> Index() {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");

            var model = new IndexModel {
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        /// <summary>
        /// Method used to return manage index view containing status message of result.
        /// </summary>
        /// <param name="model">Manage index model containing status message.</param>
        /// <returns>Manage index view containing status message.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexModel model) {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");

            StatusMessage = "Your profile has been updated!";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Method used to return change password view.
        /// </summary>
        /// <returns>Change password view.</returns>
        [HttpGet]
        public async Task<IActionResult> ChangePassword() {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");

            var model = new ChangePasswordModel { StatusMessage = StatusMessage };
            return View(model);
        }

        /// <summary>
        /// Method used to return change password view containing status message of result.
        /// </summary>
        /// <param name="model">Change password model containing updated password.</param>
        /// <returns>Returns redirect to change password view containing status message of result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model) {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded) {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        /// <summary>
        /// Method used to return change profile photo view.
        /// </summary>
        /// <returns>Change profile photo view.</returns>
        [HttpGet]
        public async Task<IActionResult> ChangeProfilePhoto() {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");

            var model = new ChangeProfilePhotoModel { StatusMessage = StatusMessage };
            return View(model);
        }


        /// <summary>
        /// Method used to return change password password view containing status message of result.
        /// </summary>
        /// <param name="model">Change profile photo model containing updated profile photo.</param>
        /// <returns>Returns redirect to change profile photo view containing status message of result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeProfilePhoto(ChangeProfilePhotoModel model) {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");

            // If image exists, then convert and store into byte array
            if (Request.Form.Files.Count > 0) {
                byte[] profileData = null;
                var file = Request.Form.Files["Profile Photo"];

                // Convert image to binary array
                using (var ms = new MemoryStream()) {
                    file.CopyTo(ms);
                    profileData = ms.ToArray();
                }
                
                // Update profile photo
                user.ProfilePhoto = profileData;
                var changeProfilePhotoResult = await userManager.UpdateAsync(user);
                if (!changeProfilePhotoResult.Succeeded) { 
                    AddErrors(changeProfilePhotoResult);
                    return View(model);
                }
            }


            await signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Your profile photo has been changed.";

            return RedirectToAction(nameof(ChangeProfilePhoto));
        }

        /// <summary>
        /// Method used to add errors to list if error occured.
        /// </summary>
        /// <param name="result">Result of task.</param>
        private void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
