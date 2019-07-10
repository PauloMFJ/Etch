using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etch.Models;
using Etch.Models.ScheduleModels;
using Microsoft.EntityFrameworkCore;

namespace Etch.Data {
    public class TasksRepository : ITasksRepository {
        private readonly ApplicationDbContext context;
        private readonly IRequestUser requestUser;

        public TasksRepository(ApplicationDbContext context, IRequestUser requestUser) {
            this.context = context;
            this.requestUser = requestUser;
        }

        public Task<List<GanttTask>> GetAll() =>
            context.GanttTask.Where(m => m.UserId == requestUser.GetUserId()).OrderBy(m => m.StartDate).ToListAsync();

        public Task<List<GanttTask>> GetLatest(int num) =>
            context.GanttTask.Where(m => m.UserId == requestUser.GetUserId()).OrderBy(m => m.StartDate).Take(num).ToListAsync();

        public Task<GanttTask> Find(string id) =>
            context.GanttTask.Where(m => m.UserId == requestUser.GetUserId()).SingleOrDefaultAsync(m => m.GanttTaskId.Equals(id));

        public void Add(GanttTask task) => context.GanttTask.Add(task);

        public void Update(GanttTask task) => context.GanttTask.Update(task);

        public void Remove(GanttTask task) => context.GanttTask.Remove(task);

        public Task SaveChanges() => context.SaveChangesAsync();
    }
}
