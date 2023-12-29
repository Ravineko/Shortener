using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Shortener.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(255)]
        public string DrivingLicense { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }

/*        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note: The authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationScheme
            var userIdentity = await manager.GenerateUserIdentityAsync(this, IdentityConstants.ApplicationScheme);

            // Add custom user claims here

            return userIdentity;
        }*/

    }
}