using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RedarborWebApiTest.Models
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public string? CreatedOn { get; set; }

        public string? DeletedOn { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Fax { get; set; }

        public string? Name { get; set; }

        public string? LastLogin { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "The password has to be at least 4 characters")]
        [MaxLength(25)]
        public string Password { get; set; }

        [Required]
        public int PortalId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int StatusId { get; set; }

        public string? Telephone { get; set; }

        public string? UpdatedOn { get; set; }

        [Required]
        public string Username { get; set; }
    }
}

