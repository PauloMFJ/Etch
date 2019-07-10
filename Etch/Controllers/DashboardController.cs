using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Owin;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Etch.Models;
using System.IO;
using Etch.Data;

namespace Etch.Controllers {
    [Authorize]
    [Route("[controller]/[action]")]
    public class DashboardController : Controller {
        private readonly ITasksRepository tasksRepository;
        private readonly IRequestUser requestUser;

        /// <summary>
        /// Constructor method used to initialize new dashboard object instance.
        /// </summary>
        /// <param name="tasksRepository">Tasks repository object.</param>
        /// <param name="requestUser">Request user object.</param>
        public DashboardController(ITasksRepository tasksRepository, IRequestUser requestUser) {
            this.tasksRepository = tasksRepository;
            this.requestUser = requestUser;
        }

        /// <summary>
        /// Method used to return schedule index view.
        /// </summary>
        /// <returns>Schedule index view.</returns>
        [HttpGet]
        public async Task<IActionResult> Index() {
            return View(await tasksRepository.GetLatest(7));
        }
    }
}