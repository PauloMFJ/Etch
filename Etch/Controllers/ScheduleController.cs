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
using Etch.Models.ScheduleModels;
using System.IO;
using System.Threading;
using System.Globalization;
using Etch.Data;

namespace Etch.Controllers {
    [Authorize]
    [Route("[controller]/[action]")]
    public class ScheduleController : Controller {
        private readonly ITasksRepository tasksRepository;
        private readonly IRequestUser requestUser;

        /// <summary>
        /// Constructor method used to initialize new schedule object instance.
        /// </summary>
        /// <param name="tasksRepository">Tasks repository object.</param>
        /// <param name="requestUser">Request user object.</param>
        public ScheduleController(ITasksRepository tasksRepository, IRequestUser requestUser) {
            this.tasksRepository = tasksRepository;
            this.requestUser = requestUser;
        }

        /// <summary>
        /// Method used to return schedule index view.
        /// </summary>
        /// <returns>Schedule index view.</returns>
        [HttpGet]
        public async Task<IActionResult> Index() {
            return View(await tasksRepository.GetAll());
        }

        /// <summary>
        /// Method used to return partial view containing details corresponding to passed in Id.
        /// </summary>
        /// <param name="id">String Id of task to determine.</param>
        /// <returns>Details partial view.</returns>
        public async Task<IActionResult> Details(string id) {
            if (String.IsNullOrEmpty(id))
                return NotFound();

            var task = await tasksRepository.Find(id);
            if (task == null)
                return NotFound();

            return PartialView("_Details", task);
        }

        /// <summary>
        /// Method used to return add task view.
        /// </summary>
        /// <returns>Add task view.</returns>
        [Authorize]
        public IActionResult AddTask() {
            return View();
        }

        /// <summary>
        /// Method used to return add task view containing status message of result.
        /// </summary>
        /// <param name="model">Gantt task model containing task information.</param>
        /// <returns>Redirect to schedule index view or add task view if failed.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddTask(GanttTask task) {
            if (ModelState.IsValid) {
                task.UserId = requestUser.GetUserId();
                tasksRepository.Add(task);
                await tasksRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(task);
        }

        /// <summary>
        /// Method used to update existing task.
        /// </summary>
        /// <param name="model">Gantt task model containing updated task information.</param>
        /// <returns>Redirect to schedule index view or index if failed.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Update(GanttTask task) {
            if (ModelState.IsValid) {
                task.UserId = requestUser.GetUserId();
                tasksRepository.Update(task);
                await tasksRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(task);
        }

        /// <summary>
        /// method used to delete task from repository.
        /// </summary>
        /// <param name="id">Task Id.</param>
        /// <returns>Returns current view without task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(string id) {
            if (String.IsNullOrEmpty(id))
                return NotFound();

            var task = await tasksRepository.Find(id);
            if (task == null)
                return NotFound();

            tasksRepository.Remove(task);
            await tasksRepository.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}