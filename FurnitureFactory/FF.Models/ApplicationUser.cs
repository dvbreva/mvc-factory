using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FF.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Zipcode { get; set; }
    }
}