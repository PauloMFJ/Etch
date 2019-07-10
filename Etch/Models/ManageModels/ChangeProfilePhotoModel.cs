using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.Models.ManageModels {
    public class ChangeProfilePhotoModel {
        // Profile photo field
        [Display(Name = "Profile Photo")]
        public byte[] ProfilePhoto { get; set; }

        // Status message
        public string StatusMessage { get; set; }
    }
}
