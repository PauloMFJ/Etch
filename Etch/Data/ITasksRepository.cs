using System;
using System.Collections.Generic;
using System.Linq;
using Etch.Models;
using System.Threading.Tasks;
using Etch.Models.ScheduleModels;

namespace Etch.Data {
    public interface ITasksRepository {
        Task<List<GanttTask>> GetAll();

        Task<List<GanttTask>> GetLatest(int num);

        Task<GanttTask> Find(string id);

        void Add(GanttTask task);

        void Update(GanttTask task);

        void Remove(GanttTask task);

        Task SaveChanges();
    }
}
