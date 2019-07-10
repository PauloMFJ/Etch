using Etch.Controllers;
using Etch.Data;
using Etch.Models.ScheduleModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Etch.Tests {
    public class SharedControllerUnitTests {
        private Mock<ITasksRepository> tasksRepository;
        private Mock<IRequestUser> requestUser;
        private ScheduleController controller;

        public SharedControllerUnitTests() {
            tasksRepository = new Mock<ITasksRepository>();
            requestUser = new Mock<IRequestUser>();
            controller = new ScheduleController(tasksRepository.Object, requestUser.Object);
        }

        // ------------------------------------ Index Testing ------------------------------------
        [Fact]
        public async Task IndexTest_ReturnsViewContainingTaskList() {
            // Arrange
            var tasksList = new List<GanttTask>
            {
                new GanttTask { Title = "Test 1" },
                new GanttTask { Title = "Test 2" }
            };
            tasksRepository.Setup(t => t.GetAll()).Returns(Task.FromResult(tasksList));

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<GanttTask>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        // ------------------------------------ Details Testing ------------------------------------
        [Fact]
        public async Task DetailsTest_ReturnsNotFound_WhenIdIsNullOrEmpty() {
            // Arrange
            string Id = "";

            // Act
            var result = await controller.Details(Id);

            // Assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DetailsTest_ReturnsNotFound_WhenTaskDoesNotExist() {
            // Arrange
            var Id = "25e7ce17-1377-44f3-8261-e0b3242da89f";
            tasksRepository.Setup(t => t.Find(Id)).Returns(Task.FromResult<GanttTask>(null));

            // Act
            var result = await controller.Details(Id);

            // Assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DetailsTest_ReturnsPartialViewContainingTaskModel_WhenTaskExists() {
            // Arrange
            var Id = "25e7ce17-1377-44f3-8261-e0b3242da89f";
            var task = new GanttTask { Title = "Test 1" };
            tasksRepository.Setup(t => t.Find(Id)).Returns(Task.FromResult(task));

            // Act
            var result = await controller.Details(Id);

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Equal(task, partialViewResult.ViewData.Model);
        }

        // ------------------------------------ Add Task Testing ------------------------------------
        [Fact]
        public void AddTaskTest_ReturnsTaskView() {
            // Act
            var result = controller.AddTask();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task AddTaskTest_ReturnsAddTaskView_WhenModelStateIsInvalid() {
            // Arrange
            var task = new GanttTask { Title = "Test 1" };
            controller.ModelState.AddModelError("Duration", "Please fill in this field.");

            // Act
            var result = await controller.AddTask(task);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(task, viewResult.ViewData.Model);
        }

        [Fact]
        public async Task AddTaskTest_ReturnsRedirectsToIndex_WhenTaskIsAddedToRepository() {
            // Arrange
            var task = new GanttTask { Title = "Test 1" };
            tasksRepository.Setup(t => t.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.AddTask(task);

            // Assert
            tasksRepository.Verify(t => t.Add(task));
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task AddTaskTest_SetsUserId_BeforeAddingTaskToRepository() {
            // Arrange
            var task = new GanttTask { Title = "Test 1" };
            var userId = "40e11bef-633c-44ff-aab1-af6c1bea6bf5";
            tasksRepository.Setup(t => t.Find(userId)).Returns(Task.FromResult(task));
            requestUser.Setup(m => m.GetUserId()).Returns(userId);

            // Act
            var result = await controller.AddTask(task);

            // Assert
            tasksRepository.Verify(t => t.Add(It.Is<GanttTask>(m => m == task && m.UserId == userId)));
        }

        // ------------------------------------ Update Testing ------------------------------------
        [Fact]
        public async Task UpdateTest_ReturnsUpdatePartialView_WhenModelStateIsInvalid() {
            // Arrange
            var task = new GanttTask { Title = "Test 1" };
            controller.ModelState.AddModelError("Duration", "Please fill in this field.");

            // Act
            var result = await controller.Update(task);

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Equal(task, partialViewResult.ViewData.Model);
        }

        [Fact]
        public async Task UpdateTest_ReturnsRedirectsToIndex_WhenTaskIsUpdated() {
            // Arrange
            var task = new GanttTask { Title = "Test 1" };
            tasksRepository.Setup(t => t.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.Update(task);

            // Assert
            tasksRepository.Verify(t => t.Update(task));
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task UpdateTest_SetsUserId_BeforeUpdatingTask() {
            // Arrange
            var task = new GanttTask { Title = "Test 1" };
            var userId = "40e11bef-633c-44ff-aab1-af6c1bea6bf5";
            tasksRepository.Setup(t => t.Find(userId)).Returns(Task.FromResult(task));
            requestUser.Setup(m => m.GetUserId()).Returns(userId);

            // Act
            var result = await controller.Update(task);

            // Assert
            tasksRepository.Verify(t => t.Update(It.Is<GanttTask>(m => m == task && m.UserId == userId)));
        }

        // ------------------------------------ Delete Testing ------------------------------------
        [Fact]
        public async Task DeleteTest_ReturnsNotFound_WhenIdIsNullOrEmpty() {
            // Arrange
            string Id = "";

            // Act
            var result = await controller.Delete(Id);

            // Assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteTest_ReturnsNotFound_WhenTaskDoesNotExist() {
            // Arrange
            var Id = "25e7ce17-1377-44f3-8261-e0b3242da89f";
            tasksRepository.Setup(t => t.Find(Id)).Returns(Task.FromResult<GanttTask>(null));

            // Act
            var result = await controller.Delete(Id);

            // Assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteTest_ReturnsRedirectsToIndex_WhenTaskIsDeletedFromRepository() {
            // Arrange
            var Id = "25e7ce17-1377-44f3-8261-e0b3242da89f";
            var task = new GanttTask { Title = "Test 1" };
            tasksRepository.Setup(t => t.Find(Id)).Returns(Task.FromResult(task));

            // Act
            var result = await controller.Delete(Id);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
    }
}
