using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Etch.Models {
    public class ApplicationUser : IdentityUser {
        // User profile data
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public string Theme { get; set; }

        /// <summary>
        /// Get method used to convert theme string into theme enumerator type.
        /// </summary>
        public Themes ThemeEnum {
            get {
                Enum.TryParse<Themes>(Theme, out Themes theme);
                return theme;
            }
        }

        /// <summary>
        /// Theme type enumerator.
        /// </summary>
        public enum Themes {
            Default,
            Light,
            Dark,
            Agua,
            Sunrise,
            Berry,
            Pastel
        }
    }
}