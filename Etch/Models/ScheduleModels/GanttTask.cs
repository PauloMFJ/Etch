using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.Models.ScheduleModels {
    public class GanttTask {
        public string GanttTaskId { get; set; }

        // Parent task field
        [Display(Name = "Parent Task")]
        public string ParentId { get; set; }

        // Title field
        [Required]
        [Display(Name = "Task")]
        [MaxLength(100)]
        public string Title { get; set; }

        // Type of task field
        [Display(Name = "Type Colour")]
        public string Type { get; set; }

        // Start date field
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        // Duration field
        [Required]
        [Display(Name = "Duration")]
        public int Duration { get; set; }

        // Progress field
        [Display(Name = "Progress")]
        public int Progress { get; set; }

        // User Id
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
