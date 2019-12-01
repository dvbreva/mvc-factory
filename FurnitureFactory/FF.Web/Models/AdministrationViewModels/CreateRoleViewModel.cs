using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FF.Web.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }
    }
}
