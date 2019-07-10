using Etch.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.Data {
    public class RequestUser : IRequestUser {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// Constructor used to create new instance of object.
        /// </summary>
        /// <param name="contextAccessor"></param>
        /// <param name="userManager"></param>
        public RequestUser(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager) {
            this.contextAccessor = contextAccessor;
            this.userManager = userManager;
        }

        /// <summary>
        /// Method used to return user Id of current user.
        /// </summary>
        /// <returns>String user Id.</returns>
        public string GetUserId() {
            return userManager.GetUserId(contextAccessor.HttpContext.User);
        }
    }
}
