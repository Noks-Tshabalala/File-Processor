using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using FileProcessor.Models.Results;
using System.Security.Principal;

namespace FileProcessor.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        [Display(Name = "Name")]
        [MinLength(1), MaxLength(100)]
        public string FullName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            //Custom user claims
            userIdentity.AddClaim(new Claim("FullName", this.FullName));
            return userIdentity;
        }
       
    }

    public static class GenericPrincipalExtensions
    {
        public static string GetFullName(this IPrincipal user)
        {
            var fullNameClaim = ((ClaimsIdentity)user.Identity).FindFirst("FullName");
            if (fullNameClaim != null)
            {
                return fullNameClaim.Value;
            }
            else
            {
                return "";
            }
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<FileDetail> FileDetails { get; set; }
        public DbSet<CalculationDetail> CalculationDetails { get; set; }
    }
}