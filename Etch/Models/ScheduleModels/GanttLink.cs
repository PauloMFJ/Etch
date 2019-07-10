using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.Models.ScheduleModels {
    public class GanttLink {
        // Data representing a single link between two tasks
        public string GanttLinkId { get; set; }
        [Required]
        public string SourceTaskId { get; set; }
        [Required]
        public string TargetTaskId { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }

        // User Id
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
