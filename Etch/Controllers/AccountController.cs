using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Etch.Models;
using Etch.Models.AccountModels;
using System.IO;

namespace Etch.Controllers {
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Account controller constructor method.
        /// </summary>
        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// Method used to return registration view.
        /// </summary>
        /// <param name="returnUrl">Url to return to after registering.</param>
        /// <returns>Register view.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Method used to register user.
        /// </summary>
        /// <param name="model">Register model containing user registration information.</param>
        /// <param name="returnUrl">Url to return to after registering.</param>
        /// <returns>Returns redirect to return Url or the registration view if something failed.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model, string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid) {
                // If image exists, then convert it and store into byte array
                byte[] profileData = null;
                if (Request.Form.Files.Count > 0) {
                    var file = Request.Form.Files["Profile Photo"];

                    using (var ms = new MemoryStream()) {
                        file.CopyTo(ms);
                        profileData = ms.ToArray();
                    }
                }

                // Create new application user based on registration data
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email,
                    FirstName = model.FirstName, LastName = model.LastName, ProfilePhoto = profileData,
                    Theme = ApplicationUser.Themes.Default.ToString() };

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToReturnUrl(returnUrl);
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Method used to return login view.
        /// </summary>
        /// <param name="returnUrl">Url to return to after loging in.</param>
        /// <returns>Login view.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null) {
            // Clear any existing external cookies to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Method used to login user.
        /// </summary>
        /// <param name="model">Login model containing user login information.</param>
        /// <param name="returnUrl">Url to return to after loging in.</param>
        /// <returns>Returns redirect to return Url or the login view if something failed.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid) {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                    return RedirectToReturnUrl(returnUrl);
                else {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. Please Try again.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Method used to return logout view.
        /// </summary>
        /// <returns>Logout view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// Method used to return forgot password view.
        /// </summary>
        /// <returns>Forgot password view.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword() {
            return View();
        }

        /// <summary>
        /// Method used to redirect to forgot password screen.
        /// </summary>
        /// <param name="model">Forgot password model containing email information.</param>
        /// <returns>Returns redirect to forgot password confirmation or the forgot password view if something failed.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model) {
            if (ModelState.IsValid) {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user))) {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // Redirect to forgot password
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Method used to return forgot password confirmation view.
        /// </summary>
        /// <returns>Forgot password confirmation view.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation() {
            return View();
        }

        /// <summary>
        /// Method used to return reset password view.
        /// </summary>
        /// <returns>Reset password view.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null) {
            if (code == null)
                throw new ApplicationException("A code must be supplied for password reset.");

            var model = new ResetPasswordModel { Code = code };
            return View(model);
        }

        /// <summary>
        /// Method used to reset password.
        /// </summary>
        /// <param name="model">Reset password model containing new password information.</param>
        /// <returns>Returns redirect to reset password confirmation or the reset password view if something failed.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model) {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
                return RedirectToAction(nameof(ResetPasswordConfirmation));

            AddErrors(result);
            return View();
        }

        /// <summary>
        /// Method used to return reset password confirmation view.
        /// </summary>
        /// <returns>Reset password confirmation view.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation() {
            return View();
        }

        /// <summary>
        /// Method used to add errors to list if error occured.
        /// </summary>
        /// <param name="result">Result of task.</param>
        private void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        /// <summary>
        /// Method to redirect the user to the previous URL.
        /// </summary>
        /// <param name="returnUrl">Previous URL.</param>
        /// <returns>Redirect to return URL.</returns>
        private IActionResult RedirectToReturnUrl(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
